using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Interfaces;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Actions;

public class SleepAction : IPetAction
{
    private readonly IPet _pet;
    private readonly IValidator _validator;
    private readonly IUserCommunication _userCommunication;
    private int? _sleepSpecified = null;

    public SleepAction(IPet pet, IValidator validator, IUserCommunication userCommunication, int sleepSpecified) : this(pet, validator, userCommunication)
    {
        _sleepSpecified = sleepSpecified;
    }

    public SleepAction(IPet pet, IValidator validator, IUserCommunication userCommunication)
    {
        _pet = pet;
        _validator = validator;
        _userCommunication = userCommunication;
    }

    public async Task<int> Execute()
    {
        var sleepMessage = $"{_pet.Name} is napping";
        var oneSleep = 1;
        int amountSlept = 0;

        if (_sleepSpecified.HasValue && !_validator.IsNonNegative(_sleepSpecified.Value, nameof(_sleepSpecified)))
        {
            return amountSlept;
        }

        while (_pet.Energy < AttributeValue.MAX && _sleepSpecified != 0)
        {
            await _userCommunication.RunOperation(oneSleep, sleepMessage);
            _pet.ChangeEnergy(oneSleep);
            amountSlept += oneSleep;

            if (_sleepSpecified > 0)
            {
                _sleepSpecified--;
            }
        }

        return amountSlept;
    }
}