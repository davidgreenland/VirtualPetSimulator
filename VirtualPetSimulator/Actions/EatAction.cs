using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Validators.Interfaces;

namespace VirtualPetSimulator.Actions;

public class EatAction : IPetAction
{
    private readonly IPet _pet;
    private readonly IValidator _validator;
    private readonly IUserCommunication _userCommunication;
    private readonly ITimeService _timeService;
    private readonly PetAction eatAction = PetAction.Eat;
    private int _foodAmount;

    public EatAction(IPet pet, IValidator validator, IUserCommunication userCommunication, ITimeService timeService, int foodAmount = 1)
    {
        _pet = pet;
        _validator = validator;
        _timeService = timeService;
        _userCommunication = userCommunication;
        _foodAmount = foodAmount;
    }

    public async Task<int> Execute()
    {
        _pet.CurrentAction = eatAction;
        var onePortion = 1;
        int portionsEaten = 0;
        if (!_validator.Validate(_foodAmount, nameof(_foodAmount)) || _pet.Hunger == AttributeValue.MIN)
        {
            return portionsEaten;
        }

        _userCommunication.SetDisplayMessage($"{_pet.Name} enjoying his food");
        var tokenSource = new CancellationTokenSource();
        var cancellationToken = tokenSource.Token;

        try
        {
            while (_pet.Hunger > AttributeValue.MIN && _foodAmount > 0 && !cancellationToken.IsCancellationRequested)
            {
                var eatDuration = onePortion * AttributeValue.DEFAULT_OPERATION_LENGTH_MILLISECONDS;
                var operation = _timeService.WaitForOperation(eatDuration, cancellationToken);

                _userCommunication.RenderScreen(_pet);
                var progress = _userCommunication.ShowProgressAsync(operation);

                var listenForKey = new Task(() => _userCommunication.ListenForKeyStroke(tokenSource, operation));
                listenForKey.Start();

                portionsEaten++;
                _foodAmount--;

                await operation;
                await progress;

                _pet.ChangeHunger(-onePortion);
            }
        }
        catch (TaskCanceledException)
        {
            _pet.ChangeHappiness(-3);
            _userCommunication.SetDisplayMessage($"{_pet.Name}'s meal has been cruelly snatched away - Shame on you.");
            _userCommunication.RenderScreen(_pet);
            await _timeService.WaitForOperation(2000);
        }
        finally
        {
            _userCommunication.SetDisplayMessageToOptions();
            Console.WriteLine("Hello");
        }

        return portionsEaten;
    }
}
