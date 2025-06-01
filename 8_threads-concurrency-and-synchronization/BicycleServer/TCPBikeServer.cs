using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using BicycleServer;
using Shared;

namespace BicycleClient;

public class TCPBikeServer(BicycleInventoryService bicycleInventoryService, int port = 8000)
{
    private readonly BicycleInventoryService _bicycleInventoryService = bicycleInventoryService;
    private readonly TcpListener _tcpListener = new TcpListener(IPAddress.Any, port);
    private readonly List<TcpClient> _clientConnections = [];
    private bool _isRunning;

    public async Task Start()
    {
        _tcpListener.Start();
        _isRunning = true;

        Console.WriteLine($"[LOG] Server listening on port {port}");

        while (_isRunning) // TODO maybe try/catch
        {
            var client = await _tcpListener.AcceptTcpClientAsync();
            Console.WriteLine($"[LOG] Client connected: {client.Client.RemoteEndPoint}");
            _clientConnections.Add(client);

            // clients go to their own thread so we can keep listening
            _ = Task.Run(() => HandleClient(client));
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        var clientEndpoint = client.Client.RemoteEndPoint?.ToString() ?? "Unknown";

        var stream = client.GetStream();
        var reader = new StreamReader(stream); // TODO maybe UTF
        var writer = new StreamWriter(stream)
        {
            AutoFlush = true
        };

        while (client.Connected && _isRunning)
        {
            // get request
            var requestJson = await reader.ReadLineAsync();
            if (requestJson == null) continue; // TODO maybe break

            var request = JsonSerializer.Deserialize<CommandRequest>(requestJson);
            if (request == null) continue;

            Console.WriteLine($"[LOG] Recieved from {clientEndpoint}: {request.CommandType} command");

            // process command
            var response = _bicycleInventoryService.ProcessCommand(request);

            // send response
            var responseJson = JsonSerializer.Serialize(response);
            await writer.WriteLineAsync(responseJson);

            if (request.CommandType == CommandType.Exit)
            {
                break;
            }
        }

        _clientConnections.Remove(client);
        Console.WriteLine($"[LOG] Client {clientEndpoint} has disconnected");
    }

    public void Stop()
    {
        _isRunning = false;
        _tcpListener.Stop();

        foreach (var clientConnection in _clientConnections)
        {
            clientConnection.Close();
        }
        _clientConnections.Clear();

        Console.WriteLine("[LOG] Server has stopped");
    }
}