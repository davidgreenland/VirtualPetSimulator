using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Models.Enums;

namespace VirtualPetSimulator.Models.Interfaces;

public interface IPet
{
    string Name { get; }
    PetAction CurrentAction { get; set; }
    PetMood CurrentMood { get; set; }
    int Energy { get; }
    int Happiness { get; }
    int Hunger { get; }
    PetMood CheckMood();
    void ChangeHunger(int value);
    void ChangeEnergy(int value);
    void ChangeHappiness(int value);
    string PerformSound();
}