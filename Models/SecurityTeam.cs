using System.ComponentModel.DataAnnotations;

namespace fleet_tracking.Models;
public class SecurityTeam
{
    public int Id { get; set; }
    [Required] 
    public string TeamName { get; set; }
    public int? VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
}
