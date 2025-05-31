namespace TransAndOtherVehicles;

public interface IVehicle
{
    public int CurrentSpeed { get; }
    public int MaxSpeed { get; }
    public bool IsOperational { get; }

    public bool TryMove();
    public bool TryStop();
}