namespace BikeSimulator.components;

public class Tire(string name)
{
    public bool HasHole { get; private set; } = false;
    public string Name { get; private set; } = name;

    public void FixHole()
    {
        if (!HasHole)
        {
            Console.WriteLine("Tire has no hole...");
            return;
        };
        Console.WriteLine($"Fixed hole in {Name} tire");
        HasHole = false;
    }

    public void PunctureTire()
    {
        Console.WriteLine($"{Name} tire has been punctured{(HasHole ? " (again)": "")}");
        HasHole = true;
    }
}