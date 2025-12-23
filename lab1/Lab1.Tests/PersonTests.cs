using Xunit;
using Lab1;

namespace Lab1.Tests;


public class PersonTests
{
    [Fact]
    public void FullName_ConcatenatesFirstNameAndLastName()
    {
        // Arrange
        var person = new Person
        {
            FirstName = "Иван",
            LastName = "Филиппов"
        };

        // Act
        var fullName = person.FullName;

        // Assert
        Assert.Equal("Иван Филиппов", fullName);
    }

    [Fact]
    public void IsAdult_Age18OrMore_ReturnsTrue()
    {
        // Arrange
        var person = new Person
        {
            Age = 18
        };

        // Act
        var isAdult = person.IsAdult;

        // Assert
        Assert.True(isAdult);
    }

    [Fact]
    public void IsAdult_AgeLessThan18_ReturnsFalse()
    {
        // Arrange
        var person = new Person
        {
            Age = 17
        };

        // Act
        var isAdult = person.IsAdult;

        // Assert
        Assert.False(isAdult);
    }

    [Fact]
    public void Email_ValidEmail_DoesNotThrow()
    {
        // Arrange
        var person = new Person();

        // Act & Assert
        person.Email = "test@testov.com";
        Assert.Equal("test@testov.com", person.Email);
    }

    [Fact]
    public void Email_InvalidEmail_ThrowsArgumentException()
    {
        // Arrange
        var person = new Person();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => person.Email = "invalid-email");
    }

    [Fact]
    public void Email_EmptyString_DoesNotThrow()
    {
        // Arrange
        var person = new Person();

        // Act & Assert
        person.Email = "";
        Assert.Equal("", person.Email);
    }

    [Fact]
    public void FullName_EmptyNames_ReturnsSpacedString()
    {
        // Arrange
        var person = new Person
        {
            FirstName = "",
            LastName = ""
        };

        // Act
        var fullName = person.FullName;

        // Assert
        Assert.Equal(" ", fullName);
    }
}


