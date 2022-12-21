using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RethinkSample.Models;

public class NewPatientViewModel
{
    [Required]
    [StringLength(255)]
    public string FirstName { get; init; }

    [Required]
    [StringLength(255)]
    public string LastName { get; init; }

    [Required]
    [StringLength(1)]
    public string Gender { get; init; }

    [Required]
    [DataType(DataType.Date)]
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Birthday { get; init; }
}