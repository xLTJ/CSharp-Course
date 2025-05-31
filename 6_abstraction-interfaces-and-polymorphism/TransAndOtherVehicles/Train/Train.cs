namespace TransAndOtherVehicles.Train;

public class Train(int maxWagons) : IVehicle
{
    public int CurrentSpeed => Locomotive?.CurrentSpeed ?? 0;
    public int MaxSpeed => Locomotive?.MaxSpeed ?? 0;
    public bool IsOperational => Locomotive != null;

    public int MaxWagons { get; } = maxWagons;
    protected Locomotive? Locomotive = null;
    protected readonly List<Wagon> Wagons = [];

    /// <returns>True if locomotive was moved, false if no locomotive is attached</returns>
    public bool TryMove()
    {
        if (Locomotive == null)
        {
            return false;
        }
        Locomotive.Move();
        return true;
    }

    /// <returns>True if locomotive was stopped, false if no locomotive is attached</returns>
    public bool TryStop()
    {
        if (Locomotive == null)
        {
            return false;
        }
        Locomotive.Stop();
        return true;
    }

    /// <returns>True if the locomotive was attached, false if a locomotive is already attached</returns>
    public bool TryAttachLocomotive(Locomotive locomotive)
    {
        if (Locomotive != null)
        {
            return false;
        }

        Locomotive = locomotive;
        return true;
    }

    /// <returns>The detached locomotive if one exists, null if no locomotive was attached</returns>
    public Locomotive? DetachLocomotive()
    {
        var oldLocomotive = Locomotive;
        Locomotive = null;
        return Locomotive;
    }

    /// <returns>True if the wagon was attached, false if there is not capacity for an extra wagon</returns>
    public (bool ok, int index) TryAttachWagon(Wagon wagon)
    {
        if (Wagons.Count == MaxWagons)
        {
            return (false, 0);
        }

        var index = Wagons.Count;
        Wagons.Add(wagon);
        return (true, index);
    }

    /// <returns>The detached wagon if one exists at given index, otherwise null</returns>
    public Wagon? DetachWagon(int index)
    {
        if (index < 0 || index >= Wagons.Count)
        {
            return null;
        }

        var wagonToDetach = Wagons[index];
        Wagons.RemoveAt(index);
        return wagonToDetach;
    }

    /// <returns>The wagon if one exists at given index, otherwise null</returns>
    public Wagon? GetWagon(int index)
    {
        if (index < 0 || index >= Wagons.Count)
        {
            return null;
        }

        return Wagons[index];
    }
}