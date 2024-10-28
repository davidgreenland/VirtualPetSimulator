using VirtualPetSimulator.Actions.Interfaces;

namespace VirtualPetSimulator.Actions.SoundBehaviours;

public class Meow: ISoundBehaviour
{
    public string MakeSound()
    {
        return "Meow!";
    }
}
