using System.Reflection;
using API.Controllers.Database.Information.Table.Process;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API.Controllers.Code
{
    public class GetProcessFunctionController : ControllerBase
    {
        public GetProcessFunctionController()
        {

        }

        internal string[] GetProcessFunction(JObject json)
        {
            if (json == null
                || !json.ContainsKey("Process")
                || json["Process"] == null
                || string.IsNullOrWhiteSpace(json["Process"].ToString())
                || !Guid.TryParse(json["Process"].ToString(), out Guid processGuid))
            {
                return [];
            }

            var processController = new ProcessController();
            var processEntity = processController.GetActiveEntityByGuid(processGuid);

            if (processEntity == null)
            {
                return [];
            }

            var processAttributeController = new ProcessAttributeController();
            var namespaceProcessAttributeEntity = processAttributeController.GetActiveEntityByDescription("Namespace");
            var controllerProcessAttributeEntity = processAttributeController.GetActiveEntityByDescription("Controller");
            var functionProcessAttributeEntity = processAttributeController.GetActiveEntityByDescription("Function");

            var processDetailController = new ProcessDetailController();
            var namespaceProcessDetailEntity = processDetailController.GetActiveEntityByIdAndAttributeId(processEntity.Id, namespaceProcessAttributeEntity.Id);
            var controllerProcessDetailEntity = processDetailController.GetActiveEntityByIdAndAttributeId(processEntity.Id, controllerProcessAttributeEntity.Id);
            var functionProcessDetailEntity = processDetailController.GetActiveEntityByIdAndAttributeId(processEntity.Id, functionProcessAttributeEntity.Id);

            string[] functionArray = [namespaceProcessDetailEntity.Description, controllerProcessDetailEntity.Description, functionProcessDetailEntity.Description];
            return functionArray;
        }

        internal IActionResult ProcessFunction(string[] functionArray, JObject json)
        {
            var controller = $"{functionArray[0]}.{functionArray[1]}";
            object[] parameterArray = [json];
            Type[] parameterTypeArray = [typeof(JObject)];
            Type rawType = Type.GetType(controller);

            if (rawType == null)
            {
                return BadRequest(new { error = "Invalid JSON format", details = $"Type '{controller}' not found." });
            }

            var function = functionArray[2];
            var instance = Activator.CreateInstance(rawType);

            // Get the method with correct parameters
            MethodInfo method = rawType.GetMethod(
                function,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                parameterTypeArray,
                null
            );

            if (method == null)
            {
                return BadRequest(new { error = "Invalid JSON format", details = $"Method '{function}' not found." });
            }

            // Store the return value
            var result = method.Invoke(instance, parameterArray);

            return result as IActionResult;
        }
    }
}
