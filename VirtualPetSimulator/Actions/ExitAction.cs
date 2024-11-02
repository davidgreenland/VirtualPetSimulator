using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Validators.Interfaces;

namespace VirtualPetSimulator.Actions;

public class ExitAction : IPetAction
{
    private readonly IUserCommunication _userCommunication;

    public ExitAction(IUserCommunication userCommunication)
    {
        _userCommunication = userCommunication;
    }

    public async Task<int> Execute()
    {
        _userCommunication.ClearScreen();
        _userCommunication.ShowMessage("\n           Thanks for coming! Goodbye.");
        _userCommunication.WaitForUser();

        return 0;
    }
}
