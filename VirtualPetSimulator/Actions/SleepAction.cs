using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Helpers.Interfaces;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Actions;

public class SleepAction : IPetAction
{
    private readonly IPet _pet;
    private readonly IValidator _validator;
    private readonly IUserCommunication _userCommunication;
    private readonly ITimeService _timeService;
    private readonly PetAction sleepAction = PetAction.Sleep;
    private int _sleepSpecified = AttributeValue.MAX;

    public SleepAction(IPet pet, IValidator validator, IUserCommunication userCommunication, ITimeService timeService, int sleepSpecified) : this(pet, validator, userCommunication, timeService)
    {
        _sleepSpecified = sleepSpecified;
    }

    public SleepAction(IPet pet, IValidator validator, IUserCommunication userCommunication, ITimeService timeService)
    {
        _pet = pet;
        _validator = validator;
        _userCommunication = userCommunication;
        _timeService = timeService;
    }

    public async Task<int> Execute()
    {
        _pet.CurrentAction = sleepAction;
        _userCommunication.SetDisplayMessage($"{_pet.Name} is napping");
        var oneSleep = 1;
        int amountSlept = 0;

        if (!_validator.IsNonNegative(_sleepSpecified, nameof(_sleepSpecified)))
        {
            return amountSlept;
        }

        while (_pet.Energy < AttributeValue.MAX && _sleepSpecified > 0)
        {
            var sleepDuration = oneSleep * AttributeValue.OPERATION_LENGTH_MILLISECONDS;
            var operation = _timeService.WaitForOperation(sleepDuration);

            _userCommunication.RenderScreen(_pet);
            var progress = _userCommunication.ShowProgress(operation);
            amountSlept += oneSleep;
            _sleepSpecified--;

            await operation;
            await progress;
            _pet.ChangeEnergy(oneSleep);
        }

        _userCommunication.SetDisplayMessageToOptions();
        return amountSlept;
    }
}