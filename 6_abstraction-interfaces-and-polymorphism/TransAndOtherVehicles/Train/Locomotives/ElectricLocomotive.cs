using TransAndOtherVehicles.Interfaces;

namespace TransAndOtherVehicles.Train.Locomotives;

public class ElectricLocomotive(int weight, int power, int maxSpeed) : Locomotive(weight, power, maxSpeed), IElectricPowered
{
    public int BatteryLevel { get; private set; } = 100;

    public void Charge()
    {
        BatteryLevel = 100;
    }

    internal override void Move()
    {
        base.Move();
        BatteryLevel -= 20; // in a real implementation this would be done a lot different ofc
    }
}