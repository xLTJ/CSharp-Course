using VehicleProject.Vehicles;

namespace BicycleLookup;

public class BicycleManager
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

    public List<Bicycle> GetBicycles()
    {
        return [.._bicycleList];
    }

    public void AddBicycle(Bicycle bicycle)
    {
        _bicycleList.Add(bicycle);
    }

    // -------------------------------------- sort functionality ---------------------------------------------------
    // uses IComparer once to show how it works
    public List<Bicycle> SortByMaxSpeed(bool descending = false)
    {
        List<Bicycle> sortedBicycleList = [.._bicycleList];
        sortedBicycleList.Sort(new BicycleMaxSpeedComparer(descending));
        return sortedBicycleList;
    }

    // LINQ for the rest cus wtf was that
    public List<Bicycle> SortByMaxEnergy(bool descending = false)
    {
        if (descending) return _bicycleList.OrderByDescending(bike => bike.MaxEnergy).ToList();
        return _bicycleList.OrderBy(bike => bike.MaxEnergy).ToList();
    }

    public List<Bicycle> SortByEnergyConsumption(bool descending = false)
    {
        if (descending) return _bicycleList.OrderByDescending(bike => bike.EnergyConsumptionRate).ToList();
        return _bicycleList.OrderBy(bike => bike.EnergyConsumptionRate).ToList();
    }

    public List<Bicycle> SortByYear(bool descending = false)
    {
        if (descending) return _bicycleList.OrderByDescending(bike => bike.Year).ToList();
        return _bicycleList.OrderBy(bike => bike.Year).ToList();
    }

    // -------------------------------------- search functionality ---------------------------------------------------
    public List<Bicycle> SearchBicycles(SearchProperty searchProperty, SearchOperator searchOperator, string query)
    {
        return _bicycleList
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
}

public class BicycleMaxSpeedComparer(bool descending) : IComparer<Bicycle>
{
    public int Compare(Bicycle? x, Bicycle? y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        var result = x.MaxSpeed.CompareTo(y.MaxSpeed);
        return descending ? -result : result;
    }
}