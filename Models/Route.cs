using System.ComponentModel.DataAnnotations;

namespace fleet_tracking.Models;
public class Route
{
    public int Id { get; set; }
    [Required] 
    public string Name { get; set; }
}