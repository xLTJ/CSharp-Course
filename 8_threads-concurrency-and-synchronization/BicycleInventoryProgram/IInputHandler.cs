using System.Threading.Channels;

namespace BicycleInventoryProgram;

public interface IInputHandler
{
    ChannelReader<InputCommand> CommandChanReader { get; }
    Task Start();
    bool IsRunning { get; }

}