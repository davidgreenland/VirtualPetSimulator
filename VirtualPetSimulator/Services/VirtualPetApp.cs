using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class VirtualPetApp
{
    public IPet? Pet { get; private set; }
    private readonly Dictionary<char, PetAction> _petActions;
    private readonly Dictionary<char, PetType> _petTypes;
    private readonly IUserCommunication _userCommunication;
    private readonly ITimeService _timeService;

    public VirtualPetApp(Dictionary<char, PetAction> petActions, Dictionary<char, PetType> petTypes, IUserCommunication userCommunication, ITimeService timeService)
    {
        _petActions = petActions;
        _petTypes = petTypes;
        _userCommunication = userCommunication;
        _timeService = timeService;
    }

    public PetType ChoosePet()
    {
        _userCommunication.ShowMessage("Welcome to the Virtual Pet Simulator\n");
        _userCommunication.ShowMessage(_userCommunication.GetPetChoices());
        var petChoice = GetPetType();

        return petChoice;
    }

    public string ChooseName()
    {
        return _userCommunication.ReadInput("What is the name of your pet? ");
    }

    public void SetPet(IPet pet)
    {
        Pet = pet;
    }

    public async Task Run()
    {
        bool running = true;
        PetAction userChoice;

        var timer = _timeService.StartTimer(x => RunPetUpdate());

        while (running)
        {
            _userCommunication.RenderScreen(Pet);
            userChoice = GetUserChoice();

            IPetAction? petAction = null;

            switch (userChoice)
            {
                case PetAction.Sleep:
                    petAction = new SleepAction(Pet, new Validator(), _userCommunication, _timeService);
                    break;
                case PetAction.Eat:
                    petAction = new EatAction(Pet, new Validator(), _userCommunication, _timeService);
                    break;
                case PetAction.Play:
                    petAction = new PlayAction(Pet, new Validator(), _userCommunication, _timeService);
                    break;
                default:
                    Pet.CurrentAction = PetAction.Sit;
                    break;
            }

            if (petAction != null)
            {
                await petAction.Execute();
                Pet.CurrentAction = PetAction.Sit;
            }
        }
    }

    private void RunPetUpdate()
    {
        PetUpdaterService.UpdatePetAttributes(Pet);
        _userCommunication.RenderAttributes(Pet);
        _userCommunication.DisplaySound(Pet);
    }

    private PetType GetPetType()
    {
        char userChoice;
        do
        {
            userChoice = _userCommunication.GetUserChoice("Choose your pet: ");
        }
        while (!_petTypes.ContainsKey(userChoice));

        return _petTypes[userChoice];
    }

    private PetAction GetUserChoice()
    {
        char userChoice;
        do
        {
            _userCommunication.RenderScreen(Pet);
            userChoice = _userCommunication.GetUserChoice("Choose an option: ");
        }
        while (!_petActions.ContainsKey(userChoice));

        return _petActions[userChoice];
    }
}