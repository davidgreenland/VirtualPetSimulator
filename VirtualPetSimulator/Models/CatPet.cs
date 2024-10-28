using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services;

namespace VirtualPetSimulator.Models;

public class CatPet : Pet
{
    public CatPet(string name, ISoundBehaviour soundBehaviour, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
        : base(name, soundBehaviour, energy, hunger, happiness)
    { }
}
