using VirtualPetSimulator.Helpers;
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
        get => _hunger;
        private set
        {
            _hunger = Math.Min(value, AttributeValue.MAX);
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

    public async Task Eat(int foodAmount = 1)
    {
        if (Hunger == AttributeValue.MIN)
        {
            return;
        }

        Hunger -= foodAmount;
        await _timeService.Delay(foodAmount * 1000);
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

    public void Play()
    {
        Happiness += 1;
    }
}
