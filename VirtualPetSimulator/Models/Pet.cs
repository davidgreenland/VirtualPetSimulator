﻿using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Models;

public abstract class Pet
{
    private readonly ITimeService _timeService;
    public string Name { get; }

    private int _energy;
    public int Energy
    {
        get => _energy;
        private set
        {
            _energy = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
        }
    }

    private int _hunger;
    public int Hunger
    {
        get => _hunger;
        private set
        {
            _hunger = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
        }
    }

    private int _happiness;
    public int Happiness
    {
        get => _happiness;
        private set
        {
            _happiness = Math.Min(value, AttributeValue.MAX);
        }
    }

    public Pet(ITimeService timeService, string name, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
    {
        _timeService = timeService;
        Name = name;
        Energy = energy;
        Hunger = hunger;
        Happiness = happiness;
    }

    public async Task<int> Eat(int foodAmount = 1)
    {
        int portionsEaten;
        if (Hunger == AttributeValue.MIN)
        {
            portionsEaten = 0;
            return portionsEaten;
        }

        var eatingOperation = _timeService.Delay(foodAmount * 1000);

        portionsEaten = Math.Min(foodAmount, Hunger);
        Hunger -= foodAmount;
        await eatingOperation;

        return portionsEaten;
    }

    public async Task Sleep(int sleepValue = 1)
    {
        if (Energy == AttributeValue.MAX)
        {
            return;
        }

        Energy += sleepValue;
        await _timeService.Delay(sleepValue * 1000);
    }

    public void Play(int playValue = 1)
    {
        Happiness += playValue;
    }
}
