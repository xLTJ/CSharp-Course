namespace VehicleProject;

public abstract class Vehicle(string model, int year, string color)
{
    // for randomly generated values
    protected static readonly Random Rand = new Random();

    // immutable attributes based on supplied parameters. these can never be changed
    public string Model { get; } = !string.IsNullOrWhiteSpace(model)
        ? model
        : throw new ArgumentException("Model cannot empty");

    public string Color { get; } = !string.IsNullOrWhiteSpace(color)
        ? color
        : throw new ArgumentException("Color cannot be empty");

    public int Year { get; } = year;

    // immutable attributes defined by the subclass
    protected abstract int MaxSpeed { get; }
    protected abstract int MaxEnergy { get; }
    protected abstract int AccelerateSpeedIncrement { get; }
    protected abstract int DecelerateSpeedDecrement { get; }
    protected abstract int CoastingSpeedDecrement { get; }
    protected abstract int EnergyConsumptionRate { get; }

    // changeable attributes, protected setters so that they cannot be changed from the outside, but can be changed by subclasses
    public int CurrentSpeed { get; protected set; } = 0;
    public int DistanceTraveled { get; protected set; } = 0;
    public bool IsMoving { get; protected set; } = false;
    public bool IsOperational { get; protected set; } = true;
    public virtual bool IsEnabled { get; protected set; } = false;
    public int Turn { get; protected set; } = 0;
    private int? _energyLevel;

    // uses lazy initialization (only assigns the value once its actually used), so that MaxEnergy is available,
    public int EnergyLevel
    {
        get => _energyLevel ??= MaxEnergy;
        protected set => _energyLevel = value;
    }

    public virtual void Enable()
    {
        if (IsEnabled)
        {
            Console.WriteLine($"{Model} is already enabled\n");
            return;
        }

        if (!IsOperational)
        {
            Console.WriteLine($"{Model} cannot be enabled: Vehicle is not operational\n");
            return;
        }

        Console.WriteLine($"{Model} is being enabled...");
        QuickAction();
        IsEnabled = true;
        Console.WriteLine($"{Model} has been enabled!\n");
    }

    public virtual void Disable()
    {
        if (!IsEnabled)
        {
            Console.WriteLine($"{Model} is already disabled\n");
            return;
        }

        if (IsMoving)
        {
            Console.WriteLine($"{Model} cannot be disabled: Vehicle is currently moving\n");
            return;
        }

        Console.WriteLine($"{Model} is being disabled...");
        QuickAction();
        IsEnabled = false;
        Console.WriteLine($"{Model} has been disabled!\n");
    }

    /// <summary>
    /// Accelerates the car
    /// </summary>
    /// <returns>Whether the car is able to accelerate further</returns>
    public virtual bool Accelerate()
    {
        if (!CanAccelerate())
        {
            return false;
        }

        Console.WriteLine($"{Model} is accelerating...");
        MediumAction();

        IsMoving = true;
        CurrentSpeed = Math.Min(MaxSpeed, CurrentSpeed + AccelerateSpeedIncrement);
        EnergyLevel = Math.Max(0, EnergyLevel - EnergyConsumptionRate);
        DistanceTraveled += CurrentSpeed;

        Console.WriteLine($"{Model} accelerated:\n{GetStats()}");
        return CurrentSpeed != MaxSpeed;
    }

    protected bool CanAccelerate()
    {
        if (!IsEnabled)
        {
            Console.WriteLine($"{Model} cannot accelerate: Vehicle not enabled\n");
            return false;
        }

        if (CurrentSpeed == MaxSpeed)
        {
            Console.WriteLine($"{Model} cannot accelerate: Already at top speed\n");
            return false;
        }

        if (EnergyLevel == 0)
        {
            Console.WriteLine($"{Model} cannot accelerate: Vehicle has no energy\n");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Decelerates the car
    /// </summary>
    /// <returns>Whether the car is able to decelerate further</returns>
    public virtual bool Decelerate()
    {
        if (!IsMoving)
        {
            Console.WriteLine($"{Model} cannot decelerate: Vehicle is not moving");
            return false;
        }

        Console.WriteLine($"{Model} is decelerating...");
        MediumAction();

        CurrentSpeed = Math.Max(0, CurrentSpeed - DecelerateSpeedDecrement);
        DistanceTraveled += CurrentSpeed;

        if (CurrentSpeed == 0)
        {
            IsMoving = false;
        }

        Console.WriteLine($"{Model} decelerated:\n{GetStats()}");
        return CurrentSpeed != 0;
    }

    public virtual void Coast()
    {
        if (!IsMoving)
        {
            Console.WriteLine($"{Model} cannot coast: Vehicle is not moving (smh)");
            return;
        }

        Console.WriteLine($"{Model} is coasting...");
        MediumAction();

        CurrentSpeed = Math.Max(0, CurrentSpeed - CoastingSpeedDecrement);
        DistanceTraveled += CurrentSpeed;

        if (CurrentSpeed == 0)
        {
            IsMoving = false;
        }

        Console.WriteLine($"{Model} coasted:\n{GetStats()}");
    }

    public void GetStatus()
    {
        Console.WriteLine($"Vehicle Status:");
        GetBaseStatus();
        GetVehicleSpecificStatus();
    }

    private void GetBaseStatus()
    {
        Console.WriteLine($" - Model: {Model}");
        Console.WriteLine($" - Year: {Year}");
        Console.WriteLine($" - Color: {Color}");
        Console.WriteLine($" - Max Speed: {MaxSpeed}");
        Console.WriteLine($" - Energy Level: {EnergyLevel}");
        Console.WriteLine($" - Distance Traveled: {DistanceTraveled}");
        Console.WriteLine($" - Is Moving: {IsMoving}");
        Console.WriteLine($" - Current Speed: {CurrentSpeed}");
        Console.WriteLine($" - Is Operational: {IsOperational}");
        Console.WriteLine($" - Is Enabled: {IsEnabled}");
        Console.WriteLine($" - Current Turn: {Turn}");
    }

    protected virtual void GetVehicleSpecificStatus(){}

    public void AccelerateToMax()
    {
        var canAccelerateMore = true;
        while (canAccelerateMore)
        {
            canAccelerateMore = Accelerate();
        }
    }

    public void Stop()
    {
        var canDecelerateMore = true;
        while (canDecelerateMore)
        {
            canDecelerateMore = Decelerate();
        }
    }

    protected virtual string GetStats()
    {
        return $"- Current turn: {Turn}\n- Current speed:{CurrentSpeed}\n- Energy left: {EnergyLevel}\n- Distance Traveled: {DistanceTraveled}\n";
    }

    protected void QuickAction()
    {
        Turn++;
        Thread.Sleep(500);
    }

    protected void MediumAction()
    {
        Turn++;
        Thread.Sleep(1000);
    }

    protected void LongAction()
    {
        Turn++;
        Thread.Sleep(2000);
    }
}