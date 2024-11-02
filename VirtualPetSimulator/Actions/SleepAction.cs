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
    private int _sleepValue;

    public SleepAction(IPet pet, IValidator validator, IUserCommunication userCommunication, ITimeService timeService, int sleepValue)
    {
        _pet = pet;
        _validator = validator;
        _userCommunication = userCommunication;
        _timeService = timeService;
        _sleepValue = sleepValue;
    }

    public async Task<int> Execute()
    {
        _pet.CurrentAction = sleepAction;
        _userCommunication.SetDisplayMessage($"{_pet.Name} is napping");
        var oneSleep = 1;
        int amountSlept = 0;

        if (!_validator.Validate(_sleepValue, nameof(_sleepValue)))
        {
            return amountSlept;
        }

        var tokenSource = new CancellationTokenSource();
        var cancellationToken = tokenSource.Token;

        try
        {
            while (_pet.Energy < AttributeValue.MAX && _sleepValue > 0 && !cancellationToken.IsCancellationRequested)
            {
                var sleepDuration = oneSleep * AttributeValue.DEFAULT_OPERATION_LENGTH_MILLISECONDS;
                var operation = _timeService.WaitForOperation(sleepDuration, cancellationToken);

                _userCommunication.RenderScreen(_pet);
                var progress = _userCommunication.ShowProgressAsync(operation);

                var listenForKey = new Task(() => _userCommunication.ListenForKeyStroke(tokenSource, operation));
                listenForKey.Start();

                amountSlept += oneSleep;
                _sleepValue--;

                await operation;
                await progress;

                _pet.ChangeEnergy(oneSleep);
            }
        }
        catch (TaskCanceledException)
        {
            _pet.ChangeHappiness(-4);
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