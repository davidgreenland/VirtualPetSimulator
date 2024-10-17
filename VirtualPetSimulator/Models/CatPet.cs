using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Models;

public class CatPet : Pet
{
    public CatPet(ITimeService timeService, string name, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
        : base(timeService, name, energy, hunger, happiness)
    { }
}
