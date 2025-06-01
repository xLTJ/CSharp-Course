namespace VehicleProject.Vehicles;

public class BoringCar(string model, int year, string color) : Vehicle(model, year, color)
{
    public override int MaxSpeed { get; set; } = Rand.Next(120, 161);
    public override int MaxEnergy { get; set; } = Rand.Next(60, 91);
    public override int AccelerateSpeedIncrement { get; set; } = Rand.Next(15, 21);
    public override int DecelerateSpeedDecrement { get; set; } = Rand.Next(20, 31);
    public override int CoastingSpeedDecrement { get; set; } = Rand.Next(3, 6);
    public override int EnergyConsumptionRate { get; set; } = Rand.Next(6, 9);

    public void Honk()
    {
        Console.WriteLine("...beep");
    }
}