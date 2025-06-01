namespace VehicleProject.Vehicles;

public class Supercar(string model, int year, string color) : Vehicle(model, year, color)
{
    public override int MaxSpeed { get; set; } = Rand.Next(200, 301);
    public override int MaxEnergy { get; set; } = Rand.Next(60, 81);
    public override int AccelerateSpeedIncrement { get; set; } = Rand.Next(25, 41);
    public override int DecelerateSpeedDecrement { get; set; } = Rand.Next(35, 51);
    public override int CoastingSpeedDecrement { get; set; } = Rand.Next(4, 8);
    public override int EnergyConsumptionRate { get; set; } = Rand.Next(9, 14);

    public void SuperBoost()
    {
        if (!IsEnabled)
        {
            Console.WriteLine($"{Model} cannot super boost: Vehicle not enabled\n");
            return;
        }

        var randomNumber = Rand.Next(101);

        Console.WriteLine($"{Model} is superboosting...");
        LongAction();
        if (randomNumber < 30)
        {
            Console.WriteLine($"Oh noes, {Model} superboost failed !!");
            IsOperational = false;
            IsEnabled = false;
            CurrentSpeed = 0;
            IsMoving = false;
            MediumAction();
            Console.WriteLine($"{Model} explodes, resulting in you and your cars unfortunate demise :c");
            return;
        }

        Console.WriteLine($"{Model} superboost succeeded !!");
        IsMoving = true;
        CurrentSpeed += AccelerateSpeedIncrement * 10;
        MediumAction();
        Console.WriteLine($"{Model} superboosted:\n{GetStats()}");
    }
}