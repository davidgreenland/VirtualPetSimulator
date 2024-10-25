using VirtualPetSimulator.Helpers.Enumerations;

namespace VirtualPetSimulator.Models.Interfaces;

public interface IPet
{
    string Name { get; }
    PetAction CurrentAction { get; set; }
    int Energy { get; }
    int Happiness { get; }
    int Hunger { get; }

    void ChangeHunger(int value);
    void ChangeEnergy(int value);
    void ChangeHappiness(int value);
}