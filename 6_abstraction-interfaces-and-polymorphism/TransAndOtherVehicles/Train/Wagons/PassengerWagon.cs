namespace TransAndOtherVehicles.Train.Wagons;

public class PassengerWagon(int weight, int seatCount, int passengerCapacity) : Wagon(weight)
{
    public int SeatCount { get; } = seatCount;
    public int PassengerCapacity { get; } = passengerCapacity;
}