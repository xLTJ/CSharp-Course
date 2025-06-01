using System.Threading.Channels;

namespace BicycleInventoryProgram;

public class SimulatedInputHandler(int simulationId, int actionsAmount) : IInputHandler
{
    private readonly Channel<InputCommand> _commandChan = Channel.CreateUnbounded<InputCommand>();
    public ChannelReader<InputCommand> CommandChanReader => _commandChan.Reader;
    private string[] _commandTypes = ["add", "add", "add", "search", "search", "search", "search", "list", "list", "list", "remove", "update", "update"];
    private string[] _bicycleColors = ["Black", "White", "LavenderMist"];


    public int SimulationId { get; } = simulationId;
    private int _actionsAmount = actionsAmount;

    public bool IsRunning { get; private set; } = true;
    private readonly Random _random = new Random();

    public async Task Start()
    {
        Console.WriteLine($"Started simulation id: {SimulationId}");

        for (var i = 0; i < _actionsAmount; i++)
        {
            var command = GenerateRandomCommand();
            await _commandChan.Writer.WriteAsync(command);

            await Task.Delay(100);
        }

        _commandChan.Writer.Complete();
    }

    private InputCommand GenerateRandomCommand()
    {
        var randomCommand = _commandTypes[_random.Next(_commandTypes.Length)];

        return randomCommand switch
        {
            "add" => new InputCommand(CommandType.Add,
            [
                $"Bicycle{_random.Next(1000, 10000)}",
                _bicycleColors[_random.Next(_bicycleColors.Length)],
                _random.Next(2000, 2026).ToString()
            ]),
            "search" => new InputCommand(CommandType.Search,
            [
                "color", "=", "LavenderMist"
            ]),
            "list" => new InputCommand(CommandType.List, []),
            "remove" => new InputCommand(CommandType.Remove,
            [
                _random.Next(5).ToString() // might be out of range but we have ✨input validation✨ so it doesnt matter
            ]),
            "update" => new InputCommand(CommandType.Update,
            [
                _random.Next(5).ToString(), "color", _bicycleColors[_random.Next(_bicycleColors.Length)] // same here as above
            ]),
            _ => new InputCommand(CommandType.List, [])
        };
    }
}