using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JsonDiff.Models
{
    public class JsonResult
    {
        [Required]
        public string id { get; set; }
        public string message { get; set; }
        public List<string> differences { get; set; }
    }
}