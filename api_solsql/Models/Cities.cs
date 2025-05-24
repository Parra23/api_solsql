namespace api_solsql.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Cities
{
	[Key]
	[Column("city_id")]
	public int Id { get; set; }
	public string? Name_city { get; set; }
	[Column("department_id")]
	public int Department_id { get; set; }
	public string? Name_department { get; set; }
} 