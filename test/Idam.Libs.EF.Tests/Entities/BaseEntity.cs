using Idam.Libs.EF.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Idam.Libs.EF.Tests.Entities;
/// <summary>
/// Base Entity
/// </summary>
/// <seealso cref="IGuidEntity" />
public class BaseEntity : IGuidEntity
{
    public Guid Id { get; set; }

    [StringLength(191)]
    public string Name { get; set; } = default!;

    [StringLength(191)]
    public string? Description { get; set; }
}
