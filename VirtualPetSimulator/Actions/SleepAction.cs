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
    private readonly PetAction sleepAction = PetAction.Sleep;
    private int _sleepSpecified = AttributeValue.MAX;

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
        _pet.CurrentAction = sleepAction;
        var sleepMessage = $"{_pet.Name} is napping";
        var oneSleep = 1;
        int amountSlept = 0;

        if (!_validator.IsNonNegative(_sleepSpecified, nameof(_sleepSpecified)))
        {
            return amountSlept;
        }

        while (_pet.Energy < AttributeValue.MAX && _sleepSpecified > 0)
        {
            var operation = _userCommunication.RunOperation(oneSleep, sleepMessage, _pet.GetAsciiArt());
            var progress = _userCommunication.ShowProgress(operation);

            _pet.ChangeEnergy(oneSleep);
            amountSlept += oneSleep;
            _sleepSpecified--;

            await operation;
            await progress;
        }

        _userCommunication.ActivityMessage = "";
        return amountSlept;
    }
}