using BicyclesAndDataStructures;

var rand = new Random();
var nameGenerator = new BicycleNameGenerator();

Main();
return;

void Main()
{
    // start with a normal list of 100 bicycles
    var bicycles = new List<Bicycle>();
    for (var i = 0; i < 100; i++)
    {
        bicycles.Add(GenerateRandomBicycle());
    }

    ProcessBicyclesStack(bicycles);
    ProcessBicyclesQueue(bicycles);
    ProcessBicyclesLinkedList(bicycles);

    var bicycleHashmap = CreateBicycleHashmap(bicycles);

    Console.WriteLine("\n\nSearch bicycles by name, 'exit' to leave");
    var input = "";
    while (input != "exit")
    {
        input = Console.ReadLine() ?? "";

        if (input == "exit") continue;

        var ok = bicycleHashmap.TryGetValue(input, out var bicycle);

        // (`ok` should already provide validation safety, but the compiler is still like "oh nooo, it might be null omg")
        if (ok && bicycle != null)
        {
            Console.WriteLine($"--- Bike: {bicycle.Name} ---\nspeed: {bicycle.Speed}\n");
            continue;
        }
        Console.WriteLine("Bicycle doesn't exist ;-;\n");
    }
}

void ProcessBicyclesLinkedList(List<Bicycle> list)
{
    // ---- create linked list ----
    Console.WriteLine("\n\n ===== Linked List ===== ");
    var bicycleLinkedList = new LinkedList<Bicycle>();
    foreach (var bicycle in list)
    {
        bicycleLinkedList.AddLast(bicycle.Clone());
    }

    Console.WriteLine($"\n --- Original ({bicycleLinkedList.Count} elements) --- ");
    foreach (var bicycle in bicycleLinkedList)
    {
        Console.Write($" [{bicycle.Name} ({bicycle.Speed})] ");
    }

    // ---- iterate over linked list ----
    // and remove all slower than 10 km/h
    var currentBicycle = bicycleLinkedList.First;
    while (currentBicycle != null)
    {
        var nextBicycle = currentBicycle.Next;

        if (currentBicycle.Value.Speed < 10)
        {
            bicycleLinkedList.Remove(currentBicycle);
        }

        currentBicycle = nextBicycle;
    }

    Console.WriteLine($"\n\n --- Filtered ({bicycleLinkedList.Count} elements) --- ");
    foreach (var bicycle in bicycleLinkedList)
    {
        Console.Write($" [{bicycle.Name} ({bicycle.Speed})] ");
    }
}

void ProcessBicyclesQueue(List<Bicycle> list)
{
    // ---- create priority queue ----
    Console.Write("\n\n ===== Priority Queue =====");
    var bicyclePriorityQueue = new PriorityQueue<Bicycle, int>();
    foreach (var bicycle in list)
    {
        var bicycleClone = bicycle.Clone();
        bicyclePriorityQueue.Enqueue(bicycleClone, bicycleClone.Speed);
    }

    Console.WriteLine($"\n --- Original ({list.Sum(bicycle => bicycle.Speed)} combined speed) --- ");
    foreach (var bicycle in list)
    {
        Console.Write($" [{bicycle.Name} ({bicycle.Speed})] ");
    }

    // ---- iterate over queue ----
    var queueElementList = new List<Bicycle>();
    while (bicyclePriorityQueue.Count > 0)
    {
        var dequeuedBicycle = bicyclePriorityQueue.Dequeue();
        dequeuedBicycle.Speed = (int)(dequeuedBicycle.Speed * 0.75);
        queueElementList.Add(dequeuedBicycle);
    }

    Console.WriteLine($"\n\n --- Modified ({queueElementList.Sum(bicycle => bicycle.Speed)} combined speed) --- ");
    foreach (var bicycle in queueElementList)
    {
        Console.Write($" [{bicycle.Name} ({bicycle.Speed})] ");
    }
}

void ProcessBicyclesStack(List<Bicycle> list)
{
    // ---- create stack ----
    Console.Write("\n\n ===== Stack =====");
    var bicycleStack = new Stack<Bicycle>();
    foreach (var bicycle in list)
    {
        bicycleStack.Push(bicycle.Clone());
    }

    Console.WriteLine($"\n --- Original ({list.Sum(bicycle => bicycle.Speed)} combined speed) --- ");
    foreach (var bicycle in list)
    {
        Console.Write($" [{bicycle.Name} ({bicycle.Speed})] ");
    }

    // ---- iterate over stack ----
    var stackElementsList = new List<Bicycle>();
    while (bicycleStack.Count > 0)
    {
        var removedBicycle = bicycleStack.Pop();
        removedBicycle.Speed = (int)(removedBicycle.Speed * 0.75);
        stackElementsList.Add(removedBicycle);
    }

    Console.WriteLine($"\n\n --- Modified ({stackElementsList.Sum(bicycle => bicycle.Speed)} combined speed) --- ");
    foreach (var bicycle in stackElementsList)
    {
        Console.Write($" [{bicycle.Name} ({bicycle.Speed})] ");
    }
}

Dictionary<string, Bicycle> CreateBicycleHashmap(List<Bicycle> list)
{
    var bicycleHashmap = new Dictionary<string, Bicycle>();
    foreach (var bicycle in list)
    {
        bicycleHashmap[bicycle.Name] = bicycle;
    }

    return bicycleHashmap;
}

Bicycle GenerateRandomBicycle()
{
    return new Bicycle(rand.Next(51), nameGenerator.GenerateRandomName());
}