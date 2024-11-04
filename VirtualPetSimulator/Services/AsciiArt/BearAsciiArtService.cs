using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Models.Enums;
using VirtualPetSimulator.Services.Interfaces;

namespace VirtualPetSimulator.Services.AsciiArt;

public class BearAsciiArtService : IAsciiArtService
{
    private readonly IDictionary<PetAction, string> _actionImages = new Dictionary<PetAction, string>() {
            //{ PetAction.Play, " _._     _,-'\"\"`-._\r\n(,-.`._,'(       |\\`-/|\r\n    `-.-' \\ )-`( , o o)\r\n          `-    \\`_`\"'-" },
            { PetAction.Play, "     :\"'._..---.._.'\";\r\n     `.             .'\r\n     .'    ^   ^    `.\r\n    :      a   a      :                 __....._\r\n    :     _.-0-._     :---'\"\"'\"-....--'\"        '.\r\n     :  .'   :   `.  :                          `,`.\r\n      `.: '--'--' :.'                             ; ;\r\n       : `._`-'_.'                                ;.'\r\n       `.   '\"'                                   ;\r\n        `.               '                        ;\r\n         `.     `        :           `            ;\r\n          .`.    ;       ;           :           ;\r\n        .'    `-.'      ;            :          ;`.\r\n    __.'      .'      .'              :        ;   `.\r\n  .'      __.'      .'`--..__      _._.'      ;      ;\r\n  `......'        .'         `'\"\"'`.'        ;......-'\r\n jgs    `.......-'                 `........'" },
            { PetAction.Eat, "                             ,;'':,\r\n                 ,,,,:::::::' ,~, ::\r\n ,:'':.    ..:::~~            \\.' ::\r\n:: .', :::~''     ':             ':.\r\n:: :./              .o.. ':,       ':.\r\n ::,            :'.' '. '           '::    ,, .::,,\r\n  `::          '  /.  : .\\          .::   : ''  ''::\r\n   ::.              '':'            ::'  :: ,'''.  :'\r\n   `::                 :          ,::'  ,:  ',,,' .:'\r\n    `::,                .     ..::::,, ::'      .::'\r\n      '::::++,....      :..::::~     '::,     .::'\r\n      ::   ~''''::::::::'~   .         ''::, ::'\r\n      ::           .': ,.      .         `::::'\r\n      ::             '' ':,     .          ::\r\n       ::                ::      :         ::\r\n         :::,,          ,:'      .       .::'\r\n         :: '::::,,.   ,:'       .     .:':\r\n          ::    ~~::::''        . ,,,::'  ,::'''::\r\n           :::,,,         ...::::::''            ::\r\n           :: '''::::;:::;'': ::.               ,::\r\n          .::             ..:,,::            ..:''\r\n         .::                  ':::    ...,,::'\r\n         ::.                    :::::::''\r\n         `'::::,,,,,,,,,,,,,,,::''" },
            { PetAction.Sleep, "                    ...\r\n                 .::::::          .:::.            \r\n                ::::::@@@@@@@@@:.::::::\r\n                 \"\":@@@@@@@@@@@@@:::::\r\n                  :@@@@@@@@@@@@@@@@:\r\n                .@P;:;@@@@@@@@@@@@@d   zzzzzzzz...\r\n              .@@P@@@:d@@@P::;@@@o \r\n              :@@@od@@@@@::@@::@o@       zzzzzzzzzz.....\r\n              `@@@@(   )@@b:::d@@o \r\n          ..::::@@@@..d@@@@@@@@@@@ \r\n        .:::::.\\::@@@@@@@@P.::::.. \r\n      .::::::::\"::::::::::::::::: \r\n      :::::::::.:::::::::::::::::.\r\n      ::::::::.::::::::::::::::: \r\n     .::::::::.::::::::::::::::: \r\n  .:::::.`::::'@@@@@@@@@@`:::\"'  \r\n..:::::::@@@@@@@@@@@@@@@@@@@@@@@@  \r\n::::::::@@@@@@@@@@@@@@@@@@@@@@@@:  \r\n::::::::.@@@@@@@@@@@@@@@P;:::::::  \r\n ::::::::@@P.::::::::::::::::::::.  \r\n  :::::::,.::::::::::::::::::::::: \r\n  .:::::::.:::::::::::::::::::::::\r\n ::::::::.:::::::::::::::::::::::: \r\n:::::::: ::::::::::::::::::::::::'  \r\n`::::\"' ::::::::\"\"\"::::::\"\"\"'        \r\n         :::::'                           " },
            //{ PetAction.Sit, " /\\_/\\\r\n( o.o ) \r\n > ^ < )\r\n /   \\/ \r\n(\\|||/)"}
    };
    private IDictionary<PetMood, string> _petMoodImages = new Dictionary<PetMood, string>() {
        { PetMood.Grumpy, "          ,-.          ___\r\n         / /\\\\_______ /,-.\\\r\n        | ( ,/       ;;   `;\r\n         `,/         \\\\   ;`,\r\n         ,'           `--'~`,`.\r\n        ;   ,'`.    ,-.     `. `.\r\n        :  ; o :   /o `,     ;  `.\r\n        `. `...;   `.  ;    ,'   :\r\n          `.  .'     `'   ,-     `\r\n            `(__V_,-',-'~~,--'\"\"\"`.\r\n              `----'~ ,-\"\"      ;  `.\r\n              ;----\"\"\"        ;  \"  :\r\n            .'   ;  ;     ,    \" ; \";\r\n          ,'    \"  ,   ; ;         ,'\r\n         ;     \" --`--',-'      \"  :\r\n       ,'   ,-'~~~~~~`;     ,'~~~~~`.\r\n       ;   /         /     /        :\r\n      :   :        ,' ;  ,' ;~~`-._ `,\r\n      ( ; :        `.__,',-'  ;    `-;\r\n(^^^)' vvv`.        vvv ; ; \"  \" ;   )\r\n;|||; ; '_,-`--      _,-'  \"   ;    ,'\r\n)   (_,--'    (^^^)-'   \"  ; ` _\",-'\r\n`---'~        ;|||:  \";  _\"_,-'~~\r\n              )   ( __,-'~~~\r\n              `---'~~~" },
        { PetMood.Happy, "            ,-._____,-.\r\n           (_c       c_)\r\n            /  e-o-e  \\\r\n           (  (._|_,)  )\r\n            >._`---'_,<\r\n          ,'/  `---'  \\`.\r\n        ,' /           \\ `.\r\n       (  (             )  )\r\n        `-'\\           /`-'\r\n           |`-._____.-'|\r\n           |     Y     |\r\n           /     |     \\\r\n      hjw (      |      )"}
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
