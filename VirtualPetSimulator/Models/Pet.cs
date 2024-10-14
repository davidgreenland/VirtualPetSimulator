namespace VirtualPetSimulator.Models;

public abstract class Pet
{
    public string Name { get; }
    public int Happiness { get; set; }
    public int Energy { get; set; }
    public int Bladder { get; set; } = 1;
    public int Hunger { get; set; } = 1;
    public int Thirst { get; set; } = 1;

    public Pet(string name, int happiness = 10, int energy = 10)
    {
        Name = name;
        Happiness = happiness;
        Energy = energy;
    }

    public void Eat(int foodValue)
    {
    }

    public void Play()
    {
    }

    public void Sleep()
    {
    }

    public void Wee()
    {

    }
}
