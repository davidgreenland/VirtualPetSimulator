using VirtualPetSimulator.Actions.Interfaces;

namespace VirtualPetSimulator.Actions.SoundBehaviours;

public class Bark : ISoundBehaviour
{
    public string MakeSound()
    {
        return "Woof, woof!";
    }
}