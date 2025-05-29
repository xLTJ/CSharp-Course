namespace BikeSimulator.components;

public class Wheel(string name)
{
    public string Name { get; private set; } = name;
    public bool IsAttachedToBike { get; private set; } = true;
    private Tire _tire = new Tire(name);

    public void RemoveWheel()
    {
        if (!IsAttachedToBike)
        {
            Console.WriteLine("Wheel already removed");
            return;
        }
        Console.WriteLine($"Removing {Name} wheel");
        IsAttachedToBike = false;
    }

    public void AttachWheel()
    {
        if (IsAttachedToBike)
        {
            Console.WriteLine("Wheel already attached");
            return;
        }
        Console.WriteLine($"Attaching {Name} wheel");
        IsAttachedToBike = true;
    }

    public void FixTire()
    {
        if (IsAttachedToBike)
        {
            Console.WriteLine("Cannot fix wheel when attached to bike");
            return;
        }
        Console.WriteLine($"Fixing tire from {Name} wheel");
        _tire.FixHole();
        return;
    }

    public void PunctureTire()
    {
        Console.WriteLine($"Puncturing tire on {Name} wheel");
        _tire.PunctureTire();
    }

    public bool HasHoleInTire()
    {
        return _tire.HasHole;
    }
}