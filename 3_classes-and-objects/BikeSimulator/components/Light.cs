namespace BikeSimulator.components;

public class Light(string name)
{
    public bool IsTurnedOn { get; private set; } = false;
    public string Name { get; private set; } = name;

    public void ToggleLight()
    {
        IsTurnedOn = !IsTurnedOn;
        Console.WriteLine($"{Name} lights turned: {(IsTurnedOn ? "on" : "off")}");
    }
}