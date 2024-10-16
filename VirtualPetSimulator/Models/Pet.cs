using VirtualPetSimulator.Helpers;

namespace VirtualPetSimulator.Models;

public abstract class Pet
{
    public string Name { get; }

    public bool HasEatenSinceSleeping { get; private set; } = false;

    private int _hunger;
    public int Hunger
    {
        get => _hunger;
        private set
        {
            _hunger = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
        }
    }

    private int _energy;
    public int Energy
    {
        get => _energy;
        private set
        {
            _energy = Math.Clamp(value, AttributeValue.MIN, AttributeValue.MAX);
        }
    }

    public Pet(string name, int energy = 10, int hunger = 0)
    {
        Name = name;
        Energy = energy;
        Hunger = hunger;
    }

    public void Eat(int foodAmount = 1)
    {
        if (Hunger == AttributeValue.MIN) return;

        Hunger -= foodAmount;

        if (!HasEatenSinceSleeping)
        {
            Energy += 1;
        }
    }
}
