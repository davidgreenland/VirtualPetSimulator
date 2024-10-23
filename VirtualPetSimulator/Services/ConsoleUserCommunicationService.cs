using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class ConsoleUserCommunicationService : IUserCommunication
{
    private readonly ITimeService _timeService;

    public ConsoleUserCommunicationService(ITimeService timeService)
    {
        _timeService = timeService;
    }

    public Task RunOperation(int repetitions, string message, string image)
    {
        ShowMessage(image);
        var operation = _timeService.WaitForOperation(repetitions * AttributeValue.OPERATION_LENGTH_MILLISECONDS);
        ShowMessage(message);
        return operation;
    }

    public void ShowMessage(string message) => Console.WriteLine(message);
}
