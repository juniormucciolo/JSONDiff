using System.Collections.Generic;

namespace JsonDiff.Models
{
    /// <summary>
    /// JsonResult is send as diff response.
    /// </summary>
    public class JsonResult
    {
        public string id { get; set; }
        public string message { get; set; }
        public List<string> differences { get; set; }
    }
}