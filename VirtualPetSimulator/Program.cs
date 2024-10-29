using VirtualPetSimulator.Factories;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Services;

var petFactory = new PetFactory();
var timeService = new TimeService();
var userCommunication = new ConsoleUserCommunicationService(new TimeService(), new CatAsciiArtService());

var app = new VirtualPetApp(
    new Dictionary<char, PetAction> { 
        { 'S', PetAction.Sleep },
        { 'E', PetAction.Eat }, 
        { 'P', PetAction.Play } 
    },
    new Dictionary<char, PetType> {
        { 'C', PetType.Cat },
    },
    userCommunication, timeService);

var petType = app.ChoosePet();
var petName = app.ChooseName();
var pet = petFactory.CreatePet(petType, petName);
app.SetPet(pet);
await app.Run();
