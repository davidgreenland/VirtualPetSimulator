using VirtualPetSimulator.Helpers;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Models;
using VirtualPetSimulator.Services;

var cat = new CatPet("Steve");
var validator = new Validator();
var userCommunication = new ConsoleUserCommunicationService(cat, new TimeService());

var app = new VirtualPetApp(cat, 
    new Dictionary<char, PetActions> { 
        { 'S', PetActions.Sleep },
        { 'E', PetActions.Eat }, 
        { 'P', PetActions.Play } 
    },
    validator, userCommunication);

await app.StartApp();
