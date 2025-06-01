namespace VehicleProject.Vehicles;

public class Bicycle(string model, int year, string color) : Vehicle(model, year, color)
{
    public override int MaxSpeed { get; } = Rand.Next(25, 46);
    public override int MaxEnergy { get; } = Rand.Next(40, 61);
    public override int AccelerateSpeedIncrement { get; } = Rand.Next(4, 8);
    public override int DecelerateSpeedDecrement { get; } = Rand.Next(6, 11);
    public override int CoastingSpeedDecrement { get; } = Rand.Next(1, 3);
    public override int EnergyConsumptionRate { get; } = Rand.Next(2, 4);

    public override bool IsEnabled { get; protected set; } = true;

    public int CurrentGear { get; private set; } = 1;
    private const int MaxGear = 3;

    public override void Enable()
    {
        Console.WriteLine("No need to enable a bicycle\n");
    }

    public override void Disable()
    {
        Console.WriteLine("Cannot disable a bicycle\n");
    }

    public void ShiftUp()
    {
        if (CurrentGear < MaxGear)
        {
            Console.WriteLine($"{Model} shifting up gear...");
            QuickAction();
            CurrentGear++;
            Console.WriteLine($"{Model} shifted up to gear {CurrentGear}\n");
        }
        else
        {
            Console.WriteLine($"{Model} already in highest gear!");
        }
    }

    public void ShiftDown()
    {
        if (CurrentGear > 1)
        {
            Console.WriteLine($"{Model} shifting down gear...");
            QuickAction();
            CurrentGear--;
            Console.WriteLine($"{Model} shifted down to gear {CurrentGear}\n");
        }
        else
        {
            Console.WriteLine($"{Model} already in lowest gear!");
        }
    }

    public override bool Accelerate()
    {
        if (!CanAccelerate())
        {
            return false;
        }

        Console.WriteLine($"{Model} is accelerating...");
        MediumAction();

        IsMoving = true;
        EnergyLevel = (int)Math.Max(0, EnergyLevel - (EnergyConsumptionRate * _gearSpeedMultiplier()));
        CurrentSpeed = (int)Math.Min(MaxSpeed, CurrentSpeed + (AccelerateSpeedIncrement * _gearSpeedMultiplier()));
        DistanceTraveled += CurrentSpeed;

        Console.WriteLine($"{Model} accelerated:\n{GetStats()}");
        return CurrentSpeed != MaxSpeed;
    }

    protected override void GetVehicleSpecificStatus()
    {
        Console.WriteLine($" - Gear: {CurrentGear}");
        Console.WriteLine($" - Gear Speed Multiplier: {_gearSpeedMultiplier():F1}"); // F1 = 0.0 format
        Console.WriteLine($" - Gear Energy Multiplier: {_gearEnergyMultiplier():F1}");
    }

    private double _gearSpeedMultiplier() => CurrentGear switch
    {
        1 => 0.75,
        2 => 1,
        3 => 1.5,
        _ => 1,
    };

    private double _gearEnergyMultiplier() => CurrentGear switch
    {
        1 => 0.5,
        2 => 1,
        3 => 2,
        _ => 1,
    };
}