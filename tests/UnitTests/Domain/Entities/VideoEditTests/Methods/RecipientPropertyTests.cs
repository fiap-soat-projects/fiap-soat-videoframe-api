using Domain.Entities;
using Domain.Entities.Exceptions;
using FluentAssertions;

namespace UnitTests.Domain.Entities.VideoEditTests.Methods;

public class RecipientPropertyTests : VideoEditDependenciesMock
{
    [Fact]
    public void When_Recipient_Is_Get_Then_Returns_Stored_Value()
    {
        // Act
        var result = _sut.Recipient;

        // Assert
        result.Should().Be("recipient@example.com");
    }

    [Fact]
    public void When_Recipient_Is_Set_With_Valid_Value_Then_Updates_Property()
    {
        // Arrange
        var newRecipient = "newrecipient@example.com";

        // Act
        _sut.Recipient = newRecipient;

        // Assert
        _sut.Recipient.Should().Be(newRecipient);
    }

    [Theory]
    [InlineData("user1@example.com")]
    [InlineData("webhook@webhook.com")]
    [InlineData("target@domain.org")]
    public void When_Recipient_Is_Set_With_Different_Valid_Values_Then_Updates_Property(string newRecipient)
    {
        // Act
        _sut.Recipient = newRecipient;

        // Assert
        _sut.Recipient.Should().Be(newRecipient);
    }

    [Fact]
    public void When_Recipient_Is_Set_To_Null_Then_Throw_InvalidEntityPropertyException()
    {
        // Act
        var act = () => _sut.Recipient = null;

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void When_Recipient_Is_Set_To_Whitespace_Then_Throw_InvalidEntityPropertyException(string whitespaceRecipient)
    {
        // Act
        var act = () => _sut.Recipient = whitespaceRecipient;

        // Assert
        act.Should()
            .Throw<InvalidEntityPropertyException<VideoEdit>>();
    }

    [Fact]
    public void When_Recipient_Is_Changed_Multiple_Times_Then_Property_Reflects_Latest_Value()
    {
        // Arrange
        var recipient1 = "recipient1@example.com";
        var recipient2 = "recipient2@example.com";
        var recipient3 = "recipient3@example.com";

        // Act
        _sut.Recipient = recipient1;
        _sut.Recipient = recipient2;
        _sut.Recipient = recipient3;

        // Assert
        _sut.Recipient.Should().Be(recipient3);
    }
}
