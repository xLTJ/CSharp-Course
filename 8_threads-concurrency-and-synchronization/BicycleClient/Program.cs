using BicycleClient;

Console.WriteLine("Client starting...");

var inputHandler = new InputHandler();
await inputHandler.Start();

Console.WriteLine("Client shutting down...");