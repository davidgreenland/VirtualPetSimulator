using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Factories.Interfaces;
using VirtualPetSimulator.Models.Enums;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Configuration;
using VirtualPetSimulator.Helpers;

namespace VirtualPetSimulator.Services;

public class VirtualPetApp
{
    private readonly KeyStrokeMappings _keyStrokeMappings;
    private readonly IUserCommunication _userCommunication;
    private readonly ITimeService _timeService;
    private readonly IPetActionFactory _petActionFactory;

    public VirtualPetApp(KeyStrokeMappings keyStrokeMappings, IUserCommunication userCommunication, ITimeService timeService, IPetActionFactory petActionFactory)
    {
        _keyStrokeMappings = keyStrokeMappings;
        _userCommunication = userCommunication;
        _timeService = timeService;
        _petActionFactory = petActionFactory;
    }

    public PetType ChoosePetType()
    {
        _userCommunication.ShowMessage("Welcome to the Virtual Pet Simulator\n");
        _userCommunication.ShowMessage(_userCommunication.GetOptions(typeof(PetType)));
        var petChoice = SelectOptionByKey(_keyStrokeMappings.PetTypes, "Choose your pet: ");

        return petChoice;
    }

    public string ChooseName()
    {
        return _userCommunication.ReadInput("What is the name of your pet? ");
    }

    public async Task Run(IPet pet)
    {
        bool running = true;
        PetAction userChoice;

        var timer = new Task(() => _timeService.StartTimer(x => RunPetUpdate(pet)));
        //var timer = _timeService.StartTimer(x => RunPetUpdate(pet));
        timer.Start();

        while (running)
        {
            _userCommunication.RenderScreen(pet);

            userChoice = SelectOptionByKey(_keyStrokeMappings.PetActions, "Choose an option: ");
            var actionValue = GetAmount(pet, userChoice);

            var petAction = _petActionFactory.CreatePetAction(pet, userChoice, actionValue);
            if (petAction != null)
            {
                await petAction.Execute();
                pet.CurrentAction = PetAction.Sit;
            }

            if (userChoice == PetAction.Exit)
            {
                running = false;
            }
        }
    }

    private int GetAmount(IPet pet, PetAction userChoice)
    {
        switch (userChoice)
        {
            case PetAction.Sleep:
                return AttributeValue.MAX;
            case PetAction.Eat:
                _userCommunication.ShowMessage(_userCommunication.GetOptions(typeof(EatOption)));
                var mealChoice = SelectOptionByKey(_keyStrokeMappings.EatOptions, $"What are you going to give {pet.Name}: ");
                if (mealChoice == EatOption.Meal)
                {
                    return 5;
                }
                else if (mealChoice == EatOption.Snack)
                {
                    return 1;
                }
                return 0;
            case PetAction.Play:
                return 1;
            default:
                return 0;
        }
    }

    private void RunPetUpdate(IPet pet)
    {
        if (pet.CurrentAction == PetAction.Sit)
        {
            var mood = pet.CurrentMood;

            PetUpdaterService.UpdatePetAttributes(pet);
            _userCommunication.RenderScreen(pet);

            if (pet.CurrentMood != mood)
            {
                _userCommunication.RenderScreen(pet);
            }

            //_userCommunication.DisplaySoundAsync(pet);
        }
    }

    private T SelectOptionByKey<T>(Dictionary<char, T> options, string prompt) where T : Enum
    {
        char userChoice;
        do
        {
            userChoice = _userCommunication.GetUserChoice(prompt);
        }
        while (!options.ContainsKey(userChoice));

        return options[userChoice];
    }
}