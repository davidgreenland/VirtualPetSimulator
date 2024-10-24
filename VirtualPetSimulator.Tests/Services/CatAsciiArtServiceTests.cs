using NUnit.Framework;
using VirtualPetSimulator.Services;
using VirtualPetSimulator.Actions;
using VirtualPetSimulator.Helpers.Enumerations;
using Moq;

namespace VirtualPetSimulator.Tests.Services;

public class CatASciiArtServiceTests
{
    private CatAsciiArtService CatAsciiArtService;

    [Test]
    public void GetAsciiForInput_WhenGivenValidAction_ReturnsString()
    {
        var artService = new CatAsciiArtService();
        var action = PetAction.Eat;

        var catPic = artService.GetAsciiForInput(action);

        Assert.That(catPic, Is.EqualTo(It.IsAny<string>()));
    }
}
