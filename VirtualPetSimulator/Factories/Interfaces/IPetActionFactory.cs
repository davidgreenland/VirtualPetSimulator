using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Factories.Interfaces
{
    public interface IPetActionFactory
    {
        IPetAction CreatePetAction(IPet pet, PetAction selectedAction, int actionValue);
    }
}