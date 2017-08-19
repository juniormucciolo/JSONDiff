using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonDiff.Service
{
    public class EncodeHandler
    {
        /// <summary>
        /// Decodes base64 string.
        /// </summary>
        /// <param name="encodedString">A string contaning json base63 encoded.</param>
        /// <returns>A string contaning json base64 string decoded.</returns>
        public string Decode(string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Deserializes base64 string to JObject.
        /// </summary>
        /// <param name="encodedString">A string contaning json base63 encoded</param>
        /// <returns>Returns json object from base64 string.</returns>
        public JObject DeserializeJson(string encodedString)
        {
            return JsonConvert.DeserializeObject<JObject>(Decode(encodedString));
        }
    }
}