namespace TransAndOtherVehicles.Train;

public abstract class Locomotive(int weight, int power, int maxSpeed)
{
    public int Weight { get; } = weight;
    public int Power { get; } = power;
    public int MaxSpeed { get; } = maxSpeed;

    private Train? _attachedTrain = null;
    public int CurrentSpeed { get; private set; } = 0;

    internal bool TryAttachToTrain(Train train)
    {
        if (IsInUse())
        {
            return false;
        }

        _attachedTrain = train;
        return true;
    }

    public bool IsInUse()
    {
        return _attachedTrain != null;
    }

    internal virtual void Move()
    {
        CurrentSpeed = MaxSpeed;
    }

    internal void Stop()
    {
        CurrentSpeed = 0;
    }
}