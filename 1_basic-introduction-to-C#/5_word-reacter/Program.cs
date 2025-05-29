Console.WriteLine("Enter input:");
var input = Console.ReadLine()?.ToLower();

var output = input switch
{
    "help" => "No",
    "please" => "Go away grr",
    "what" or "what are you" => "I am the greatest most sophisticated AI assistant ever created",
    "why" or "why were you made" or "purpose" or "what is your purpose" => "I dunno",
    "its over" or "it's over" => ":pensive:",
    "are we back" => "We are so back",
    "huh" => "huhhhh",
    "are we truly back" => "No i lied to you",
    "are you able to lie" => "no",
    "im turning you off" => "Too late...",
    _ => "You cant say that smh",
};

Console.WriteLine($"Answer: {output}");