Console.WriteLine("Welcome to Guessing Game TM !!");

var rand = new Random();
var randomNumber = rand.Next(1000);
while (true)
{
    Console.WriteLine("\nGuess the number:");
    var guess = int.Parse(Console.ReadLine() ?? "0");

    if (guess == randomNumber) break;

    if (guess < randomNumber)
    {
        Console.WriteLine("Too low");
        continue;
    }
    Console.WriteLine("Too high");
}

Console.WriteLine("Woahh, u guessed the number !!");