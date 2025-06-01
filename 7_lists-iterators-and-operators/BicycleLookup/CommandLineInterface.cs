using VehicleProject.Vehicles;

namespace BicycleLookup;

public class CommandLineInterface(BicycleManager bicycleManager)
{
    private enum SearchProperty
    {
        Model,
        Color,
        Year,
        MaxSpeed,
        MaxEnergy,
        EnergyConsumption,
    }

    private enum SearchOperator
    {
        EqualTo,
        GreaterThan,
        LessThan,
    }

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
        SearchOperator? searchOperator = searchOperatorString switch
        {
            "=" => SearchOperator.EqualTo,
            ">" => SearchOperator.GreaterThan,
            "<" => SearchOperator.LessThan,
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

        // i already checked if its null, stupid ahh compiler, why dont u work like typescript smh
        var result = SearchBicycles((SearchProperty)searchProperty, (SearchOperator)searchOperator, query);

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

    private List<Bicycle> SearchBicycles(SearchProperty searchProperty, SearchOperator searchOperator, string query)
    {
        return _bicycleManager.GetBicycles()
            .Where(bicycle => BicycleMatches(bicycle, searchProperty, searchOperator, query))
            .ToList();
    }

    private bool BicycleMatches(Bicycle bicycle, SearchProperty searchProperty, SearchOperator searchOperator, string query)
    {
        var value = GetValueFromBicycle(bicycle, searchProperty);
        var shouldAdd = false;

        switch (value)
        {
            case string stringValue:
                shouldAdd = searchOperator switch
                {
                    SearchOperator.EqualTo => string.Equals(query, stringValue, StringComparison.CurrentCultureIgnoreCase),
                    SearchOperator.GreaterThan => string.CompareOrdinal(stringValue, query) > 0,
                    SearchOperator.LessThan => string.CompareOrdinal(stringValue, query) < 0,
                    _ => false
                };
                break;

            case int intValue:
            {
                if (!int.TryParse(query, out int queryValue)) return false;
                shouldAdd = searchOperator switch
                {
                    SearchOperator.EqualTo => queryValue == intValue,
                    SearchOperator.GreaterThan => intValue > queryValue,
                    SearchOperator.LessThan => intValue < queryValue,
                    _ => false,
                };
                break;
            }
        }

        return shouldAdd;
    }

    private static object? GetValueFromBicycle(Bicycle bicycle, SearchProperty property) => property switch
    {
        SearchProperty.Model => bicycle.Model,
        SearchProperty.Color => bicycle.Color,
        SearchProperty.Year => bicycle.Year,
        SearchProperty.MaxSpeed => bicycle.MaxSpeed,
        SearchProperty.MaxEnergy => bicycle.MaxEnergy,
        SearchProperty.EnergyConsumption => bicycle.EnergyConsumptionRate,
        _ => null,

    };

    private SearchProperty? ParseSearchPropertyString(string searchProperty) => searchProperty.ToLower() switch
    {
        "model" => SearchProperty.Model,
        "color" => SearchProperty.Color,
        "year" => SearchProperty.Year,
        "maxspeed" => SearchProperty.MaxSpeed,
        "maxenergy" => SearchProperty.MaxEnergy,
        "energyconsumption" => SearchProperty.EnergyConsumption,
        _ => null
    };
}