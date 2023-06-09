﻿namespace Idam.Libs.EF.Interfaces;

/// <summary>
/// Timestamps interface using unix format
/// </summary>
public interface ITimeStamps
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}