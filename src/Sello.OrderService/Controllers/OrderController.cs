using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Sello.Model;
using Sello.OrderService.Model;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.Extensions.Configuration;

namespace Sello.OrderService.Controllers
{
    [ApiController]
    [Route("api/v1/order")]
    public class OrderController : ControllerBase
    {
        private IConfiguration _configuration;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost(Name = "Order_Post")]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Boolean), StatusCodes.Status503ServiceUnavailable)]
        [SwaggerResponseHeader(200, "RequestId", "string", "The header that has a request ID that uniquely identifies this operation call")]
        [SwaggerResponseHeader(200, "X-Transaction-Id", "string", "The header that has the transaction ID is used to correlate multiple operation calls.")]
        public async Task<IActionResult> Create(Order order)
        {
            bool isValidOrder = true;
            foreach (var orderItem in order.OrderItems)
            {
                using (HttpClient client = new HttpClient())
                {
                    var stockRequest = new StockRequest() { ItemId = orderItem.ItemId };

                    var content = new StringContent(JsonConvert.SerializeObject(stockRequest), Encoding.UTF8, "application/json");
                    var response = client.PostAsync(_configuration["StockService"], content).Result;

                    var stockResponse = JsonConvert.DeserializeObject<StockResponse>(await response.Content.ReadAsStringAsync());

                    if (orderItem.Qty > stockResponse.Stock)
                    {
                        isValidOrder = false;
                        break;
                    }
                }

            }

            if (isValidOrder)
            {
                
                //Send shipment
                using (HttpClient client = new HttpClient())
                {
                    var shipmentRequest = new ShipmentRequest()
                    {
                        CustomerEmail = order.Customer.Email,
                        CustomerName = order.Customer.Name,
                        OrderId = order.OrderNr,
                        ShipmentItems = order.OrderItems.Select(o => new ShipmentItem() { ItemId = o.ItemId, Qty = o.Qty }).ToList()
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(shipmentRequest), Encoding.UTF8, "application/json");
                    var result = await client.PostAsync(_configuration["ShipmentService"], content);
                }


                return Ok();
            }
            else
                return BadRequest("No stock available");

        }
    }
}