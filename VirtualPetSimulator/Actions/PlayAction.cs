using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Interfaces;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Actions;

public class PlayAction : IPetAction
{
    private readonly IPet _pet;
    private readonly IValidator _validator;
    private readonly IUserCommunication _userCommunication;
    public int PlayAmountRequest { get; }

    public PlayAction(IPet pet, IValidator validator, IUserCommunication userCommunication, int playAmountRequest = 1)
    {
        _pet = pet;
        _validator = validator;
        _userCommunication = userCommunication;
        PlayAmountRequest = playAmountRequest;
    }

    public async Task<int> Execute()
    {
        int playAmount;
        if (_validator.IsNonNegative(PlayAmountRequest, nameof(PlayAmountRequest)) || _pet.Happiness <= AttributeValue.HAPPINESS_PLAY_THRESHOLD)
        {
            playAmount = 0;
            return playAmount;
        }

        var playMessage = $"{_pet.Name} is having a good play";
        var playingOperation = _userCommunication.RunOperation(PlayAmountRequest, playMessage);

        _pet.ChangeHappiness(PlayAmountRequest);
        playAmount = PlayAmountRequest;
        await playingOperation;
        // todo: play reduces energy

        return playAmount;
    }
}