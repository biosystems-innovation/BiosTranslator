using System.Collections.ObjectModel;

namespace Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for collection manipulation and operations,
/// including safe addition, removal, and replacement of elements.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Adds an item to a collection only if it doesn't already exist.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="content">The collection to add to.</param>
    /// <param name="value">The item to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when content or value is null.</exception>
    public static void AddOnce<T> ( this ICollection<T> content, T value )
    {
        ArgumentNullException.ThrowIfNull( content );
        ArgumentNullException.ThrowIfNull( value );

        if ( !content.Contains( value ) )
        {
            content.Add( value );
        }
    }

    /// <summary>
    /// Determines whether a list is null or contains no elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to check.</param>
    /// <returns>true if the list is null or empty; otherwise, false.</returns>
    public static bool IsNullOrEmpty<T> ( this IList<T> list ) =>
        list is null or { Count: 0 };

    /// <summary>
    /// Clears a collection and adds a range of items from an enumerable source.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to modify.</param>
    /// <param name="items">The items to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection or items is null.</exception>
    public static void ClearAndAddRange<T> ( this ICollection<T> collection, IEnumerable<T> items )
    {
        ArgumentNullException.ThrowIfNull( collection );
        ArgumentNullException.ThrowIfNull( items );

        collection.Clear( );
        collection.AddRange( items );
    }

    /// <summary>
    /// Clears a collection and adds a range of items from a params array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to modify.</param>
    /// <param name="items">The items to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection or items is null.</exception>
    public static void ClearAndAddRange<T> ( this ICollection<T> collection, params T[ ] items )
    {
        ArgumentNullException.ThrowIfNull( collection );
        ArgumentNullException.ThrowIfNull( items );

        collection.Clear( );
        collection.AddRange( items );
    }

    /// <summary>
    /// Adds multiple items from an enumerable source to a collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to add to.</param>
    /// <param name="items">The items to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection or items is null.</exception>
    public static void AddRange<T> ( this ICollection<T> collection, IEnumerable<T> items )
    {
        ArgumentNullException.ThrowIfNull( collection );
        ArgumentNullException.ThrowIfNull( items );

        foreach ( var item in items )
        {
            collection.Add( item );
        }
    }

    /// <summary>
    /// Adds multiple items from a params array to a collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to add to.</param>
    /// <param name="items">The items to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection or items is null.</exception>
    public static void AddRange<T> ( this ICollection<T> collection, params T[ ] items )
    {
        ArgumentNullException.ThrowIfNull( collection );
        ArgumentNullException.ThrowIfNull( items );

        foreach ( var item in items )
        {
            collection.Add( item );
        }
    }

    /// <summary>
    /// Removes multiple items from a collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to remove from.</param>
    /// <param name="items">The items to remove.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection or items is null.</exception>
    public static void RemoveRange<T> ( this ICollection<T> collection, IEnumerable<T> items )
    {
        ArgumentNullException.ThrowIfNull( collection );
        ArgumentNullException.ThrowIfNull( items );

        foreach ( var item in items )
        {
            collection.Remove( item );
        }
    }

    /// <summary>
    /// Removes all items that match the specified condition.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to remove from.</param>
    /// <param name="selector">A function to test each element for a condition.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection or selector is null.</exception>
    public static void RemoveRange<T> ( this ICollection<T> collection, Func<T, bool> selector )
    {
        ArgumentNullException.ThrowIfNull( collection );
        ArgumentNullException.ThrowIfNull( selector );

        var itemsToRemove = collection.Where( selector ).ToList();
        collection.RemoveRange( itemsToRemove );
    }


    /// <summary>
    /// Replaces an element at the specified index in a Collection{T}.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to modify.</param>
    /// <param name="index">The index of the element to replace.</param>
    /// <param name="newItem">The new element.</param>
    /// <exception cref="ArgumentNullException">Thrown when collection is null.</exception>
    public static void ReplaceAt<T> ( this Collection<T> collection, int index, T newItem )
    {
        ArgumentNullException.ThrowIfNull( collection );
        collection[ index ] = newItem;
    }

    /// <summary>
    /// Replaces the first occurrence of an element in a Collection{T}.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="sourceCollection">The collection to modify.</param>
    /// <param name="oldItem">The element to replace.</param>
    /// <param name="newItem">The new element.</param>
    /// <exception cref="ArgumentNullException">Thrown when sourceCollection is null.</exception>
    public static void Replace<T> ( this Collection<T> sourceCollection, T oldItem, T newItem )
    {
        ArgumentNullException.ThrowIfNull( sourceCollection );
        var index = sourceCollection.IndexOf( oldItem );
        sourceCollection.ReplaceAt( index, newItem );
    }
}
