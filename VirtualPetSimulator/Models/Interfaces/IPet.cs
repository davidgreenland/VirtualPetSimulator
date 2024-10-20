namespace VirtualPetSimulator.Models.Interfaces;

public interface IPet
{
    string Name { get; }
    int Energy { get; set; }
    int Happiness { get; set; }
    int Hunger { get; set; }
    void ChangeHunger(int value);
}