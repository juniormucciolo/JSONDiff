using System.ComponentModel.DataAnnotations;

namespace JsonDiff.Models
{
    public class Json
    {
        [Required]
        public int Id { get; set; }
        public string JsonId { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
    }
}