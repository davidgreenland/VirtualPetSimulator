using VirtualPetSimulator.Helpers.Enumerations;

namespace VirtualPetSimulator.Models.Interfaces;

public interface IPet
{
    string Name { get; }
    PetActions CurrentAction { get; set; }
    string GetAsciiArt();
    int Energy { get; set; }
    int Happiness { get; set; }
    int Hunger { get; set; }

    void ChangeHunger(int value);
    void ChangeEnergy(int value);
    void ChangeHappiness(int value);

}