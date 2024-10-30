using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Validators.Interfaces;

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

        if (!_validator.Validate(_sleepSpecified, nameof(_sleepSpecified)))
        {
            return amountSlept;
        }

        var tokenSource = new CancellationTokenSource();
        var cancellationToken = tokenSource.Token;

        try
        {
            while (_pet.Energy < AttributeValue.MAX && _sleepSpecified > 0 && !cancellationToken.IsCancellationRequested)
            {
                var sleepDuration = oneSleep * AttributeValue.DEFAULT_OPERATION_LENGTH_MILLISECONDS;
                var operation = _timeService.WaitForOperation(sleepDuration, cancellationToken);

                _userCommunication.RenderScreen(_pet);
                var progress = _userCommunication.ShowProgress(operation);

                var listenForKey = new Task(() => _userCommunication.ListenForKeyStroke(tokenSource, operation));
                listenForKey.Start();

                amountSlept += oneSleep;
                _sleepSpecified--;

                await operation;
                await progress;

                _pet.ChangeEnergy(oneSleep);
            }
        }
        catch (TaskCanceledException)
        {
            _userCommunication.SetDisplayMessage($"{_pet.Name}'s nap has been rudely interupted... good luck.");
            _userCommunication.RenderScreen(_pet);
            await _timeService.WaitForOperation(2000);
        }
        finally
        {
            _userCommunication.SetDisplayMessageToOptions();
        }

        return amountSlept;
    }

}