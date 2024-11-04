using VirtualPetSimulator.Helpers;

namespace VirtualPetSimulator.Models;

public class CatPet : Pet
{
    public CatPet(string name, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
        : base(name, CatAscii.Images, energy, hunger, happiness)
    { }
}
