namespace api_solsql.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Cities
{
	[Key]
	[Column("city_id")]
	public int Id { get; set; }
	public string? Name { get; set; }
	public int department_id { get; set; }
}