namespace VehicleProject.Vehicles;

public class Truck(string model, int year, string color) : Vehicle(model, year, color)
{
    protected override int MaxSpeed { get; } = Rand.Next(90, 121);
    protected override int MaxEnergy { get; } = Rand.Next(150, 201);
    protected override int AccelerateSpeedIncrement { get; } = Rand.Next(7, 12);
    protected override int DecelerateSpeedDecrement { get; } = Rand.Next(12, 19);
    protected override int CoastingSpeedDecrement { get; } = Rand.Next(4, 7);
    protected override int EnergyConsumptionRate { get; } = Rand.Next(8, 12);
}