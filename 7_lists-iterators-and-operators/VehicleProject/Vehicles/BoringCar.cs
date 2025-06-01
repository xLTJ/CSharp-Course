namespace VehicleProject.Vehicles;

public class BoringCar(string model, int year, string color) : Vehicle(model, year, color)
{
    public override int MaxSpeed { get; } = Rand.Next(120, 161);
    public override int MaxEnergy { get; } = Rand.Next(60, 91);
    public override int AccelerateSpeedIncrement { get; } = Rand.Next(15, 21);
    public override int DecelerateSpeedDecrement { get; } = Rand.Next(20, 31);
    public override int CoastingSpeedDecrement { get; } = Rand.Next(3, 6);
    public override int EnergyConsumptionRate { get; } = Rand.Next(6, 9);

    public void Honk()
    {
        Console.WriteLine("...beep");
    }
}