using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Services;

public class PetUpdaterService
{
    public static void UpdatePetAttributes(IPet pet)
    {
        pet.ChangeEnergy(-1);
    }
}
