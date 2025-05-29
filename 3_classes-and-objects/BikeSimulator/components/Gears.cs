namespace BikeSimulator.components;

public class Gears(int maxGear, int startGear)
{
    public int CurrentGear { get; private set; } = startGear;
    private readonly int _maxGear = maxGear;

    public void ShiftUp()
    {
        if (CurrentGear == _maxGear)
        {
            Console.WriteLine("Already on highest gear");
            return;
        }

        CurrentGear++;
        Console.WriteLine("Gear shift up to: " + CurrentGear);
    }

    public void ShiftDown()
    {
        if (CurrentGear == 1)
        {
            Console.WriteLine("Already on lowest gear");
            return;
        }

        CurrentGear--;
        Console.WriteLine("Gear shift down to: " + CurrentGear);
    }
}