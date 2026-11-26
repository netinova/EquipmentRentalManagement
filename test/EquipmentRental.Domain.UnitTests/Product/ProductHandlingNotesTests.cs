using EquipmentRental.Domain.Product;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Product;

public class HandlingNotesTests
{
    [Fact]
    public void Create_ShouldSucceed_WithValidInput()
    {
        // Arrange
        const string notes = "Fragile - handle with care.";

        // Act
        var result = HandlingNotes.Create(notes);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Notes.Should().Be(notes);
    }

    [Fact]
    public void Create_ShouldSucceed_WithEmptyString()
    {
        // Arrange
        const string notes = "";

        // Act
        var result = HandlingNotes.Create(notes);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Notes.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    public void Create_ShouldFail_WithInvalidInput(string? notes)
    {
        // Act
        var result = HandlingNotes.Create(notes);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.HandlingNotesRequired);
    }

    [Fact]
    public void Create_ShouldTrimNotes_WithSurroundingWhiteSpaces()
    {
        // Arrange
        const string notes = "  Fragile - handle with care.  ";

        // Act
        var result = HandlingNotes.Create(notes);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Notes.Should().Be(notes.Trim());
    }
}
