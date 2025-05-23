namespace api_solsql.Models;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class PlaceTypes
{
	[Key]
	[Column("type_id")]
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
}