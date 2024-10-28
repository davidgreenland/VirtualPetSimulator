using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Helpers.Interfaces;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

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
        int playAmount;
        if (!_validator.IsNonNegative(PlayAmountRequest, nameof(PlayAmountRequest)) || _pet.Happiness <= AttributeValue.HAPPINESS_PLAY_THRESHOLD)
        {
            playAmount = 0;
            return playAmount;
        }

        _pet.CurrentAction = playAction;
        _userCommunication.SetDisplayMessage($"{_pet.Name} is having a good play");
        playAmount = PlayAmountRequest;
        var playDuration = PlayAmountRequest * AttributeValue.OPERATION_LENGTH_MILLISECONDS;
        var playingOperation = _timeService.WaitForOperation(playDuration);

        _userCommunication.RenderScreen(_pet);
        var progress = _userCommunication.ShowProgress(playingOperation);

        await Task.WhenAll(playingOperation, progress);

        // todo: play reduces energy
        _pet.ChangeHappiness(PlayAmountRequest);
        _userCommunication.SetDisplayMessageToOptions();
        return playAmount;
    }
}