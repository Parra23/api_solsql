namespace api_solsql.Models;

public class LoginRequest
{
	public int Id { get; set; }
	public string? Name { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public int Role { get; set; }
}