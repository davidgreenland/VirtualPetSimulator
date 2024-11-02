using VirtualPetSimulator.Configuration;
using VirtualPetSimulator.Factories;
using VirtualPetSimulator.Services;
using Validator = VirtualPetSimulator.Validators.Validator;

var keyStrokeMappings = new KeyStrokeMappings();
var petFactory = new PetFactory();
var timeService = new TimeService();
var validator = new Validator();
var userCommunication = new ConsoleUserCommunicationService(new TimeService(), new CatAsciiArtService());
var petActionFactory = new PetActionFactory(validator, userCommunication, timeService);

var app = new VirtualPetApp(keyStrokeMappings, userCommunication, timeService, petActionFactory);

var petType = app.ChoosePetType();
var petName = app.ChooseName();
var pet = petFactory.CreatePet(petType, petName);

await app.Run(pet);
