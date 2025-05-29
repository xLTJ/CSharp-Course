namespace BikeSimulator.components;

public class Brake(string name)
{
    private static readonly Random Rand = new Random();
    private double _condition = Rand.Next(100);
    public string Name { get; private set; } = name;

    public double Use(double speed)
    {
        Console.WriteLine("Activated Brake: " + Name);
        _condition-= 0.1;
        return (speed / 10) * (_condition / 100) + 2; // removes 10% of speed at prime condition, + 2 so it actually brakes at the end
        // speed decays as condition gets worse
    }
}