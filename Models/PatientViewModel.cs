using System.Text.Json.Serialization;

namespace RethinkSample.Models;

public class PatientViewModel
{
    public int Id { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Gender { get; init; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Birthday { get; init; }
}