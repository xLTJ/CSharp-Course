// See https://aka.ms/new-console-template for more information

using TransAndOtherVehicles.Train.Locomotives;

Console.WriteLine("Hello, World!");

var loco = new ElectricLocomotive(2, 2, 2);
loco.Move();