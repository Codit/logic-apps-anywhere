using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Workflows.WebJobs.Extensions.Trigger;
using Newtonsoft.Json.Linq;
using Sello.Model;
using System.Net.Http;
using System.Text;
using System.Configuration;

namespace Company.Function
{
    public static class SelloShipmentFunction
    {
        [FunctionName("SelloShipmentFunction")]
        public static JToken Run(
           [WorkflowActionTrigger] JToken parameters,
           ILogger log)
        {
            var shipmentRequest = parameters.Root.ToObject<ShipmentRequest>();

            //Update Stock
            foreach (var item in shipmentRequest.ShipmentItems)
            {
                using (var httpClient = new HttpClient())
                {
                    var stockUpdateRequest = new StockUpdateRequest()
                    {
                        ItemId = item.ItemId,
                        Qty = item.Qty * -1
                    };
                    var content = new StringContent(JsonConvert.SerializeObject(stockUpdateRequest), Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync(System.Environment.GetEnvironmentVariable("warehouse_setstockurl"), content).Result;
                }
            }
            

            return new JObject { { "Message", $"Shipment for order: {shipmentRequest.OrderId} succeeded" } };
        }
    }
}
