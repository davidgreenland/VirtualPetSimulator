using VirtualPetSimulator.Actions.Interfaces;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Models.Interfaces;

namespace VirtualPetSimulator.Factories.Interfaces
{
    public interface IPetActionFactory
    {
        IPetAction CreatePetAction(IPet pet, PetAction selectedAction);
    }
}