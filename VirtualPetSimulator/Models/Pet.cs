using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Models;

public abstract class Pet
{
    private readonly IOperationService _timeService;
    private readonly IValidator _validator;
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

    public Pet(IOperationService timeService, IValidator validator, string name, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
    {
        _timeService = timeService;
        _validator = validator;
        Name = name;
        Energy = energy;
        Hunger = hunger;
        Happiness = happiness;
    }

    public async Task<int> Eat(int foodAmount = 1)
    {
        int portionsEaten;
        if (_validator.IsNonNegative(foodAmount, nameof(foodAmount)) || Hunger == AttributeValue.MIN)
        {
            portionsEaten = 0;
            return portionsEaten;
        }

        portionsEaten = Math.Min(foodAmount, Hunger);
        var eatMessage = $"{Name} enjoying his food";
        var eatingOperation = _timeService.RunOperation(portionsEaten, eatMessage);

        Hunger -= portionsEaten;
        await eatingOperation;

        return portionsEaten;
    }

    public async Task<int> Sleep(int sleepValue = 1)
    {
        int amountSlept;
        if (_validator.IsNonNegative(sleepValue, nameof(sleepValue)) || Energy == AttributeValue.MAX)
        {
            amountSlept = 0;
            return amountSlept;
        }

        amountSlept = Math.Min(sleepValue, AttributeValue.MAX - Energy);
        var sleepMessage = $"{Name} is napping";
        var sleepOperation = _timeService.RunOperation(amountSlept, sleepMessage);

        Energy += sleepValue;
        await sleepOperation;

        return amountSlept;
    }

    public async Task<int> Play(int playValue = 1)
    {
        int happinessIncrease;
        if (_validator.IsNonNegative(playValue, nameof(playValue)) || Happiness <= AttributeValue.HAPPINESS_PLAY_THRESHOLD)
        {
            happinessIncrease = 0;
            return happinessIncrease;
        }

        happinessIncrease = Math.Min(playValue, AttributeValue.MAX - Happiness);
        var playMessage = $"{Name} is playing";
        var playOperation = _timeService.RunOperation(playValue, playMessage);

        Happiness += happinessIncrease;
        await playOperation;

        return happinessIncrease;
    }
}
