using VehicleProject;
using VehicleProject.Vehicles;

Main();
return;

void Main()
{
    var quitNext = false;

    Console.WriteLine("Welcome to VehicleSimulator2000 (woahh)");
    Console.WriteLine("Select your vehicle:\n1. Boring Car\n2. Bicycle\n3. Supercar\n4. truck");
    var vehicleId = int.Parse(Console.ReadLine() ?? "0");

    if (!Enum.IsDefined(typeof(VehicleType), vehicleId))
    {
        throw new ArgumentException("Invalid vehicle >:c");
    }
    Console.WriteLine("Enter Vehicle Model:");
    var Model = Console.ReadLine();
    Console.WriteLine("Enter Vehicle Manufacturing Year:");
    var Year = int.Parse(Console.ReadLine() ?? "2000");
    Console.WriteLine("Enter Vehicle Color:");
    var Color = Console.ReadLine();

    Vehicle vehicle = (VehicleType)vehicleId switch
    {
        VehicleType.BoringCar => new BoringCar(Model, Year, Color),
        VehicleType.Bicycle => new Bicycle(Model, Year, Color),
        VehicleType.Supercar => new Supercar(Model, Year, Color),
        VehicleType.Truck => new Truck(Model, Year, Color),
        _ => throw new ArgumentException("Invalid Vehicle")
    };

    var baseCommands = new Dictionary<string, (string description, Action action)>
    {
        { "q", ("Quit", () => quitNext = true)},
        { "quit", ("Quit", () => quitNext = true)},
        { "enable", ("Enable the vehicle", () => vehicle.Enable()) },
        { "disable", ("Disable the vehicle", () => vehicle.Disable()) },
        { "a", ("Increase vehicle speed", () => vehicle.Accelerate()) },
        { "accelerate", ("Increase vehicle speed", () => vehicle.Accelerate()) },
        { "d", ("Decrease vehicle speed", () => vehicle.Decelerate()) },
        { "decelerate", ("Decrease vehicle speed", () => vehicle.Decelerate()) },
        { "c", ("Maintain current speed", () => vehicle.Coast()) },
        { "coast", ("Maintain current speed", () => vehicle.Coast()) },
        { "status", ("View information about the vehicle", () => vehicle.GetStatus()) },
        { "max", ("Accelerates the vehicle to max speed", () => vehicle.AccelerateToMax()) },
        { "stop", ("Decelerates the vehicle until it stops moving", () => vehicle.Stop()) },
    };

    var vehicleSpecificCommands = GetVehicleSpecificCommands(vehicle);

    // combine base commands with vehicle commands into one big dictionary
    var allCommands = baseCommands
        .Concat(vehicleSpecificCommands)
        .ToDictionary();

    var commandDescriptions = allCommands
        .GroupBy(pair => pair.Value.description) // group based on description to make joined help message for short and full
        .ToDictionary( // turns into a dictionary where the combined names are the key, and their shared description is the value
            group => string.Join(" | ", group.Select(pair => pair.Key)),
            group => group.Key
        );

    // help command not included in the show help list, so its not needed in the commandDescription dictionary
    // and doesnt need a description
    allCommands.Add("help", ("", () => ShowHelp(commandDescriptions)));
    allCommands.Add("h", ("", () => ShowHelp(commandDescriptions)));
    
    Console.WriteLine("Starting sim, use `help` to see a list of commands");

    while (!quitNext)
    {
        Console.Write("\n>");
        var input = Console.ReadLine()?.ToLower().Trim();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("Theres no command smh");
            continue;
        }

        if (allCommands.TryGetValue(input, out var command))
        {
            command.action();
        }
        else
        {
            Console.WriteLine("Invalid command. Use 'help' for a list of available commands");
        }
    }
}

Dictionary<string, (string description, Action action)> GetVehicleSpecificCommands(Vehicle vehicle)
{
    return vehicle switch
    {
        Bicycle bicycle => new Dictionary<string, (string description, Action action)>
        {
            { "up", ("Shift gear up", () => bicycle.ShiftUp()) },
            { "down", ("Shift gear down", () => bicycle.ShiftDown()) }
        },

        BoringCar boringCar => new Dictionary<string, (string description, Action action)>
        {
            { "honk", ("Very underwhelming honk", () => boringCar.Honk()) }
        },

        Supercar supercar => new Dictionary<string, (string description, Action action)>
        {
            { "super", ("Superboost, 70% chance of accelerating 10 times faster than a normal accelerating, 30% of... exploding", () => supercar.SuperBoost()) }
        },

        _ => new Dictionary<string, (string, Action)>()
    };
}

void ShowHelp(Dictionary<string, string> commandDescriptions)
{
    Console.WriteLine("Available commands:");
    foreach (var commandDescription in commandDescriptions)
    {
        Console.WriteLine($" - {commandDescription.Key}: {commandDescription.Value}");
    }
}