namespace VehicleProject.Vehicles;

public class BoringCar(string model, int year, string color) : Vehicle(model, year, color)
{
    protected override int MaxSpeed { get; } = Rand.Next(120, 161);
    protected override int MaxEnergy { get; } = Rand.Next(60, 91);
    protected override int AccelerateSpeedIncrement { get; } = Rand.Next(15, 21);
    protected override int DecelerateSpeedDecrement { get; } = Rand.Next(20, 31);
    protected override int CoastingSpeedDecrement { get; } = Rand.Next(3, 6);
    protected override int EnergyConsumptionRate { get; } = Rand.Next(6, 9);

    public void Honk()
    {
        Console.WriteLine("...beep");
    }
}