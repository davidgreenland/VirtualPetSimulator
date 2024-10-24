using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class CatAsciiArtService : IAsciiArtService
{
    public IDictionary<PetAction, string> _actionImages { get; } = new Dictionary<PetAction, string>() {
            { PetAction.Play, " _._     _,-'\"\"`-._\r\n(,-.`._,'(       |\\`-/|\r\n    `-.-' \\ )-`( , o o)\r\n          `-    \\`_`\"'-" },
            { PetAction.Eat, "        |\\=/|.-\"\"\"-.  \r\n        /6 6\\       \\\r\n       =\\_Y_/=  (_  ;\\\r\n  jgs    _U//_/-/__///\r\n        /kit\\      ((\r\n        ^^^^^       `" },
            { PetAction.Sleep, "      |\\      _,,,---,,_\r\nZZZzz /,`.-'`'    -.  ;-;;,_\r\n     |,4-  ) )-,_. ,\\ (  `'-'\r\n    '---''(_/--'  `-'\\_)" },
            { PetAction.Sit, " /\\_/\\\r\n( o.o ) \r\n > ^ < )\r\n /   \\/ \r\n(\\|||/)"}
    };

    public string GetAsciiForAction(PetAction action)
    {
        if (!_actionImages.ContainsKey(action))
        {
            return "Image unavailable\n";
        }
        return _actionImages[action];
    }
}
