using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Models;

public abstract class Pet : IPet
{
    public string Name { get; }
    protected ISoundAction _soundBehaviour;
    public PetAction CurrentAction { get; set; } = PetAction.Sit;

    private int _energy;
    public int Energy
    {
        get => _energy;
        private set => _energy = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
    }

    private int _hunger;
    public int Hunger
    {
        get => _hunger;
        private set => _hunger = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
    }

    private int _happiness;
    public int Happiness
    {
        get => _happiness;
        private set => _happiness = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
    }

    public Pet(string name, ISoundAction soundBehaviour, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
    {
        Name = name;
        _soundBehaviour = soundBehaviour;
        Energy = energy;
        Hunger = hunger;
        Happiness = happiness;
    }

    public bool IsGrumpy() => _energy < AttributeValue.HAPPINESS_THRESHOLD || _happiness < AttributeValue.HAPPINESS_THRESHOLD || _hunger > AttributeValue.HUNGRY;

    public void ChangeEnergy(int value)
    {
        Energy += value;
    }

    public void ChangeHunger(int value)
    {
        Hunger += value;
    }

    public void ChangeHappiness(int value)
    {
        Happiness += value;
    }

    public string PerformSound() => _soundBehaviour.MakeSound();
}
