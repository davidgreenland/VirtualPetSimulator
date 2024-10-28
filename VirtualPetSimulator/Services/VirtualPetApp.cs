using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class VirtualPetApp
{
    private readonly IPet _pet;
    private readonly Dictionary<char, PetAction> _petActions;
    private readonly IUserCommunication _userCommunication;
    private readonly ITimeService _timeService;

    public VirtualPetApp(IPet pet, Dictionary<char, PetAction> petActions, IUserCommunication userCommunication, ITimeService timeService)
    {
        _pet = pet;
        _petActions = petActions;
        _userCommunication = userCommunication;
        _timeService = timeService;
    }

    public async Task StartApp()
    {
        bool running = true;
        PetAction userChoice;
        _userCommunication.ShowMessage("Welcome to the Virtual Pet Simulator");
        _userCommunication.ShowMessage("Press any key to begin");
        _userCommunication.WaitForUser();
        var timer = _timeService.StartTimer(x => RunPetUpdate());

        while (running)
        {
            _userCommunication.RenderScreen(_pet);
            userChoice = GetUserChoice();

            IPetAction? petAction = null;

            switch (userChoice)
            {
                case PetAction.Sleep:
                    petAction = new SleepAction(_pet, new Validator(), _userCommunication, _timeService);
                    break;
                case PetAction.Eat:
                    petAction = new EatAction(_pet, new Validator(), _userCommunication, _timeService);
                    break;
                case PetAction.Play:
                    petAction = new PlayAction(_pet, new Validator(), _userCommunication, _timeService);
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

    private void RunPetUpdate()
    {
        PetUpdaterService.UpdatePetAttributes(_pet);
        _userCommunication.RenderAttributes(_pet);
    }

    private PetAction GetUserChoice()
    {
        char userChoice;
        do
        {
            _userCommunication.RenderScreen(_pet);
            userChoice = _userCommunication.GetUserChoice("Choose an option: ");
        }
        while (!_petActions.ContainsKey(userChoice));

        return _petActions[userChoice];
    }
}