namespace VehicleProject.Vehicles;

public class Truck(string model, int year, string color) : Vehicle(model, year, color)
{
    public override int MaxSpeed { get; set; } = Rand.Next(90, 121);
    public override int MaxEnergy { get; set; } = Rand.Next(150, 201);
    public override int AccelerateSpeedIncrement { get; set; } = Rand.Next(7, 12);
    public override int DecelerateSpeedDecrement { get; set; } = Rand.Next(12, 19);
    public override int CoastingSpeedDecrement { get; set; } = Rand.Next(4, 7);
    public override int EnergyConsumptionRate { get; set; } = Rand.Next(8, 12);
}