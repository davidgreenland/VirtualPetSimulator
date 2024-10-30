using System;
using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Models.Enums;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services;

public class CatAsciiArtService : IAsciiArtService
{
    private readonly IDictionary<PetAction, string> _actionImages = new Dictionary<PetAction, string>() {
            //{ PetAction.Play, " _._     _,-'\"\"`-._\r\n(,-.`._,'(       |\\`-/|\r\n    `-.-' \\ )-`( , o o)\r\n          `-    \\`_`\"'-" },
            { PetAction.Play, "                 ._\r\n              .-'  `-.\r\n           .-'        \\\r\n          ;    .-'\\    ;\r\n          `._.'    ;   |\r\n                   |   |\r\n                   ;   :\r\n                  ;   :\r\n                  ;   :\r\n                 /   /\r\n                ;   :                   ,\r\n                ;   |               .-\"7|\r\n              .-'\"  :            .-' .' :\r\n           .-'       \\         .'  .'   `.\r\n         .'           `-. \"\"-.-'`\"\"    `\",`-._..--\"7\r\n         ;    .          `-.J `-,    ;\"`.;|,_,    ;\r\n       _.'    |         `\"\" `. .\"\"\"--. o \\:.-. _.'\r\n    .\"\"       :            ,--`;   ,  `--/}o,' ;\r\n    ;   .___.'        /     ,--.`-. `-..7_.-  /_\r\n     \\   :   `..__.._;    .'__;    `---..__.-'-.`\"-,\r\n     .'   `--. |   \\_;    \\'   `-._.-\")     \\\\  `-,\r\n     `.   -.`_):      `.   `-\"\"\"`.   ;__.' ;/ ;   \"\r\n       `-.__7\"  `-..._.'`7     -._;'  ``\"-''\r\n                         `--.,__.'              fsc" },
            { PetAction.Eat, "        |\\=/|.-\"\"\"-.  \r\n        /6 6\\       \\\r\n       =\\_Y_/=  (_  ;\\\r\n  jgs    _U//_/-/__///\r\n        /kit\\      ((\r\n        ^^^^^       `" },
            { PetAction.Sleep, "\r\n \r\n          |\\      _,,,---,,_\r\n    ZZZzz /,`.-'`'    -.  ;-;;,_\r\n         |,4-  ) )-,_. ,\\ (  `'-'\r\n        '---''(_/--'  `-'\\_)\r\n\r\n\r\n" },
            //{ PetAction.Sit, " /\\_/\\\r\n( o.o ) \r\n > ^ < )\r\n /   \\/ \r\n(\\|||/)"}
    };
    private IDictionary<PetMood, string> _petMoodImages = new Dictionary<PetMood, string>() { 
        { PetMood.Grumpy, "                               |        |\r\n                               |\\      /|\r\n                               | \\____/ |\r\n                               |  /\\/\\  |\r\n                              .'___  ___`.\r\n                             /  \\|/  \\|/  \\\r\n            _.--------------( ____ __ _____)\r\n         .-' \\  -. | | | | | \\ ----\\/---- /\r\n       .'\\  | | / \\` | | | |  `.  -'`-  .'\r\n      /`  ` ` '/ / \\ | | | | \\  `------'\\\r\n     /-  `-------.' `-----.       -----. `---.\r\n    (  / | | | |  )/ | | | )/ | | | | | ) | | )\r\n     `._________.'_____,,,/\\_______,,,,/_,,,,/  VK" },
        { PetMood.Happy, "                _                       \r\n                \\`*-.                   \r\n                 )  _`-.                \r\n                .  : `. .               \r\n                : _   '  \\              \r\n                ; *` _.   `*-._         \r\n                `-.-'          `-.      \r\n                  ;       `       `.    \r\n                  :.       .        \\   \r\n                  . \\  .   :   .-'   .  \r\n                  '  `+.;  ;  '      :  \r\n                  :  '  |    ;       ;-.\r\n                  ; '   : :`-:     _.`* ;\r\n          .*' /  .*' ; .*`- +'  `*'\r\n               `*-*   `*-*  `*-*'       "}
    };

    public string GetAsciiForAction(PetAction action)
    {
        if (!_actionImages.ContainsKey(action))
        {
            return "Image unavailable\n";
        }
        return _actionImages[action];
    }

    public string GetAsciiForMood(PetMood petMood)
    {
        if (!_petMoodImages.ContainsKey(petMood))
        {
            return "Image unavailable\n";
        }
        return _petMoodImages[petMood];
    }

}
