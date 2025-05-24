namespace api_solsql.Models;
public class vw_logs
{
	public int Id { get; set; }
	public string? User_name { get; set; }
	public string? Table_name { get; set; }
	public string? Action_type { get; set; }
	public string? description { get; set; }
	public DateTime? action_timestamp { get; set; }
}