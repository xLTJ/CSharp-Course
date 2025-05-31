using TransAndOtherVehicles.Interfaces;

namespace TransAndOtherVehicles.Train.Locomotives;

public class DieselLocomotive(int weight, int power, int maxSpeed) : Locomotive(weight, power, maxSpeed), IDieselPowered
{
    public int DieselLevel { get; private set; } = 100;

    public void Refuel()
    {
        DieselLevel = 100;
    }

    internal override void Move()
    {
        base.Move();
        DieselLevel -= 20; // in a real implementation this would be done a lot different ofc
    }
}