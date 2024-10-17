using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Models;

public class CatPet : Pet
{
    public CatPet(ITimeService timeService, string name) : base(timeService, name)
    { }    
    
    public CatPet(ITimeService timeService, string name, int energy) : base(timeService, name, energy)
    { }

    public CatPet(ITimeService timeService, string name, int energy, int hunger) : base(timeService, name, energy, hunger)
    { }

    public CatPet(ITimeService timeService, string name, int energy, int hunger, int happiness) : base(timeService, name, energy, hunger, happiness)
    { }
}
