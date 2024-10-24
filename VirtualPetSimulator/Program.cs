using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Services;

var cat = new CatPet("Steve");
var validator = new Validator();
var userCommunication = new ConsoleUserCommunicationService(cat, new TimeService());

var app = new VirtualPetApp(cat, 
    new Dictionary<char, PetAction> { 
        { 'S', PetAction.Sleep },
        { 'E', PetAction.Eat }, 
        { 'P', PetAction.Play } 
    },
    validator, userCommunication);

await app.StartApp();
