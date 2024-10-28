using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Helpers.Interfaces;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Actions;

public class EatAction : IPetAction
{
    private readonly IPet _pet;
    private readonly IValidator _validator;
    private readonly IUserCommunication _userCommunication;
    private readonly ITimeService _timeService;
    private readonly PetAction eatAction = PetAction.Eat;
    public int FoodAmount { get; }

    public EatAction(IPet pet, IValidator validator, IUserCommunication userCommunication, ITimeService timeService, int foodAmount = 1)
    {
        _pet = pet;
        _validator = validator;
        _timeService = timeService;
        _userCommunication = userCommunication;
        FoodAmount = foodAmount;
    }

    public async Task<int> Execute()
    {
        _pet.CurrentAction = eatAction;
        int portionsEaten;
        if (!_validator.IsNonNegative(FoodAmount, nameof(FoodAmount)) || _pet.Hunger == AttributeValue.MIN)
        {
            portionsEaten = 0;
            return portionsEaten;
        }

        portionsEaten = Math.Min(FoodAmount, _pet.Hunger);
        _userCommunication.SetDisplayMessage($"{_pet.Name} enjoying his food");

        var eatingDuration = portionsEaten * AttributeValue.DEFAULT_OPERATION_LENGTH_MILLISECONDS;
        var eatingOperation = _timeService.WaitForOperation(eatingDuration);
        var progress = _userCommunication.ShowProgress(eatingOperation);

        _userCommunication.RenderScreen(_pet);

        await eatingOperation;
        await progress;

        _pet.ChangeHunger(-portionsEaten);
        _userCommunication.SetDisplayMessageToOptions();
        return portionsEaten;
    }
}
