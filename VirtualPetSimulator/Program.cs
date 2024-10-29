﻿using System.ComponentModel.DataAnnotations;
using VirtualPetSimulator.Factories;
using VirtualPetSimulator.Helpers.Enumerations;
using VirtualPetSimulator.Services;
using Validator = VirtualPetSimulator.Helpers.Validator;

var petFactory = new PetFactory();
var timeService = new TimeService();
var validator = new Validator();
var userCommunication = new ConsoleUserCommunicationService(new TimeService(), new CatAsciiArtService());
var petActionFactory = new PetActionFactory(validator, userCommunication, timeService);

var app = new VirtualPetApp(
    new Dictionary<char, PetAction> { 
        { 'S', PetAction.Sleep },
        { 'E', PetAction.Eat }, 
        { 'P', PetAction.Play } 
    },
    new Dictionary<char, PetType> {
        { 'C', PetType.Cat },
    },
    userCommunication, timeService, petActionFactory);

var petType = app.ChoosePetType();
var petName = app.ChooseName();
var pet = petFactory.CreatePet(petType, petName);
app.SetPet(pet);
await app.Run();
