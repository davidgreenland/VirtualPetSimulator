using VirtualPetSimulator.Models.Interfaces;
using System.ComponentModel;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Factories.Interfaces;
using VirtualPetSimulator.Validators.Interfaces;
using VirtualPetSimulator.Actions.Enums;

namespace VirtualPetSimulator.Factories;

public class PetActionFactory : IPetActionFactory
{
    private readonly IValidator _validator;
    private readonly IUserCommunication _userCommunication;
    private readonly ITimeService _timeService;

    public PetActionFactory(IValidator validator, IUserCommunication userCommunication, ITimeService timeService)
    {
        _validator = validator;
        _userCommunication = userCommunication;
        _timeService = timeService;
    }

    public IPetAction CreatePetAction(IPet pet, PetAction selectedAction, ITimer appTimer)
    {

        switch (selectedAction)
        {
            case PetAction.Sleep:
                return new SleepAction(pet, _validator, _userCommunication, _timeService);
            case PetAction.Eat:
                return new EatAction(pet, _validator, _userCommunication, _timeService);
            case PetAction.Play:
                return new PlayAction(pet, _validator, _userCommunication, _timeService);
            case PetAction.Exit:
                return new ExitAction(_userCommunication, appTimer);
            default:
                throw new InvalidEnumArgumentException($"PetAction {selectedAction} is not implemented yet");
        }
    }
}
