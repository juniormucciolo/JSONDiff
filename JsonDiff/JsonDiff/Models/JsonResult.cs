using System.Collections.Generic;

namespace JsonDiff.Models
{
    public class JsonResult
    {
        public string id { get; set; }
        public string message { get; set; }
        public List<string> differences { get; set; }
    }
}