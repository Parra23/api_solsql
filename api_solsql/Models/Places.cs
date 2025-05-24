using System.ComponentModel.DataAnnotations;

namespace api_solsql.Models
{
    public class Places
    {
        [Key]
        public int place_id { get; set; }

        public string name { get; set; }
        public int type_id { get; set; }
        public string? name_type { get; set; }
        public string? description { get; set; }
        public string? address { get; set; }
        public int city_id { get; set; }
        public string? name_city { get; set; }
        public string? opening_hours { get; set; }
        public string? fees { get; set; }
        public string? coordinates { get; set; }
        public string? contact_phone { get; set; }
        public string? contact_email { get; set; }
        public string? social_media { get; set; }
        public int? status { get; set; }
        public DateTime? creation_date { get; set; }
    }
}
