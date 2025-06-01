using VehicleProject.Vehicles;

namespace BicycleInventoryProgram;

public class BicycleListManager(BicycleList bicycleList, IInputHandler inputHandler)
{
    private readonly BicycleList _bicycleList = bicycleList;
    private readonly IInputHandler _inputHandler = inputHandler;

    public async Task Start()
    {
        await foreach (var command in _inputHandler.CommandChanReader.ReadAllAsync())
        {
            ProcessCommand(command);
            if (command.CommandType == CommandType.Exit)
            {
                break;
            }
        }
    }

    private void ProcessCommand(InputCommand command)
    {
        Action action = command.CommandType switch
        {
            CommandType.Help => DisplayHelp,
            CommandType.List => ListBicycles,
            CommandType.View => () => ViewBicycleInfo(command.Arguments),
            CommandType.Search => () => SearchBicycles(command.Arguments),
            CommandType.Add => () => AddBicycle(command.Arguments),
            CommandType.Remove => () => RemoveBicycle(command.Arguments),
            CommandType.Update => () => UpdateBicycle(command.Arguments),
            CommandType.Exit => () => Console.WriteLine("Exiting"),
            CommandType.Unknown => () => Console.WriteLine("Unknown command"),
        };
        action();
        Console.Write("\n> ");
    }

    private static void DisplayHelp()
    {
        Console.WriteLine("\nAvailable Commands:");
        Console.WriteLine("  View <index>                      - View info about the bicycle at given index");
        Console.WriteLine("  list                              - Show all bicycles");
        Console.WriteLine("  search <property> <op> <value>    - Search bicycles");
        Console.WriteLine("  add <model> <color> <year>        - adds specified bicycle to the list");
        Console.WriteLine("  remove <index>                    - Removes the bicycle at index, if is found at given index");
        Console.WriteLine("  update <index> <property> <value> - Updates bicycle property at specified index");
        Console.WriteLine("  help                              - Show this menu");
        Console.WriteLine("  exit                              - Exit program");
        Console.WriteLine("\nBike Properties: 'Model', 'Color', 'Year', 'MaxSpeed', 'MaxEnergy', 'EnergyConsumption'");
        Console.WriteLine("Operators: '=', '<' '>'");
    }

    private void ListBicycles()
    {
        var i = 0;
        foreach (var bicycle in _bicycleList.GetBicycles())
        {
            Console.WriteLine($" Index {i}: Model: {bicycle.Model}");
            i++;
        }
    }

    private void ViewBicycleInfo(string[] arguments)
    {
        if (arguments.Length != 1)
        {
            Console.WriteLine("Incorrect number of arguments, command should follow this format: remove <index>");
            return;
        }
        
        if (!int.TryParse(arguments[0], out int index))
        {
            Console.WriteLine("Invalid index format. Enter a valid number lol");
            return;
        }

        var bicycle = _bicycleList.TryGetBicycle(index);
        if (bicycle == null)
        {
            Console.WriteLine($"No bicycle found at index {index}.");
            return;
        }
        DisplayBikeInfo(bicycle);
    }

    private void AddBicycle(string[] arguments)
    {
        if (arguments.Length != 3)
        {
            Console.WriteLine("Incorrect number of arguments, command should follow this format: add <model> <color> <year>");
            return;
        }

        var model = arguments[0];
        var color = arguments[1];
        if (!int.TryParse(arguments[2], out int year))
        {
            Console.WriteLine("Invalid year format. Enter a valid number lol");
            return;
        }
        
        _bicycleList.AddBicycle(new Bicycle(model, year, color));
        Console.WriteLine("Added bicycle to system");
    }

    private void RemoveBicycle(string[] arguments)
    {
        if (arguments.Length != 1)
        {
            Console.WriteLine("Incorrect number of arguments, command should follow this format: remove <index>");
            return;
        }
        
        if (!int.TryParse(arguments[0], out int index))
        {
            Console.WriteLine("Invalid index format. Enter a valid number lol");
            return;
        }

        if (!_bicycleList.TryRemoveBicycle(index))
        {
            Console.WriteLine($"Failed to remove bicycle at index {index}. Index is probably of range.");
            return;
        }
        
        Console.WriteLine($"Removed bicycle at index {index}");
    }

    private void UpdateBicycle(string[] arguments)
    {
        if (arguments.Length != 3)
        {
            Console.WriteLine("Incorrect number of arguments, command should follow this format: update <index> <property> <value>");
            return;
        }

        var indexString = arguments[0];
        var searchPropertyString = arguments[1];
        var newValue = arguments[2];

        if (!int.TryParse(arguments[0], out int index))
        {
            Console.WriteLine("Invalid index format. Enter a valid number lol");
            return;
        }

        var searchProperty = ParseSearchPropertyString(searchPropertyString);
        if (searchProperty == null)
        {
            Console.WriteLine("Invalid property");
            return;
        }

        if (!_bicycleList.TryUpdateBicycle(index, (BicycleList.SearchProperty)searchProperty, newValue))
        {
            Console.WriteLine("Failed to update bicycle for some reason");
            return;
        }
        Console.WriteLine("Updated bicycle");
    }

    private void SearchBicycles(string[] arguments)
    {
        if (arguments.Length != 3)
        {
            Console.WriteLine("Incorrect number of arguments, command should follow this format: search <property> <op> <value>");
            return;
        }
        var searchPropertyString = arguments[0];
        var searchOperatorString = arguments[1];
        var query = arguments[2];

        var searchProperty = ParseSearchPropertyString(searchPropertyString);
        BicycleList.SearchOperator? searchOperator = searchOperatorString switch
        {
            "=" => BicycleList.SearchOperator.EqualTo,
            ">" => BicycleList.SearchOperator.GreaterThan,
            "<" => BicycleList.SearchOperator.LessThan,
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
        var result = _bicycleList.SearchBicycles((BicycleList.SearchProperty)searchProperty, (BicycleList.SearchOperator)searchOperator, query);

        var i = 1;
        foreach (var (index, bicycle) in result)
        {
            Console.WriteLine($"\nResult {i}. (Index: {index})");
            DisplayBikeInfo(bicycle);
            i++;
        }
    }

    private void DisplayBikeInfo(Bicycle bicycle)
    {
        Console.WriteLine($" - Model: {bicycle.Model} ");
        Console.WriteLine($" - Color: {bicycle.Color} ");
        Console.WriteLine($" - Year: {bicycle.Year} ");
        Console.WriteLine($" - Max Speed: {bicycle.MaxSpeed} ");
        Console.WriteLine($" - Max Energy: {bicycle.MaxEnergy} ");
        Console.WriteLine($" - Energy Consumption: {bicycle.EnergyConsumptionRate} ");
    }

    private BicycleList.SearchProperty? ParseSearchPropertyString(string searchProperty) => searchProperty.ToLower() switch
    {
        "model" => BicycleList.SearchProperty.Model,
        "color" => BicycleList.SearchProperty.Color,
        "year" => BicycleList.SearchProperty.Year,
        "maxspeed" => BicycleList.SearchProperty.MaxSpeed,
        "maxenergy" => BicycleList.SearchProperty.MaxEnergy,
        "energyconsumption" => BicycleList.SearchProperty.EnergyConsumption,
        _ => null
    };
}