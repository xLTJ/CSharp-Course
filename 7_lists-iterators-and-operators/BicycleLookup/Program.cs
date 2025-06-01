using BicycleLookup;
using VehicleProject.Vehicles;

Main();
return;

void Main()
{
    // setup
    var bicycleManager = new BicycleManager();
    var rand = new Random();
    List<string> models = ["TheMountainBike", "TheCityBike", "TheRacingBike", "TheOldDecayingBike", "\"Bike\""];
    List<string> colors = ["black", "blue", "red", "while", "purple", "pink", "lavenderMist"];

    // add 100 bicycles
    for (var i = 0; i < 100; i++)
    {
        bicycleManager.AddBicycle(new Bicycle(
            model: models[rand.Next(models.Count)],
            year: rand.Next(2000, 2026),
            color: colors[rand.Next(colors.Count)])
        );
    }

    var commandLineInterface = new CommandLineInterface(bicycleManager);
    commandLineInterface.Start();
}