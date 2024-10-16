using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Models;

public abstract class Pet
{
    private readonly ITimeService _timeService;
    public string Name { get; }
    public int Energy { get; private set; }

    private int _hunger;
    public int Hunger
    {
        get => _hunger;
        private set
        {
            _hunger = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
        }
    }

    public Pet(ITimeService timeService, string name, int energy = 10, int hunger = 0)
    {
        _timeService = timeService;
        Name = name;
        Energy = energy;
        Hunger = hunger;
    }

    public async Task Eat(int foodAmount = 1)
    {
        Hunger -= foodAmount;

        await _timeService.Delay(foodAmount * 1000);
    }
}
