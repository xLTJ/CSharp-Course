Console.WriteLine("Input sentence:");
var sentence = Console.ReadLine();

Console.WriteLine("Input x:");
var x = int.Parse(Console.ReadLine() ?? "0");

if (x < 1)
{
    Console.WriteLine("0 or invalid input as x, quiting...");
    return;
}

var i = 0;
while (i < x)
{
    Console.WriteLine($"{i + 1}: {sentence}");
    i++;
};