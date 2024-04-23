namespace Betonchel.Domain.BaseModels;

/// <summary>
/// Base class of all Data Base Entities
/// </summary>
public class Entity<TId>
{
    public TId Id { get; init; }

    private bool Equals(Entity<TId> other) => EqualityComparer<TId>.Default.Equals(Id, other.Id);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Entity<TId>)obj);
    }

    public override int GetHashCode() => EqualityComparer<TId>.Default.GetHashCode(Id);

    public override string ToString() => $"{GetType().Name}({nameof(Id)}: {Id})";
}