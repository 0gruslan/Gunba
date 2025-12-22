using System.Text.Json.Serialization;

namespace Lab1;


public class Person
{
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("age")]
    public int Age { get; set; }

    private string _email = string.Empty;

    [JsonPropertyName("email")]
    public string Email
    {
        get => _email;
        set
        {
            if (!string.IsNullOrEmpty(value) && !value.Contains('@'))
            {
                throw new ArgumentException("Email должен содержать символ '@'", nameof(Email));
            }
            _email = value;
        }
    }

    [JsonIgnore]
    public string Password { get; set; } = string.Empty;

    
    [JsonIgnore]
    public string FullName => $"{FirstName} {LastName}";

   
    [JsonIgnore]
    public bool IsAdult => Age >= 18;
}


