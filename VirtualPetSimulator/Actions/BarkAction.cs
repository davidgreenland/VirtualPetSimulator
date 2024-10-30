using VirtualPetSimulator.Actions.Interfaces;

namespace VirtualPetSimulator.Actions;

public class BarkAction : ISoundAction
{
    public string MakeSound()
    {
        return "Woof, woof!";
    }
}
