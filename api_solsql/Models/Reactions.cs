namespace api_solsql.Models
{
    public class Reactions
    {
        
        public int id { get; set; }
        public int user_id { get; set; }
        public string? name_user { get; set; }
        public int place_id { get; set; }
        public string? name_place { get; set; }
        public string reaction_type { get; set; }
        public DateTime? reaction_date { get; set; }
    }
}
