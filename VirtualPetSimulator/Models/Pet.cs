namespace VirtualPetSimulator.Models;

public abstract class Pet
{
    public string Name { get; }
    public int Energy { get; set; }

    private int _hunger;
    public int Hunger
    {
        get => _hunger;
        set
        {
            _hunger = Math.Clamp(value, min: 0, max: 10);
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
        Hunger -= foodAmount;
    }
}
