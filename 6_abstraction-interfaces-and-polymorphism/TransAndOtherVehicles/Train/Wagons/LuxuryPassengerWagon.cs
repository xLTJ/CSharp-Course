namespace TransAndOtherVehicles.Train.Wagons;

public class LuxuryPassengerWagon(int weight, int seatCount, int passengerCapacity, string luxuryLevel, List<string> amenities) : PassengerWagon(weight, seatCount, passengerCapacity)
{
    public string LuxuryLevel { get; } = luxuryLevel;
    public List<string> Amenities { get; } = amenities;
}