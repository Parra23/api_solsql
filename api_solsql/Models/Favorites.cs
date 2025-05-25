using System.ComponentModel.DataAnnotations;

namespace api_solsql.Models
{
    public class Favorites
    {
        [Key]
        public int favorite_id { get; set; }

        public int id { get; set; }
        public string? name_user { get; set; }
        public int place_id { get; set; }
        public string? name_place { get; set; }
        public DateTime? added_date { get; set; }
    }
}
