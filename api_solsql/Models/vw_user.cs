namespace api_solsql.Models;

public class vw_user
{
 	public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    public int Status { get; set; }
    public DateTime Created_at { get; set; }
}