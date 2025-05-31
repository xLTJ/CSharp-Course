namespace TransAndOtherVehicles.Interfaces;

public interface IElectricPowered
{
    public int BatteryLevel { get; }
    void Charge();
}