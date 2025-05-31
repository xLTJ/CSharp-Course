namespace BicyclesAndDataStructures;

public class Bicycle(int speed, string name)
{
    public int Speed { get; set; } = speed;
    public string Name { get; set; } = name;

    public Bicycle Clone()
    {
        return new Bicycle(Speed, Name);
    }
}