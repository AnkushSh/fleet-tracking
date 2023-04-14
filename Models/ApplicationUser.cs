namespace fleet_tracking.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    [Required] 
    public string FirstName { get; set; }
    [Required] 
    public string LastName { get; set; }
    [Required] 
    public string Role { get; set; }
}