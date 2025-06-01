using BicycleInventoryProgram;

await Main();
return;

async Task Main()
{
    Console.WriteLine("Select program mode:");
    Console.WriteLine("1. Normal Program (default)");
    Console.WriteLine("2. Simulation Mode");

    var choice = Console.ReadLine();
    var task = choice switch
    {
        "2" => MultipleHandlersSimulation(),
        _ => NormalProgram(),
    };
    await task;
}

async Task NormalProgram()
{
    var bicycleList = new BicycleList();
    var inputHandler = new InputHandler();
    var bicycleListManager = new BicycleListManager(bicycleList, inputHandler);

    var inputTask = Task.Run(inputHandler.Start);
    var bicycleManagementTask = Task.Run(bicycleListManager.Start);

    await Task.WhenAll(inputTask, bicycleManagementTask);
}

async Task MultipleHandlersSimulation()
{
    Console.WriteLine("How many simulations to run?");
    var simNumberInput = Console.ReadLine();
    Console.WriteLine("How many tasks to give each simulation?");
    var simTasksInput = Console.ReadLine();
    if (!int.TryParse(simNumberInput, out var simNumber) || !int.TryParse(simTasksInput, out var simTaskNumber))
    {
        Console.WriteLine("Guh");
        return;
    }

    var bicycleList = new BicycleList();
    List<SimulatedInputHandler> inputSimulations = [];
    List<BicycleListManager> bicycleListManagers = [];
    List<Task> tasks = [];

    var startTime = DateTime.Now;

    for (var i = 1; i <= simNumber; i++)
    {
        var simulatedHandler = new SimulatedInputHandler(i, simTaskNumber);
        var bicycleManager = new BicycleListManager(bicycleList, simulatedHandler);

        tasks.Add(Task.Run(simulatedHandler.Start));
        tasks.Add(Task.Run(bicycleManager.Start));
    }

    await Task.WhenAll(tasks);
    var endTime = DateTime.Now;
    var duration = endTime - startTime;

    Console.WriteLine($"\n=== SIMULATION COMPLETE ===");
    Console.WriteLine($"Simulations: {simNumber}");
    Console.WriteLine($"Actions per simulation: {simTaskNumber}");
    Console.WriteLine($"Total actions: {simNumber * simTaskNumber}");
    Console.WriteLine($"Duration: {duration.TotalMilliseconds:F0}ms");
    Console.WriteLine($"Final bicycle count: {bicycleList.GetBicycles().Count}");

}