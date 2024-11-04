using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Models.Enums;

namespace VirtualPetSimulator.Configuration
{
    public class KeyStrokeMappings
    {
        public Dictionary<char, PetAction> PetActions { get; private set; }
        public Dictionary<char, PetType> PetTypes { get; private set; }
        public Dictionary<char, EatOption> EatOptions { get; private set; }

        public KeyStrokeMappings()
        {
            PetActions = new Dictionary<char, PetAction> {
                { 'S', PetAction.Sleep },
                { 'E', PetAction.Eat },
                { 'P', PetAction.Play },
                { 'X', PetAction.Exit }
            };
            PetTypes = new Dictionary<char, PetType> {
                { 'C', PetType.Cat },
                { 'B', PetType.Bear },
            };
            EatOptions = new Dictionary<char, EatOption> {
                { 'M', EatOption.Meal },
                { 'S', EatOption.Snack },
            };
        }
    }
}
