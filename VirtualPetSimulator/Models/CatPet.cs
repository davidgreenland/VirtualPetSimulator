using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services;

namespace VirtualPetSimulator.Models;

public class CatPet : Pet
{
    public CatPet(string name, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
        : base(name, energy, hunger, happiness)
    { }
}
