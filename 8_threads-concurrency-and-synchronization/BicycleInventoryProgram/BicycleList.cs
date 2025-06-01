using VehicleProject.Vehicles;

namespace BicycleInventoryProgram;

public class BicycleList
{

    public enum SearchProperty
    {
        Model,
        Color,
        Year,
        MaxSpeed,
        MaxEnergy,
        EnergyConsumption,
    }

    public enum SearchOperator
    {
        EqualTo,
        GreaterThan,
        LessThan,
    }

    private readonly List<Bicycle> _bicycleList = [];
    private readonly Lock _lockObject = new();

    // -------------------------------------- simple stuff functionality ----------------------------------------------
    public List<Bicycle> GetBicycles()
    {
        lock (_lockObject)
        {
            return [.._bicycleList];
        }
    }

    public Bicycle? TryGetBicycle(int index)
    {
        lock (_lockObject)
        {
            if (index < 0 || index >= _bicycleList.Count)
            {
                return null;
            }

            return _bicycleList[index];
        }
    }

    public void AddBicycle(Bicycle bicycle)
    {
        lock (_lockObject)
        {
            _bicycleList.Add(bicycle);
        }
    }

    public bool TryRemoveBicycle(int index)
    {
        lock (_lockObject)
        {
            if (index < 0 || index >= _bicycleList.Count)
            {
                return false;
            }

            _bicycleList.RemoveAt(index);
            return true;
        }
    }

    // -------------------------------------- update functionality ---------------------------------------------------
    public bool TryUpdateBicycle(int index, SearchProperty searchProperty, string newValue)
    {
        lock (_lockObject)
        {
            if (index < 0 || index >= _bicycleList.Count)
            {
                return false;
            }

            var bicycle = _bicycleList[index];
            return searchProperty switch
            {
                SearchProperty.Model => TrySetStringProperty(() => bicycle.Model = newValue),
                SearchProperty.Color => TrySetStringProperty(() => bicycle.Color = newValue),
                SearchProperty.Year => TrySetIntProperty(newValue, intValue => bicycle.Year = intValue),
                SearchProperty.MaxSpeed => TrySetIntProperty(newValue, intValue => bicycle.MaxSpeed = intValue),
                SearchProperty.MaxEnergy => TrySetIntProperty(newValue, intValue => bicycle.MaxEnergy = intValue),
                SearchProperty.EnergyConsumption => TrySetIntProperty(newValue, intValue => bicycle.EnergyConsumptionRate = intValue),
                _ => false,
            };
        }
    }

    // this doesnt check anything, it just need to return true so i can simplify the switch
    private static bool TrySetStringProperty(Action setAction)
    {
        setAction();
        return true;
    }

    // here we actually need to check if the input can be parsed as int
    private static bool TrySetIntProperty(string newValue, Action<int> setAction)
    {
        if (!int.TryParse(newValue, out int intValue))
        {
            return false;
        }

        setAction(intValue);
        return true;
    }

    // -------------------------------------- search functionality ---------------------------------------------------
    public List<(int index, Bicycle bicycle)> SearchBicycles(SearchProperty searchProperty, SearchOperator searchOperator, string query)
    {
        lock (_lockObject)
        {
            return _bicycleList
                .Select((bicycle, index) => new { bicycle, index }) // need to save the original index
                .Where(item => BicycleMatches(item.bicycle, searchProperty, searchOperator, query)) // get matches
                .Select(item => (item.index, item.bicycle)) // turn into tuple
                .ToList();
        }
    }

    private bool BicycleMatches(Bicycle bicycle, SearchProperty searchProperty, SearchOperator searchOperator, string query)
    {
        var value = GetValueFromBicycle(bicycle, searchProperty);
        var matches = false;

        switch (value)
        {
            case string stringValue:
                matches = searchOperator switch
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
                matches = searchOperator switch
                {
                    SearchOperator.EqualTo => queryValue == intValue,
                    SearchOperator.GreaterThan => intValue > queryValue,
                    SearchOperator.LessThan => intValue < queryValue,
                    _ => false,
                };
                break;
            }
        }

        return matches;
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
}