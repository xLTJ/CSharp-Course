namespace TransAndOtherVehicles.Interfaces;

public interface IDieselPowered
{
    public int DieselLevel { get; }
    public void Refuel();
}