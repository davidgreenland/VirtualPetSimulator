namespace VirtualPetSimulator.Models;

public abstract class Pet
{
    public string Name { get; }
    public int Energy { get; set; }
    public int Hunger { get; set; }

    public Pet(string name, int energy = 10, int hunger = 0)
    {
        Name = name;
        Energy = energy;
        Hunger = hunger;
    }

    public void Eat()
    {
        if (Hunger > 0)
        {
            --Hunger;
        }
    }
}
