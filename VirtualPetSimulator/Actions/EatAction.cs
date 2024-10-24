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
    private readonly PetActions eatAction = PetActions.Eat;
    public int FoodAmount { get; }

    public EatAction(IPet pet, IValidator validator, IUserCommunication userCommunication, int foodAmount = 1)
    {
        _pet = pet;
        _validator = validator;
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
        var eatMessage = $"{_pet.Name} enjoying his food";
        var eatingOperation = _userCommunication.RunOperation(portionsEaten, eatMessage, _pet.GetAsciiArt());

        _pet.ChangeHunger(-portionsEaten);
        await _userCommunication.ShowProgress(eatingOperation);
        await eatingOperation;

        _userCommunication.ActivityMessage = "";
        return portionsEaten;
    }
}
