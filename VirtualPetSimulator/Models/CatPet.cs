using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Interfaces;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Models;

public class CatPet : Pet
{
    public CatPet(IOperationService petOperationService, IValidator validator, string name, int energy = AttributeValue.DEFAULT, int hunger = AttributeValue.DEFAULT, int happiness = AttributeValue.DEFAULT)
        : base(petOperationService, validator, name, energy, hunger, happiness)
    { }
}
