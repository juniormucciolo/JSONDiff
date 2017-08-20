using System.ComponentModel.DataAnnotations;

namespace JsonDiff.Models
{
    /// <summary>
    /// Base model for code-first migrations.
    /// </summary>
    public class Json
    {
        [Required]
        public int Id { get; set; }
        public string JsonId { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }
    }
}