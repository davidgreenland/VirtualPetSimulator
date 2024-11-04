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
        var petOptions = _userCommunication.GetOptions(typeof(PetType));
        _userCommunication.ShowMessage("Welcome to the Virtual Pet Simulator\n\nThese wonderful animals are available\n\n");
        _userCommunication.ShowMessage(petOptions +"\nChoose your pet : ");
        var petChoice = SelectOptionByKey(_keyStrokeMappings.PetTypes);
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

        var timer = _timeService.StartTimer(x => RunPetUpdate(pet));

        while (running)
        {
            _userCommunication.SetDisplayMessageToOptions();
            _userCommunication.RenderScreen(pet);

            userChoice = SelectOptionByKey(_keyStrokeMappings.PetActions);
            var actionValue = await GetAmountAsync(pet, userChoice);

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

    private async Task<int> GetAmountAsync(IPet pet, PetAction userChoice)
    {
        switch (userChoice)
        {
            case PetAction.Sleep:
                return AttributeValue.MAX;
            case PetAction.Eat:
                _userCommunication.ShowMessage(_userCommunication.GetOptions(typeof(EatOption)) + $"What are you going to give {pet.Name}: ");
                var mealChoice = await Task.Run(() => SelectOptionByKey(_keyStrokeMappings.EatOptions));

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
            _userCommunication.RenderAttributes(pet);

            if (pet.CurrentMood != mood)
            {
                _userCommunication.RenderScreen(pet);
            }

            //_userCommunication.DisplaySoundAsync(pet);
        }
    }

    private T SelectOptionByKey<T>(Dictionary<char, T> options) where T : Enum
    {
        char userChoice;
        do
        {
            userChoice = _userCommunication.GetUserChoice();
        }
        while (!options.ContainsKey(userChoice));
        Console.Write(options[userChoice] + "\n");
        return options[userChoice];
    }
}