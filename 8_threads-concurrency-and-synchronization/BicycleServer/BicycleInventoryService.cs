using System.Text.Json;
using BicycleClient;
using Shared;
using VehicleProject.Vehicles;

namespace BicycleServer;

public class BicycleInventoryService
{
    private readonly BicycleList _bicycleList = new BicycleList();

    // rules for how to write to the json file
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private readonly string _dataFilePath;

    public BicycleInventoryService()
    {
        // make directory if it doesnt exist
        var dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
        Directory.CreateDirectory(dataDirectory);
        _dataFilePath = Path.Combine(dataDirectory, "bicycles.json");

        LoadBicyclesFromFile();
    }

    public CommandResponse ProcessCommand(CommandRequest request)
    {
        try
        {
            var response = request.CommandType switch
            {
                CommandType.Help => new CommandResponse(true, GetHelpText()),
                CommandType.List => ListBicycles(),
                CommandType.View => ViewBicycleInfo(request.Arguments),
                CommandType.Search => SearchBicycles(request.Arguments),
                CommandType.Add => AddBicycle(request.Arguments),
                CommandType.Remove => RemoveBicycle(request.Arguments),
                CommandType.Update => UpdateBicycle(request.Arguments),
                CommandType.Exit => new CommandResponse(true, "Exiting..."),
                CommandType.Unknown => new CommandResponse(false, "Unknown command"),
            };

            // save if command modifies the bike list
            if (IsFileModifyingCommand(request.CommandType))
            {
                SaveBicyclesToFile();
            }

            return response;
        }
        catch (Exception e)
        {
            return new CommandResponse(false, $"Error executing command: {e.Message}");
        }
    }

    private bool IsFileModifyingCommand(CommandType commandType)
    {
        return commandType is CommandType.Add or CommandType.Remove or CommandType.Update;
    }

    private void SaveBicyclesToFile()
    {
        try
        {
            var bicycles = _bicycleList.GetBicycles();
            var bicycleDataList = bicycles
                .Select((bicycle, index) => BicycleSaveData.FromBicycle(bicycle))
                .ToList();

            var json = JsonSerializer.Serialize(bicycleDataList, _jsonOptions);
            File.WriteAllText(_dataFilePath, json);

            Console.WriteLine($"[INFO] Saved {bicycleDataList.Count} bicycles to {_dataFilePath}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"[ERROR] Error saving bicycles to file: {e.Message}");
        }
    }

    private void LoadBicyclesFromFile()
    {
        try
        {
            if (!File.Exists(_dataFilePath))
            {
                Console.WriteLine(
                    $"[LOG] No existing data file found. Starting with empty inventory. Hopefully this is the expected result, otherwise uhh... go check ur data");
                return;
            }

            var json = File.ReadAllText(_dataFilePath);
            var bicycleDataList = JsonSerializer.Deserialize<List<BicycleSaveData>>(json, _jsonOptions);

            if (bicycleDataList == null)
            {
                Console.WriteLine(
                    $"[WARNING] Failed to get bicycle data even though a save file exists. *Might* be bad...");
                return;
            }

            foreach (var bicycleSaveData in bicycleDataList)
            {
                var bicycle = new Bicycle(bicycleSaveData.Model, bicycleSaveData.Year, bicycleSaveData.Color)
                {
                    MaxEnergy = bicycleSaveData.MaxEnergy,
                    MaxSpeed = bicycleSaveData.MaxSpeed,
                    EnergyConsumptionRate = bicycleSaveData.EnergyConsumptionRate,
                };
                _bicycleList.AddBicycle(bicycle);
            }

            Console.WriteLine($"[LOG] Loading {bicycleDataList.Count} bicycles from {_dataFilePath}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"[ERROR] Error loading bicycles from file: {e.Message}");
        }
    }

    public void Shutdown()
    {
        Console.WriteLine("[LOG] Shutting down... Saving data again (just in case)...");
        SaveBicyclesToFile();
        Console.WriteLine("[LOG] Shutdown complete");
    }

    private static string GetHelpText()
    {
        return """
               Available Commands:
                 View <index>                      - View info about the bicycle at given index
                 list                              - Show all bicycles
                 search <property> <op> <value>    - Search bicycles
                 add <model> <color> <year>        - adds specified bicycle to the list
                 remove <index>                    - Removes the bicycle at index, if is found at given index
                 update <index> <property> <value> - Updates bicycle property at specified index
                 help                              - Show this menu
                 exit                              - Exit program

               Bike Properties: 'Model', 'Color', 'Year', 'MaxSpeed', 'MaxEnergy', 'EnergyConsumption'
               Operators: '=', '<' '>'
               """;
    }

    private CommandResponse ListBicycles()
    {
        var bicycles = _bicycleList.GetBicycles();
        var bicycleDataStuff = bicycles
            .Select((bicycle, index) => BicycleData.FromBicycle(bicycle, index))
            .ToList();

        var message = bicycleDataStuff.Count == 0
            ? "No bicycles in inventory :pensive:"
            : $"Found {bicycleDataStuff.Count} bicycles!";

        return new CommandResponse(true, message, bicycleDataStuff);
    }

    private CommandResponse ViewBicycleInfo(string[] arguments)
    {
        if (arguments.Length != 1)
        {
            return new CommandResponse(false,
                "Incorrect number of arguments, command should follow this format: view <index>");
        }

        if (!int.TryParse(arguments[0], out int index))
        {
            return new CommandResponse(false, "Invalid index format. Enter a valid number lol");
        }

        var bicycle = _bicycleList.TryGetBicycle(index);
        if (bicycle == null)
        {
            return new CommandResponse(false, $"No bicycle found at index {index}");
        }

        return new CommandResponse(true, $"Bicycle at index {index}", BicycleData.FromBicycle(bicycle, index));
    }

    private CommandResponse AddBicycle(string[] arguments)
    {
        if (arguments.Length != 3)
        {
            return new CommandResponse(false,
                "Incorrect number of arguments, command should follow this format: add <model> <color> <year>");
        }

        var model = arguments[0];
        var color = arguments[1];
        if (!int.TryParse(arguments[2], out var year))
        {
            return new CommandResponse(false, "Invalid year format. Enter a valid number lol");
        }

        _bicycleList.AddBicycle(new Bicycle(model, year, color));
        var list = _bicycleList.GetBicycles();
        return new CommandResponse(true, $"Added bicycle to system at index {list.Count - 1}", BicycleData.FromBicycle(list.Last(), list.Count - 1));
    }

    private CommandResponse RemoveBicycle(string[] arguments)
    {
        if (arguments.Length != 1)
        {
            return new CommandResponse(false,
                "Incorrect number of arguments, command should follow this format: remove <index>");
        }

        if (!int.TryParse(arguments[0], out int index))
        {
            return new CommandResponse(false, "Invalid index format. Enter a valid number lol");
        }

        if (!_bicycleList.TryRemoveBicycle(index))
        {
            return new CommandResponse(false,
                $"Failed to remove bicycle at index {index}. Index is probably of range.");
        }

        return new CommandResponse(true, $"Removed bicycle at index {index}");
    }

    private CommandResponse UpdateBicycle(string[] arguments)
    {
        if (arguments.Length != 3)
        {
            return new CommandResponse(false,
                "Incorrect number of arguments, command should follow this format: update <index> <property> <value>");
        }

        var indexString = arguments[0];
        var searchPropertyString = arguments[1];
        var newValue = arguments[2];

        if (!int.TryParse(arguments[0], out var index))
        {
            return new CommandResponse(false, "Invalid index format. Enter a valid number lol");
        }

        var searchProperty = ParseSearchPropertyString(searchPropertyString);
        if (searchProperty == null)
        {
            return new CommandResponse(false, "Invalid property");
        }

        if (!_bicycleList.TryUpdateBicycle(index, (BicycleList.SearchProperty)searchProperty, newValue))
        {
            return new CommandResponse(false, "Failed to update bicycle for some reason");
        }

        return new CommandResponse(true, "Updated bicycle",
            BicycleData.FromBicycle(_bicycleList.TryGetBicycle(index)!, index)); // we already know the index is valid so dont care
    }

    private CommandResponse SearchBicycles(string[] arguments)
    {
        if (arguments.Length != 3)
        {
            return new CommandResponse(false,
                "Incorrect number of arguments, command should follow this format: search <property> <op> <value>");
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
            return new CommandResponse(false, "Invalid operator");
        }

        if (searchProperty == null)
        {
            return new CommandResponse(false, "Invalid property");
        }

        // i already checked if its null, stupid ahh compiler, why dont u work like typescript???
        var result = _bicycleList.SearchBicycles((BicycleList.SearchProperty)searchProperty, (BicycleList.SearchOperator)searchOperator, query);

        var resultList = result
            .Select(item => BicycleData.FromBicycle(item.bicycle, item.index))
            .ToList();

        var message = resultList.Count == 0
            ? "No bicycles found matching the search :pensive:"
            : $"Found {result.Count} matching bicycles!";

        return new CommandResponse(true, message, resultList);
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

// just BicycleData without index
public record BicycleSaveData(
    string Model,
    string Color,
    int Year,
    int MaxSpeed,
    int MaxEnergy,
    int EnergyConsumptionRate)
{
    public static BicycleSaveData FromBicycle(Bicycle bicycle)
        => new(bicycle.Model, bicycle.Color, bicycle.Year, bicycle.MaxSpeed, bicycle.MaxEnergy, bicycle.EnergyConsumptionRate);
}