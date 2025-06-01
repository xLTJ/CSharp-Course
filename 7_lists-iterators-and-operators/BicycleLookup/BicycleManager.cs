using VehicleProject.Vehicles;

namespace BicycleLookup;

public class BicycleManager
{
    private readonly List<Bicycle> _bicycleList = [];

    public List<Bicycle> GetBicycles()
    {
        return [.._bicycleList];
    }

    public void AddBicycle(Bicycle bicycle)
    {
        _bicycleList.Add(bicycle);
    }

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