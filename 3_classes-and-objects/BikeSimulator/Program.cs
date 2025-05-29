using BikeSimulator;

Console.WriteLine("\"Biking Time !!\"");
Thread.Sleep(1000);

var bicycle = new Bicycle(7);

Console.WriteLine("\"Yay i cant wait to bike\"\n");
Thread.Sleep(2000);
Console.WriteLine("*Suddenly, Evil Larry shows up, puncturing the front wheel before disappearing again*\n");
Thread.Sleep(5000);
bicycle.PunctureFrontTire();
Thread.Sleep(2000);
Console.WriteLine("\"Oh noes, theres a hole in my front tire ;-;\"\n");
Thread.Sleep(2000);
Console.WriteLine("\"Lemme fix it\"\n");
Thread.Sleep(2000);
bicycle.RemoveFrontTire();
Thread.Sleep(2000);
bicycle.FixFrontTire();
Thread.Sleep(2000);
bicycle.AttachFrontTire();
Thread.Sleep(2000);
Console.WriteLine("\"There we go ^-^\"\n");
Thread.Sleep(2000);
Console.WriteLine("\"Im going to ride my bike now, i will start at gear 7 lol\"\n");
Thread.Sleep(2000);

PedalTimes(3);

Console.WriteLine("\"This is very inefficient...\"\n");
Thread.Sleep(2000);

StopBike();

Console.WriteLine("\"Lets change to gear 1\"\n");
Thread.Sleep(2000);

ShiftGearTo(1);
Thread.Sleep(2000);

Console.WriteLine("\"Time to try again...\"\n");

PedalTo(30);

Console.WriteLine("\"Wait i forgot the lights, silly me\"");
Thread.Sleep(2000);
StopBike();
Thread.Sleep(2000);
bicycle.ToggleFrontLight();
Thread.Sleep(500);
bicycle.ToggleRearLight();
Thread.Sleep(1000);
Console.WriteLine("\"NOW we can bike hehe\"");
Thread.Sleep(1000);

PedalTo(1);
Thread.Sleep(500);
Console.WriteLine("\"Guh the gear-\"\n");
Thread.Sleep(1000);
ShiftGearTo(1);
Thread.Sleep(1000);
PedalTo(50);

Console.WriteLine("\"Uhh this is too fast, i need to stop.\"\n");

bicycle.Brake(true, false);
return;

void PedalTimes(int n)
{
    for (var i = 0; i < n; i++)
    {
        bicycle.StepPedal();
        Console.WriteLine("Current speed: " + bicycle.Speed + "\n");
        Thread.Sleep(bicycle.PedalCooldownExpire - DateTime.Now);
    }
}

void PedalTo(double target)
{
    while (bicycle.Speed < target)
    {
        if (bicycle.GetOptimalSpeed() < bicycle.Speed)
        {
            bicycle.GearShiftUp();
        }
        bicycle.StepPedal();
        Console.WriteLine("Current speed: " + bicycle.Speed + "\n");
        Thread.Sleep(bicycle.PedalCooldownExpire - DateTime.Now);
    }
}

void StopBike()
{
    while (bicycle.Speed > 0)
    {
        bicycle.Brake(brakeFront: true, brakeRear: true);
        Console.WriteLine("Current speed: " + bicycle.Speed + "\n");
        Thread.Sleep(bicycle.BrakeCooldownExpire - DateTime.Now);
    }
}

void ShiftGearTo(int target)
{
    while (bicycle.GetCurrentGear() != target)
    {
        if (bicycle.GetCurrentGear() > target) bicycle.GearShiftDown();
        else bicycle.GearShiftUp();
    }
}