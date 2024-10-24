using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Helpers.Interfaces;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class VirtualPetApp
{
    private readonly IPet _pet;
    private readonly Dictionary<char, PetAction> _petActions;
    private readonly IValidator _validator;
    private readonly IUserCommunication _userCommunication;

    public VirtualPetApp(IPet pet, Dictionary<char, PetAction> petActions, IValidator validator, IUserCommunication userCommunication)
    {
        _pet = pet;
        _petActions = petActions;
        _validator = validator;
        _userCommunication = userCommunication;
    }

    public async Task StartApp()
    {
        bool running = true;
        PetAction userChoice;
        _userCommunication.ShowMessage("Welcome to the Virtual Pet Simulator");
        _userCommunication.ShowMessage("Press any key to begin");
        _userCommunication.WaitForUser();

        while (running)
        {
            _userCommunication.RenderScreen();
            userChoice = GetUserChoice();

            IPetAction? petAction = null;

            switch (userChoice)
            {
                case PetAction.Sleep:
                    petAction = new SleepAction(_pet, _validator, _userCommunication);
                    break;
                case PetAction.Eat:
                    petAction = new EatAction(_pet, _validator, _userCommunication);
                    break;
                case PetAction.Play:
                    petAction = new PlayAction(_pet, _validator, _userCommunication);
                    break;
                default:
                    _pet.CurrentAction = PetAction.Sit;
                    break;
            }

            if (petAction != null)
            {
                await petAction.Execute();
                _pet.CurrentAction = PetAction.Sit;
            }
        }
    }

    private PetAction GetUserChoice()
    {
        char userChoice;
        do
        {
            _userCommunication.RenderScreen();
            ShowOptions();
            userChoice = _userCommunication.GetUserChoice("Choose an option: ");
        }
        while (!_petActions.ContainsKey(userChoice));

        return _petActions[userChoice];
    }

    private void ShowOptions()
    {
        // todo: deal with null in pet actions dictionary
        var count = 0;
        foreach (var item in _petActions)
        {
            var optionName = Enum.GetNames(typeof(PetAction));
            var actionKey = optionName[count][0];

            _userCommunication.ShowMessage($"[{actionKey}]{optionName[count].Substring(1)}");
            count++;
        }
    }
}