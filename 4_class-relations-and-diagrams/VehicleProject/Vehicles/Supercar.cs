namespace VehicleProject.Vehicles;

public class Supercar(string model, int year, string color) : Vehicle(model, year, color)
{
    protected override int MaxSpeed { get; } = Rand.Next(200, 301);
    protected override int MaxEnergy { get; } = Rand.Next(60, 81);
    protected override int AccelerateSpeedIncrement { get; } = Rand.Next(25, 41);
    protected override int DecelerateSpeedDecrement { get; } = Rand.Next(35, 51);
    protected override int CoastingSpeedDecrement { get; } = Rand.Next(4, 8);
    protected override int EnergyConsumptionRate { get; } = Rand.Next(9, 14);

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