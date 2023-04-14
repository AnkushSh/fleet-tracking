using System.ComponentModel.DataAnnotations;

namespace fleet_tracking.Models;
public class Vehicle
{
    public int Id { get; set; }
    [Required] 
    public string VehicleName { get; set; }
    [Required] 
    public VehicleStatus Status { get; set; }
    public int? RouteId { get; set; }

    public Route Route { get; set; }
}

public enum VehicleStatus
{
    OutOfDepot = 0,
    OnRoute = 1,
    OutOfService = 2,
    OnDestination = 3
}
