using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Models;

public abstract class Pet : IPet
{
    public string Name { get; }

    private int _energy;
    public int Energy
    {
        get => _energy;
        set => _energy = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
    }

    private int _hunger;
    public int Hunger
    {
        get => _hunger;
        set => _hunger = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
    }

    private int _happiness;
    public int Happiness
    {
        get => _happiness;
        set => _happiness = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
    }

    public Pet( string name, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
    {
        Name = name;
        Energy = energy;
        Hunger = hunger;
        Happiness = happiness;
    }

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
}
