using Idam.Libs.EF.Interfaces;
using Idam.Libs.EF.Sample.Models.Dto;
using System.ComponentModel.DataAnnotations;

namespace Idam.Libs.EF.Sample.Models.Entity;

/// <summary>
/// Foo entity
/// </summary>
public class Foo : IGuidEntity, ITimeStamps, ISoftDelete
{
    public Foo()
    {
    }

    public Foo(FooCreateDto dto)
    {
        this.Name = dto.Name;
        this.Description = dto.Description;
    }

    public Foo(FooUpdateDto dto)
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

    public long CreatedAt { get; set; }

    public long UpdatedAt { get; set; }

    public long? DeletedAt { get; set; }
}