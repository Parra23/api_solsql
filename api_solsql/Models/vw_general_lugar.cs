namespace api_solsql.Models
{
    public class vw_general_lugar
    {
        public int place_id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string? address { get; set; }
        public string? opening_hours { get; set; }
        public string? tipo_lugar { get; set; }
        public string ciudad { get; set; }
        public string? url_foto { get; set; }
    }
}
