using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;

namespace VirtualPetSimulator.Models;

public class CatPet : Pet
{
    public CatPet(string name, ISoundAction soundBehaviour, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
        : base(name, soundBehaviour, energy, hunger, happiness)
    { }
}
