namespace TransAndOtherVehicles.Train.Wagons;

public class SecurityWagon(int weight, string securityLevel) : Wagon(weight)
{
    public string SecurityLevel { get; } = securityLevel;
}