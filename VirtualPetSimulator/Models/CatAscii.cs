using VirtualPetSimulator.Helpers.Enumerations;

namespace VirtualPetSimulator.Models;

public static class CatAscii
{
    public static IDictionary<PetActions, string> Images { get; }

    static CatAscii()
    {
        Images = new Dictionary<PetActions, string>() {
            { PetActions.Play, " _._     _,-'\"\"`-._\r\n(,-.`._,'(       |\\`-/|\r\n    `-.-' \\ )-`( , o o)\r\n          `-    \\`_`\"'-" },
            { PetActions.Eat, "        |\\=/|.-\"\"\"-.  \r\n        /6 6\\       \\\r\n       =\\_Y_/=  (_  ;\\\r\n  jgs    _U//_/-/__///\r\n        /kit\\      ((\r\n        ^^^^^       `" },
            { PetActions.Sleep, "      |\\      _,,,---,,_\r\nZZZzz /,`.-'`'    -.  ;-;;,_\r\n     |,4-  ) )-,_. ,\\ (  `'-'\r\n    '---''(_/--'  `-'\\_)" },
            { PetActions.Sit, " /\\_/\\\r\n( o.o ) \r\n > ^ < )\r\n /   \\/ \r\n(\\|||/)"}
        };
    }
}
