using VirtualPetSimulator.Helpers.Enumerations;

namespace VirtualPetSimulator.Models.Interfaces;

public interface IPet
{
    string Name { get; }
    PetAction CurrentAction { get; set; }
    int Energy { get; set; }
    int Happiness { get; set; }
    int Hunger { get; set; }

    void ChangeHunger(int value);
    void ChangeEnergy(int value);
    void ChangeHappiness(int value);
}