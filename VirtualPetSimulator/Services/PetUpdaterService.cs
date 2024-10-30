using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Services;

public class PetUpdaterService
{
    public static void UpdatePetAttributes(IPet pet)
    {
        if (pet.CurrentAction != PetAction.Sleep)
        {
            pet.ChangeEnergy(-1);
        }
        if (pet.Energy < AttributeValue.HAPPINESS_THRESHOLD)
        {
            pet.ChangeHappiness(-1);
        }
    }
}
