
using Microsoft.AspNetCore.Identity;

namespace LabProject.Models
{
public class ApplicationUser  : IdentityUser
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; } = true ;
    public DateTime CreatedAt { get; set; }
}
}