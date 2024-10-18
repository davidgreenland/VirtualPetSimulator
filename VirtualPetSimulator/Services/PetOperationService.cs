using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class PetOperationService : IOperationService
{
    private readonly IUserCommunication _userCommunication;

    public Task RunOperation(int repetitions, string message)
    {
        // repeat messages
        return Task.Delay(repetitions * AttributeValue.OPERATION_LENGTH_MILLISECONDS);
    }
}
