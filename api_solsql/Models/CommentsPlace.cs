namespace api_solsql.Models
{
    public class CommentsPlace
    {
        public int comment_id { get; set; }
        public int? parent_comment_id { get; set; }
        public string? name { get; set; }
        public string? comment { get; set; }
        public DateTime? comment_date { get; set; }

    }
}
