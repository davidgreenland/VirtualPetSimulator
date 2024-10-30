using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Validators.Interfaces;

namespace VirtualPetSimulator.Actions;

public class ExitAction : IPetAction
{
    private readonly IUserCommunication _userCommunication;
    private readonly ITimer _timer;

    public ExitAction(IUserCommunication userCommunication, ITimer appTimer)
    {
        _userCommunication = userCommunication;
        _timer = appTimer;
    }

    public async Task<int> Execute()
    {
        _timer.Dispose();
        _userCommunication.ClearScreen();
        _userCommunication.ShowMessage("\n           Thanks for coming! Goodbye.");
        _userCommunication.WaitForUser();

        return 0;
    }
}
