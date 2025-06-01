using System.Net.Sockets;
using Shared;

namespace BicycleClient;

public class InputHandler
{
    private readonly ClientService _clientService = new ClientService();
    private readonly TCPBikeClient _tcpBikeClient = new TCPBikeClient();

    public async Task Start()
    {
        Console.WriteLine("Welcome to BicycleManagementSystem2000 (woahh)");

        await _tcpBikeClient.Connect();

        Console.Write("Use `help` to see a list of commands");

        while (true)
        {
            Console.Write("\n> ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input)) continue;

            var command = ParseInput(input);
            if (command == null) continue;

            var response = await _tcpBikeClient.SendCommand(command);

            if (response != null)
            {
                _clientService.DisplayResponse(response);
            }

            if (command.CommandType == CommandType.Exit)
            {
                _tcpBikeClient.Disconnect();
                break;
            }
        }
    }

    private CommandRequest? ParseInput(string input)
    {
        var parts = input.Split(' ');
        var commandType = parts[0].ToLower();

        return commandType switch
        {
            "help" => new CommandRequest(CommandType.Help, []),
            "list" => new CommandRequest(CommandType.List, []),
            "view" => new CommandRequest(CommandType.View, parts.Skip(1).ToArray()),
            "search" => new CommandRequest(CommandType.Search, parts.Skip(1).ToArray()),
            "add" => new CommandRequest(CommandType.Add, parts.Skip(1).ToArray()),
            "remove" => new CommandRequest(CommandType.Remove, parts.Skip(1).ToArray()),
            "update" => new CommandRequest(CommandType.Update, parts.Skip(1).ToArray()),
            "exit" => new CommandRequest(CommandType.Exit, []),
            _ => new CommandRequest(CommandType.Unknown, [])
        };
    }

    // Temporary method TODO: will be replaced with actual network communication
    private CommandResponse SimulateServerResponse(CommandRequest request)
    {
        return new CommandResponse(false, "Not connected to server yet. Network communication will be implemented in Task 3.");
    }

}