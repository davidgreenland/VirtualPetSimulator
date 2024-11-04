using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;
using VirtualPetSimulator.Validators.Interfaces;

namespace VirtualPetSimulator.Actions;

public class PlayAction : IPetAction
{
    private readonly IPet _pet;
    private readonly IValidator _validator;
    private readonly IUserCommunication _userCommunication;
    private readonly ITimeService _timeService;
    private readonly PetAction playAction = PetAction.Play;

    public int PlayAmountRequest { get; }

    public PlayAction(IPet pet, IValidator validator, IUserCommunication userCommunication, ITimeService timeService, int playAmountRequest = 1)
    {
        _pet = pet;
        _validator = validator;
        _userCommunication = userCommunication;
        _timeService = timeService;
        PlayAmountRequest = playAmountRequest;
    }

    public async Task<int> Execute()
    {
        int playAmount = 0;
        if (!_validator.Validate(PlayAmountRequest, nameof(PlayAmountRequest)))
        {
            return playAmount;
        }

        if (_pet.Happiness <= AttributeValue.HAPPINESS_THRESHOLD)
        {
            var token = new CancellationTokenSource().Token;
            await DisplayTooGrumpyToPlay(token);
            return playAmount;
        }

        _pet.CurrentAction = playAction;
        _userCommunication.SetDisplayMessage($"{_pet.Name} is having a good play");
        playAmount = PlayAmountRequest;

        var tokenSource = new CancellationTokenSource();
        var playDuration = PlayAmountRequest * AttributeValue.DEFAULT_OPERATION_LENGTH_MILLISECONDS;
        var playingOperation = _timeService.WaitForOperation(playDuration, tokenSource.Token);

        _userCommunication.RenderScreen(_pet);
        var progress = _userCommunication.ShowProgressAsync(playingOperation);

        await Task.WhenAll(playingOperation, progress);

        // todo: play reduces energy
        _pet.ChangeHappiness(PlayAmountRequest);
        _userCommunication.SetDisplayMessageToOptions();
        return playAmount;
    }

    private async Task DisplayTooGrumpyToPlay(CancellationToken token)
    {
        _userCommunication.SetDisplayMessage($"{_pet.Name} is too grumpy to play");
        _userCommunication.RenderScreen(_pet);
        await _timeService.WaitForOperation(2000, token);
        _userCommunication.SetDisplayMessageToOptions();
        _userCommunication.RenderScreen(_pet);
    }
}