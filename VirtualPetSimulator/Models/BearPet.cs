using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers;

namespace VirtualPetSimulator.Models;

public class BearPet : Pet
{
    public BearPet(string name, ISoundAction soundBehaviour, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
        : base(name, soundBehaviour, energy, hunger, happiness)
    { }
}

