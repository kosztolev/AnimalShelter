using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShelterApi.Models;

[Table("shelters")]
public partial class Shelter
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [MaxLength(40)]
    public string City { get; set; } = null!;

    public int Capacity { get; set; }

    public DateTime RegTime { get; set; }

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
