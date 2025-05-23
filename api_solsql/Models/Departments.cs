using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_solsql.Models;
public class Departments
{
    [Key]
    [Column("department_id")]
    public int id { get; set; }
    public string? Name { get; set; }
}