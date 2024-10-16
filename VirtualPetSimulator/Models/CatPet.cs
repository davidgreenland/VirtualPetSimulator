using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Models;

public class CatPet : Pet
{
    public CatPet(ITimeService timeService, string name, int energy, int hunger) : base(timeService, name, energy, hunger)
    { }
}
