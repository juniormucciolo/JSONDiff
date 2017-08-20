using System.Collections.Generic;
using JsonDiff.Models;
using Newtonsoft.Json.Linq;

namespace JsonDiff.Service
{
    /// <summary>
    /// DiffHandler is responsible to perform differentiations between two JSON.
    /// </summary>
    public class DiffHandler
    {
        /// <summary>
        /// Process both side and find the differences.
        /// </summary>
        /// <param name="jsonById">Json object that contains two Json sides</param>
        /// <returns>Array of differences from two json.</returns>
        public JsonResult ProcessDiff(Json jsonById)
        {
            var diffList = new List<string>();
            var decoder = new EncodeHandler();
            var counter = 0;
            var message = "";

            // Gets both side json.
            var leftSide = decoder.DeserializeJson(jsonById.Left);
            var righSide = decoder.DeserializeJson(jsonById.Right);

            // Get boolean variables for equality and size.
            var isJsonLenghEqual = leftSide.Count == righSide.Count;
            var isJsonValuesEquals = JToken.DeepEquals(leftSide, righSide);

            if (!isJsonLenghEqual)
            {
                message = "Json lenght is not equal.";
            }

            if (isJsonValuesEquals && isJsonLenghEqual)
            {
                message = "Objects are same";
            }

            // Check json side against each other.
            if (!isJsonValuesEquals && isJsonLenghEqual)
            {
                foreach (KeyValuePair<string, JToken> property in leftSide)
                {
                    // Go through props and values to check the differences.
                    JProperty targetProp = righSide.Property(property.Key);

                    if (!JToken.DeepEquals(property.Value, targetProp.Value))
                    {
                        // Add differences found.
                        diffList.Add($"value from {property.Key} property changed from {property.Value} to {targetProp.Value}");
                        counter += 1;
                    }
                }

                message = $"Found {counter} differences between jsons";
            }

            var jsonResult = new JsonResult
            {
                message = message,
                differences = diffList,
                id = jsonById.JsonId
            };

            return jsonResult;
        }
    }
}