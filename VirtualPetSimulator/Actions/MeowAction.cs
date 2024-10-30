using VirtualPetSimulator.Actions.Interfaces;

namespace VirtualPetSimulator.Actions;

public class MeowAction : ISoundAction
{
    public string MakeSound()
    {
        return "Meow!";
    }
}
