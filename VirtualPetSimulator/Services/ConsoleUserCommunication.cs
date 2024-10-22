using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class ConsoleUserCommunication : IUserCommunication
{
    private readonly ITimeService _timeService;

    public ConsoleUserCommunication(ITimeService timeService)
    {
        _timeService = timeService;
    }

    public Task RunOperation(int repetitions, string message)
    {
        // todo: show messages and cat ascii
        return _timeService.WaitForOperation(repetitions * AttributeValue.OPERATION_LENGTH_MILLISECONDS);
    }

    public void ShowMessage(string message) => Console.WriteLine(message);
}
