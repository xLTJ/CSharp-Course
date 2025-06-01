namespace VehicleProject.Vehicles;

public class Truck(string model, int year, string color) : Vehicle(model, year, color)
{
    public override int MaxSpeed { get; } = Rand.Next(90, 121);
    public override int MaxEnergy { get; } = Rand.Next(150, 201);
    public override int AccelerateSpeedIncrement { get; } = Rand.Next(7, 12);
    public override int DecelerateSpeedDecrement { get; } = Rand.Next(12, 19);
    public override int CoastingSpeedDecrement { get; } = Rand.Next(4, 7);
    public override int EnergyConsumptionRate { get; } = Rand.Next(8, 12);
}