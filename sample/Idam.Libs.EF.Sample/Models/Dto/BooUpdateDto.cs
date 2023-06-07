using System.ComponentModel.DataAnnotations;

namespace Idam.Libs.EF.Sample.Models.Dto;

public class BooUpdateDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(191)]
    public string Name { get; set; } = default!;

    [StringLength(191)]
    public string? Description { get; set; }
}
