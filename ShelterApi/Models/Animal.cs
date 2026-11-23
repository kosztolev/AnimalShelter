using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShelterApi.Models;

[Table("animals")]
public partial class Animal
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(40)]
    public string Name { get; set; } = null!;
    [MaxLength(30)]
    public string Species { get; set; } = null!;

    public int Age { get; set; }

    public double Weight { get; set; }

    public DateTime RegTime { get; set; }

    public Guid ShelterId { get; set; }

    [JsonIgnore]
    public virtual Shelter Shelter { get; set; } = null!;
}
