using System.Net.Sockets;
using System.Text.Json;
using Shared;

namespace BicycleClient;

public class TCPBikeClient
{
    private readonly TcpClient _tcpClient = new TcpClient();
    private StreamReader _tcpReader;
    private StreamWriter _tcpWriter;
    private bool _isConnected;

    public async Task Connect(string host = "localhost", int port = 8000)
    {
        await _tcpClient.ConnectAsync(host, port);

        var stream = _tcpClient.GetStream();
        _tcpReader = new StreamReader(stream); // TODO maybe UTF
        _tcpWriter = new StreamWriter(stream)
        {
            AutoFlush = true
        };

        _isConnected = true;

        Console.WriteLine($"Connected to bicycle server at {host}:{port}");
    }

    public async Task<CommandResponse?> SendCommand(CommandRequest request)
    {
        if (!_isConnected)
        {
            return new CommandResponse(false, "Not connected to server");
        }

        var requestJson = JsonSerializer.Serialize(request);
        await _tcpWriter.WriteLineAsync(requestJson);

        var responseJson = await _tcpReader.ReadLineAsync();
        if (responseJson == null)
        {
            return new CommandResponse(false, "Could not get response from server, probably disconnected");
        }

        return JsonSerializer.Deserialize<CommandResponse>(responseJson);
    }

    public void Disconnect()
    {
        _isConnected = false;
        _tcpReader.Close();
        _tcpWriter.Close();
        _tcpClient.Close();
        Console.WriteLine("Disconnected from server");
    }

    public void Dispose()
    {
        Disconnect();
        _tcpReader.Dispose();
        _tcpWriter.Dispose();
        _tcpClient.Dispose();
    }
}