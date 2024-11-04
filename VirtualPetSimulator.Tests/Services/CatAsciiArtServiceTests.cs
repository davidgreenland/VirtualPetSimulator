using NUnit.Framework;
using VirtualPetSimulator.Actions;
using Moq;
using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Services.AsciiArt;

namespace VirtualPetSimulator.Tests.Services;

public class CatASciiArtServiceTests
{
    private CatAsciiArtService CatAsciiArtService;

    [Test]
    public void GetAsciiForAction_WhenGivenValidAction_ReturnsString()
    {
        var artService = new CatAsciiArtService();
        var action = PetAction.Eat;

        var catPic = artService.GetAsciiForAction(action);

        Assert.That(catPic, Is.Not.Null.Or.Empty);
    }
}
