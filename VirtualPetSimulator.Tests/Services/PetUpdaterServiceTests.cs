using Moq;
using NUnit.Framework;
using System.Runtime.Intrinsics.X86;
using VirtualPetSimulator.Actions.Enums;
using VirtualPetSimulator.Models.Interfaces;
using VirtualPetSimulator.Services;

namespace VirtualPetSimulator.Tests.Services;

public class PetUpdaterServiceTests
{
    private Mock<IPet> _testPet;


    [SetUp]
    public void SetUp()
    {
        _testPet = new Mock<IPet>();


    }
    [Test]
    public void UpdatePetAttributes_IfPetISSleeping_ShouldNotDecreaseEnergy()
    {
        _testPet.Setup(x => x.CurrentAction).Returns(PetAction.Sleep);
        
        PetUpdaterService.UpdatePetAttributes(_testPet.Object);

        _testPet.Verify(x => x.ChangeEnergy(It.IsAny<int>()), Times.Never());
    }
}
