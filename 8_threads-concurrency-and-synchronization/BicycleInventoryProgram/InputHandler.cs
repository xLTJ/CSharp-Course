using System.Collections.Concurrent;
using System.Threading.Channels;

namespace BicycleInventoryProgram;

public class InputHandler : IInputHandler
{
    private readonly Channel<InputCommand> _commandChan = Channel.CreateUnbounded<InputCommand>();
    public ChannelReader<InputCommand> CommandChanReader => _commandChan.Reader;

    public bool IsRunning { get; private set; } = true;

    public async Task Start()
    {
        Console.WriteLine("Welcome to BicycleManagementSystem2000 (woahh)");
        Console.Write("Use `help` to see a list of commands");
        Console.Write("\n> ");

        while (IsRunning)
        {
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input)) continue;

            var command = ParseInput(input);
            if (command != null)
            {
                await _commandChan.Writer.WriteAsync(command);
            }
        }

        _commandChan.Writer.Complete();
    }

    private InputCommand? ParseInput(string input)
    {
        var parts = input.Split(' ');
        var commandType = parts[0].ToLower();

        return commandType switch
        {
            "help" => new InputCommand(CommandType.Help, []),
            "list" => new InputCommand(CommandType.List, []),
            "view" => new InputCommand(CommandType.View, parts.Skip(1).ToArray()),
            "search" => new InputCommand(CommandType.Search, parts.Skip(1).ToArray()),
            "add" => new InputCommand(CommandType.Add, parts.Skip(1).ToArray()),
            "remove" => new InputCommand(CommandType.Remove, parts.Skip(1).ToArray()),
            "update" => new InputCommand(CommandType.Update, parts.Skip(1).ToArray()),
            "exit" => HandleExit(),
            _ => new InputCommand(CommandType.Unknown, [])
        };
    }

    private InputCommand HandleExit()
    {
        IsRunning = false;
        return new InputCommand(CommandType.Exit, []);
    }
}

public record InputCommand(CommandType CommandType, string[] Arguments);

public enum CommandType
{
    Help,
    List,
    View,
    Search,
    Add,
    Remove,
    Update,
    Exit,
    Unknown
}