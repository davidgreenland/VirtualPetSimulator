using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Services;

var cat = new CatPet("Steve");
var validator = new Validator();
var userCommunication = new ConsoleUserCommunicationService(cat, new TimeService());
//var eat = new EatAction(cat, validator, userCommunication, 3);
//var sleep = new SleepAction(cat, validator, userCommunication);
//var play = new PlayAction(cat, validator, userCommunication, 3);

var app = new VirtualPetApp(cat, 
    new Dictionary<char, PetActions> { 
        { 'S', PetActions.Sleep },
        { 'E', PetActions.Eat }, 
        { 'P', PetActions.Play } 
    },
    validator, userCommunication);

await app.StartApp();

//userCommunication.RenderScreen();
//Console.WriteLine("Press E to eat");
//await eat.Execute();
//Console.WriteLine($"{cat.Name} is ready for a nap. Press S to sleep");
//Console.ReadKey();
//await sleep.Execute();
//Console.WriteLine($"{cat.Name} wants to play. Press P to play");
//Console.ReadKey();
//await play.Execute();
//Console.ReadKey();