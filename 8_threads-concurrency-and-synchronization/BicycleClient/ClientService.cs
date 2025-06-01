using System.Text.Json;
using Shared;

namespace BicycleClient;

public class ClientService
{
    public void DisplayResponse(CommandResponse response)
    {
        if (response.Success)
        {
            Console.Write(response.Message);

            if (response.Data != null)
            {
                DisplayData(response.Data);
            }
        }
        else
        {
            Console.WriteLine($"Error: {response.Message}");
        }
    }

    private void DisplayData(object data)
    {
        // funny json stuff which allows us to check if its an array (and so a list of bicycles) or a single bicycle
        var jsonString = JsonSerializer.Serialize(data);
        var element = JsonSerializer.Deserialize<JsonElement>(jsonString);

        // check if list
        if (element.ValueKind == JsonValueKind.Array)
        {
            Console.WriteLine("\nBicycles:");
            foreach (var item in element.EnumerateArray())
            {
                if (item.TryGetProperty("Index", out var indexProp) &&
                    item.TryGetProperty("Model", out var modelProp))
                {
                    Console.WriteLine($"  Index {indexProp}: {modelProp}");
                }
            }
            return;
        }

        // otherwise print as single bicycle
        Console.WriteLine("\nBicycle Details:");
        if (element.TryGetProperty("Model", out var model))
            Console.WriteLine($"  Model: {model}");
        if (element.TryGetProperty("Color", out var color))
            Console.WriteLine($"  Color: {color}");
        if (element.TryGetProperty("Year", out var year))
            Console.WriteLine($"  Year: {year}");
        if (element.TryGetProperty("MaxSpeed", out var maxSpeed))
            Console.WriteLine($"  Max Speed: {maxSpeed}");
        if (element.TryGetProperty("MaxEnergy", out var maxEnergy))
            Console.WriteLine($"  Max Energy: {maxEnergy}");
        if (element.TryGetProperty("EnergyConsumptionRate", out var energy))
            Console.WriteLine($"  Energy Consumption: {energy}");

    }
}