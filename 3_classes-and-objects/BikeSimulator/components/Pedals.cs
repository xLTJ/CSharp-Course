namespace BikeSimulator.components;

public class Pedals
{
    public double Step(int currentGear, double currentSpeed)
    {
        Console.WriteLine("Stepped Pedals");
        const double baseSpeedPerGear = 2;

        // the speed where the gear is at optimal efficiency
        var optimalSpeed = 5.0 * Math.Pow(1.4, currentGear - 1);

        // gaussian efficiency calculation
        var speedDifference = Math.Abs(currentSpeed - optimalSpeed);
        var sigma = 12.0;
        var efficiency = Math.Exp(-Math.Pow(speedDifference, 2) / (2 * Math.Pow(sigma, 2)));
        efficiency = Math.Max(0.10, efficiency); // min 10% efficiency

        var gearModifier = 1.0 + (currentGear - 1) * 0.15;

        var speedIncrease = (gearModifier * baseSpeedPerGear * efficiency) - Math.Pow(1.015, currentSpeed) + 1;
        return speedIncrease;
    }
}