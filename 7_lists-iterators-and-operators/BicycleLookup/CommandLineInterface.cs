using VehicleProject.Vehicles;

namespace BicycleLookup;

public class CommandLineInterface(BicycleManager bicycleManager)
{
    private BicycleManager _bicycleManager = bicycleManager;
    private bool _running = true;

    public void Start()
    {
        Console.WriteLine("Welcome to BicycleManagementSystem2000 (woahh)");
        Console.Write("Use `help` to see a list of commands");

        while (_running)
        {
            Console.Write("\n> ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input)) continue;
            ProcessInput(input);
        }
    }

    private void ProcessInput(string input)
    {
        var parts = input.Split(' ');
        var command = parts[0].ToLower();

        Action action = command switch
        {
            "help" => DisplayHelp,
            "list" => ListBicycles,
            "search" => () => SearchBicycles(parts),
            "exit" => () => _running = false,
            _ => () => Console.WriteLine("Unknown command"),
        };
        action();
    }

    private static void DisplayHelp()
    {
        Console.WriteLine("\nAvailable Commands:");
        Console.WriteLine("  list                           - Show all bicycles");
        Console.WriteLine("  search <property> <op> <value> - Search bicycles");
        Console.WriteLine("  help                           - Show this menu");
        Console.WriteLine("  exit                           - Exit program");
        Console.WriteLine("\nSearch Properties: 'Model', 'Color', 'Year', 'MaxSpeed', 'MaxEnergy', 'EnergyConsumption'");
        Console.WriteLine("Operators: '=', '<' '>'");
    }

    private void ListBicycles()
    {
        var i = 1;
        foreach (var bicycle in _bicycleManager.GetBicycles())
        {
            Console.WriteLine($" {i}. Model: {bicycle.Model}");
            i++;
        }
    }

    private void SearchBicycles(string[] parts)
    {
        var arguments = parts.Skip(1).ToArray();
        if (arguments.Length != 3)
        {
            Console.WriteLine("Incorrect number of arguments, command should follow this format: search <property> <op> <value>");
            return;
        }
        var searchPropertyString = arguments[0];
        var searchOperatorString = arguments[1];
        var query = arguments[2];

        var searchProperty = ParseSearchPropertyString(searchPropertyString);
        BicycleManager.SearchOperator? searchOperator = searchOperatorString switch
        {
            "=" => BicycleManager.SearchOperator.EqualTo,
            ">" => BicycleManager.SearchOperator.GreaterThan,
            "<" => BicycleManager.SearchOperator.LessThan,
            _ => null,
        };

        if (searchOperator == null)
        {
            Console.WriteLine("Invalid operator");
            return;
        }

        if (searchProperty == null)
        {
            Console.WriteLine("Invalid property");
            return;
        }

        // i already checked if its null, stupid ahh compiler, why dont u work like typescript???
        var result = _bicycleManager.SearchBicycles((BicycleManager.SearchProperty)searchProperty, (BicycleManager.SearchOperator)searchOperator, query);

        var i = 1;
        foreach (var bicycle in result)
        {
            Console.WriteLine($"\nResult {i}.");
            Console.WriteLine($" - Model: {bicycle.Model} ");
            Console.WriteLine($" - Color: {bicycle.Color} ");
            Console.WriteLine($" - Year: {bicycle.Year} ");
            Console.WriteLine($" - Max Speed: {bicycle.MaxSpeed} ");
            Console.WriteLine($" - Max Energy: {bicycle.MaxEnergy} ");
            Console.WriteLine($" - Energy Consumption: {bicycle.EnergyConsumptionRate} ");
            i++;
        }
    }

    private BicycleManager.SearchProperty? ParseSearchPropertyString(string searchProperty) => searchProperty.ToLower() switch
    {
        "model" => BicycleManager.SearchProperty.Model,
        "color" => BicycleManager.SearchProperty.Color,
        "year" => BicycleManager.SearchProperty.Year,
        "maxspeed" => BicycleManager.SearchProperty.MaxSpeed,
        "maxenergy" => BicycleManager.SearchProperty.MaxEnergy,
        "energyconsumption" => BicycleManager.SearchProperty.EnergyConsumption,
        _ => null
    };
}