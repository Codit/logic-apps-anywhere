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
            return new JObject { { "Message", $"Shipment for order: {shipmentRequest.OrderId} succeeded" } };
        }
    }
}
