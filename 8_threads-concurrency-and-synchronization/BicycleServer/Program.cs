using BicycleClient;
using BicycleServer;
using Shared;

Console.WriteLine("=== Bicycle Inventory Server ===");
Console.WriteLine("Server starting...");

var bicycleInventoryService = new BicycleInventoryService();
var tcpServer = new TCPBikeServer(bicycleInventoryService);

var serverTask = Task.Run(tcpServer.Start);

Console.WriteLine("Server ready!");

await serverTask;