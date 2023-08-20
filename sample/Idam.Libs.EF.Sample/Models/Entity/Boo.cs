using Idam.Libs.EF.Attributes;
using Idam.Libs.EF.Interfaces;
using Idam.Libs.EF.Sample.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Idam.Libs.EF.Sample.Models.Entity;
[TimeStamps]
public class Boo : IGuidEntity
{
    public Boo()
    {
    }

    public Boo(BooCreateDto dto)
    {
        this.Name = dto.Name;
        this.Description = dto.Description;
    }

    public Boo(BooUpdateDto dto)
    {
        this.Id = dto.Id;
        this.Name = dto.Name;
        this.Description = dto.Description;
    }

    public Guid Id { get; set; }

    [StringLength(191)]
    public string Name { get; set; } = default!;

    [StringLength(191)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
