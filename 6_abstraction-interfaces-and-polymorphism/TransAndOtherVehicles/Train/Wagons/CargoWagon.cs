namespace TransAndOtherVehicles.Train.Wagons;

public class CargoWagon(int weight, string cargoType, int cargoCapacity) : Wagon(weight)
{
    public string CargoType { get; } = cargoType;
    public int CargoCapacity { get; } = cargoCapacity;
}