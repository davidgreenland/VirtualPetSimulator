using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class ConsoleUserCommunication : IUserCommunication
{
    public Task RunOperation(int repetitions, string message)
    {
        // repeat messages
        return Task.Delay(repetitions * AttributeValue.OPERATION_LENGTH_MILLISECONDS);
    }

    public void ShowMessage(string message) => Console.WriteLine(message);
}
