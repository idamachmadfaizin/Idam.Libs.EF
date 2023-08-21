﻿using Idam.Libs.EF.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Idam.Libs.EF.Extensions;

/// <summary>
/// DbContext extension class.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Add timestamps to the Entity with TimeStampsAttribute when state is Added or Modified or Deleted.
    /// </summary>
    /// <param name="changeTracker">The change tracker.</param>
    /// <exception cref="InvalidCastException"></exception>
    public static void AddTimestamps(this ChangeTracker changeTracker)
    {
        foreach (EntityEntry entityEntry in changeTracker.Entries())
        {
            TimeStampsAttribute? timeStampsAttribute = entityEntry.Entity.GetType().GetCustomAttribute<TimeStampsAttribute>();

            if (timeStampsAttribute is not null)
            {
                AddTimestamps(entityEntry, timeStampsAttribute);
            }
        }
    }

    /// <summary>
    /// Add timestamps to the Entity with TimeStampsAttribute when state is Added or Modified or Deleted.
    /// </summary>
    /// <param name="entityEntry">The entity entry.</param>
    /// <param name="timeStampsAttribute">The time stamps attribute.</param>
    /// <exception cref="InvalidCastException"></exception>
    private static void AddTimestamps(this EntityEntry? entityEntry, TimeStampsAttribute timeStampsAttribute)
    {
        if (entityEntry is null) return;

        // current datetime
        var now = timeStampsAttribute.TimeStampsType.GetMapValue();

        Type entityType = entityEntry.Entity.GetType();
        PropertyInfo? createdAtProperty = entityType.GetProperty(timeStampsAttribute.CreatedAtField);
        PropertyInfo? updatedAtProperty = entityType.GetProperty(timeStampsAttribute.UpdatedAtField);
        PropertyInfo? deletedAtProperty = entityType.GetProperty(timeStampsAttribute.DeletedAtField);

        switch (entityEntry.State)
        {
            case EntityState.Modified:
                InvalidCastValidationException.ThrowIfInvalidTimeStamps(timeStampsAttribute.UpdatedAtField, entityType, timeStampsAttribute);

                updatedAtProperty!.SetValue(entityEntry.Entity, now, null);
                break;

            case EntityState.Added:
                InvalidCastValidationException.ThrowIfInvalidTimeStamps(timeStampsAttribute.CreatedAtField, entityType, timeStampsAttribute);
                InvalidCastValidationException.ThrowIfInvalidTimeStamps(timeStampsAttribute.UpdatedAtField, entityType, timeStampsAttribute);

                createdAtProperty!.SetValue(entityEntry.Entity, now, null);
                updatedAtProperty!.SetValue(entityEntry.Entity, now, null);
                break;

            case EntityState.Deleted:
                InvalidCastValidationException.ThrowIfInvalidTimeStamps(timeStampsAttribute.DeletedAtField, entityType, timeStampsAttribute);

                var value = deletedAtProperty!.GetValue(entityEntry.Entity);

                if (value is null)
                {
                    entityEntry.State = EntityState.Modified;
                    deletedAtProperty.SetValue(entityEntry.Entity, now, null);
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Query Filter to get model where DeletedAt field is null.
    /// </summary>
    /// <param name="builder">The builder.</param>
    public static void AddSoftDeleteFilter(this ModelBuilder builder)
    {
        IEnumerable<IMutableEntityType>? mutables = builder.Model.GetEntityTypes();

        if (mutables is not null)
        {
            foreach (IMutableEntityType mutable in mutables)
            {
                builder.AddSoftDeleteFilter(mutable);
            }
        }
    }

    /// <summary>
    /// Query Filter to get model where DeletedAt field is null.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="mutable">The mutable.</param>
    private static void AddSoftDeleteFilter(this ModelBuilder builder, IMutableEntityType? mutable)
    {
        if (mutable is null) return;

        TimeStampsAttribute? timeStampsAttribute = mutable.ClrType.GetCustomAttribute<TimeStampsAttribute>();

        if (timeStampsAttribute is null) return;

        ParameterExpression parameter = Expression.Parameter(mutable.ClrType, "e");

        Type[] typeArguments = new[] { timeStampsAttribute.TimeStampsType.GetNullableMapType() };

        BinaryExpression body = Expression.Equal(
                Expression.Call(typeof(Microsoft.EntityFrameworkCore.EF), nameof(Microsoft.EntityFrameworkCore.EF.Property), typeArguments, parameter, Expression.Constant(timeStampsAttribute.DeletedAtField)),
                Expression.Constant(null));

        LambdaExpression expression = Expression.Lambda(body, parameter);

        builder.Entity(mutable.ClrType).HasQueryFilter(expression);
    }
}