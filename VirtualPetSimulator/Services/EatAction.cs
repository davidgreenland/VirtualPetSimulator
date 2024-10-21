﻿using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Interfaces;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class EatAction : IPetAction
{
    private readonly IPet _pet;
    private readonly IValidator _validator;
    private readonly IUserCommunication _userCommunication;
    public int FoodAmount { get; }

    public EatAction(IPet pet, IValidator validator, IUserCommunication userCommunication, int foodAmount = 1 )
    {
        _pet = pet;
        _validator = validator;
        _userCommunication = userCommunication;
        FoodAmount = foodAmount;
    }

    public async Task<int> Execute()
    {
        int portionsEaten;
        if (_validator.IsNonNegative(FoodAmount, nameof(FoodAmount)) || _pet.Hunger == AttributeValue.MIN)
        {
            portionsEaten = 0;
            return portionsEaten;
        }

        portionsEaten = Math.Min(FoodAmount, _pet.Hunger);
        var eatMessage = $"{_pet.Name} enjoying his food";
        var eatingOperation = _userCommunication.RunOperation(portionsEaten, eatMessage);

        _pet.ChangeHunger(-portionsEaten);
        await eatingOperation;

        return portionsEaten;
    }
}