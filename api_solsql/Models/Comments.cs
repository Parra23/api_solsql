using System.ComponentModel.DataAnnotations;

namespace api_solsql.Models
{
    public class Comments
    {
        [Key]
        public int comment_id { get; set; }

        public int place_id { get; set; }
        public int id { get; set; }
        public string? comment { get; set; }
        public int? parent_comment_id { get; set; }
        public DateTime? comment_date { get; set; }
    }
}
