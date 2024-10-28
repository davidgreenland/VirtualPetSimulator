using VirtualPetSimulator.Actions.SoundBehaviours;
using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Services;

var cat = new CatPet("Steve", new Meow());
var timeService = new TimeService();
var userCommunication = new ConsoleUserCommunicationService(new TimeService(), new CatAsciiArtService());

var app = new VirtualPetApp(cat, 
    new Dictionary<char, PetAction> { 
        { 'S', PetAction.Sleep },
        { 'E', PetAction.Eat }, 
        { 'P', PetAction.Play } 
    },
    userCommunication, timeService);

await app.StartApp();
