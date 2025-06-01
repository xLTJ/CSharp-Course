using VehicleProject.Vehicles;

namespace Shared;

public record CommandRequest(CommandType CommandType, string[] Arguments);

public record CommandResponse(bool Success, string Message, object? Data = null);

public record BicycleData(
    int Index,
    string Model,
    string Color,
    int Year,
    int MaxSpeed,
    int MaxEnergy,
    int EnergyConsumptionRate)
{
    public static BicycleData FromBicycle(Bicycle bicycle, int index)
        => new(index, bicycle.Model, bicycle.Color, bicycle.Year, bicycle.MaxSpeed, bicycle.MaxEnergy, bicycle.EnergyConsumptionRate);
}

public enum CommandType
{
    Help,
    List,
    View,
    Search,
    Add,
    Remove,
    Update,
    Exit,
    Unknown
}