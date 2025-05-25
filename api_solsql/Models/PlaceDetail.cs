namespace api_solsql.Models
{
    public class PlaceDetail
    {
        public int place_id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public string? address { get; set; }
        public string? opening_hours { get; set; }
        public string? fees { get; set; }
        public string? coordinates { get; set; }
        public string contact_phone { get; set; }
        public string contact_email { get; set; }
        public string? social_media { get; set; }
        public string? tipo_lugar { get; set; }
        public string? ciudad { get; set; }
        public int? es_favorito { get; set; }
        public string? tipo_reaccion { get; set; }
    }
}
