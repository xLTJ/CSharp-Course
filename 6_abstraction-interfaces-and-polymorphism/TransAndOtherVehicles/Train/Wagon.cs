namespace TransAndOtherVehicles.Train;

public abstract class Wagon(int weight)
{
    public int Weight { get; } = weight;

    private Train? _attachedTrain = null;

    public bool IsInUse()
    {
        return _attachedTrain != null;
    }
}