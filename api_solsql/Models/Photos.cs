using System.ComponentModel.DataAnnotations;

namespace api_solsql.Models
{
    public class Photos
    {
        [Key]
        public int photo_id { get; set; }

        public int place_id { get; set; }
        public string? url { get; set; }
        public string? description { get; set; }
    }
}
