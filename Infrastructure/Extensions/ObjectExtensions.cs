using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for object type operations, type checking, JSON serialization, and error handling.
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Checks if an object reference is null.
    /// </summary>
    /// <param name="value">The object to check.</param>
    /// <returns>True if the object is null; otherwise false.</returns>
    public static bool IsNull ( [NotNullWhen( false )] this object? value ) =>
        value is null;

    /// <summary>
    /// Checks if an object reference is null and executes an action if true.
    /// </summary>
    /// <param name="value">Object to check.</param>
    /// <param name="action">Action to execute if the object is null.</param>
    /// <returns>True if the object is null; otherwise false.</returns>
    public static bool IfNullThen ( this object? value, Action action )
    {
        if ( value.IsNull( ) )
        {
            action( );
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if an object reference is null and executes an action if true.
    /// </summary>
    /// <param name="value">Object to check.</param>
    /// <param name="action">Action to execute if the object is null.</param>
    /// <returns>True if the object is null; otherwise false.</returns>
    public static Task<bool> IfNullThenAsync ( this object? value, Func<Task> action )
    {
        if ( value.IsNull( ) )
        {
            action( ).Wait( );
            return Task.FromResult( true );
        }
        return Task.FromResult( false );
    }

    /// <summary>
    /// Checks if an object reference is not null.
    /// </summary>
    /// <param name="value">The object to check.</param>
    /// <returns>True if the object is not null; otherwise false.</returns>
    public static bool IsNotNull ( [NotNullWhen( true )] this object? value ) =>
        value is not null;


    /// <summary>
    /// Checks if an object reference is not null and executes an action if true.
    /// </summary>
    /// <param name="value">The object to check.</param>
    /// <param name="action">Action to execute if the object is not null.</param>
    /// <returns>True if the object is not null; otherwise false.</returns>
    public static bool IfNotNullThen ( this object? value, Action action )
    {
        if ( value.IsNotNull( ) )
        {
            action( );
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if an object reference is not null and executes an action if true.
    /// </summary>
    /// <param name="value">The object to check.</param>
    /// <param name="action">Action to execute if the object is not null.</param>
    /// <returns>True if the object is not null; otherwise false.</returns>
    public static Task<bool> IfNotNullThenAsync ( this object? value, Func<Task> action )
    {
        if ( value.IsNotNull( ) )
        {
            action( ).Wait( );
            return Task.FromResult( true );
        }
        return Task.FromResult( false );
    }

    /// <summary>
    /// Safely casts an object to a specified type.
    /// </summary>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <param name="value">The object to cast.</param>
    /// <returns>The casted object or null if cast is not possible.</returns>
    public static T Cast<T> ( this object value ) =>
        (T) value;

    /// <summary>
    /// Converts an object to its string representation, handling null values.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <returns>String representation of the object or null if the object is null.</returns>
    public static string? ToNullableString ( this object? value ) =>
        value?.ToString( );

    /// <summary>
    /// Serializes an object to JSON string.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <returns>JSON string representation or null if the input is null.</returns>
    public static string? ToJson ( this object? value, JsonSerializerOptions? options = null ) =>
        value.IsNull() ? null : JsonSerializer.Serialize( value, options );

    /// <summary>
    /// Deserializes a JSON string to an object of specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="json">The JSON string.</param>
    /// <param name="options">Optional JSON serializer options.</param>
    /// <returns>Deserialized object or null if the input is null or empty.</returns>
    public static T? FromJson<T> ( this string? json, JsonSerializerOptions? options = null ) where T : class
    {
        try
        {
            return json.IsNullOrWhiteSpace() ? null : JsonSerializer.Deserialize<T>( json, options );
        }
        catch ( System.Text.Json.JsonException ex)
        {
            return null;
        }
    }

    /// <summary>
    /// Creates a deep clone of an object using JSON serialization.
    /// </summary>
    /// <typeparam name="T">The type of object to clone.</typeparam>
    /// <param name="value">The object to clone.</param>
    /// <returns>A deep clone of the object or null if the input is null.</returns>
    public static T? DeepClone<T> ( this T value ) where T : class
    {
        if ( value is null ) return null;
        var json = JsonSerializer.Serialize( value );
        return JsonSerializer.Deserialize<T>( json );
    }

    /// <summary>
    /// Gets a custom attribute from an object's type.
    /// </summary>
    /// <typeparam name="T">The type of attribute to get.</typeparam>
    /// <param name="value">The object to get the attribute from.</param>
    /// <returns>The custom attribute or null if not found.</returns>
    public static T? GetCustomAttribute<T> ( this object value ) where T : class =>
        value.GetCustomAttribute<T>( false );

    /// <summary>
    /// Gets a custom attribute from an object's type with inheritance option.
    /// </summary>
    /// <typeparam name="T">The type of attribute to get.</typeparam>
    /// <param name="value">The object to get the attribute from.</param>
    /// <param name="inherit">Whether to search the inheritance chain.</param>
    /// <returns>The custom attribute or null if not found.</returns>
    public static T? GetCustomAttribute<T> ( this object value, bool inherit ) where T : class =>
        value.GetType( ).GetCustomAttributes( typeof( T ), inherit ).FirstOrDefault( ) as T;

    /// <summary>
    /// Checks if an object's type has a specific custom attribute.
    /// </summary>
    /// <typeparam name="T">The type of attribute to check for.</typeparam>
    /// <param name="value">The object to check.</param>
    /// <returns>True if the attribute exists; otherwise false.</returns>
    public static bool HasCustomAttribute<T> ( this object value ) where T : class =>
        value.HasCustomAttribute<T>( false );

    /// <summary>
    /// Checks if an object's type has a specific custom attribute with inheritance option.
    /// </summary>
    /// <typeparam name="T">The type of attribute to check for.</typeparam>
    /// <param name="value">The object to check.</param>
    /// <param name="inherit">Whether to search the inheritance chain.</param>
    /// <returns>True if the attribute exists; otherwise false.</returns>
    public static bool HasCustomAttribute<T> ( this object value, bool inherit ) where T : class =>
        value.GetCustomAttribute<T>( inherit ) is not null;

    /// <summary>
    /// Safely casts an object to a specified type using pattern matching.
    /// </summary>
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <param name="source">The object to cast.</param>
    /// <returns>The casted value or default of T if cast is not possible.</returns>
    public static T? As<T> ( this object? source ) =>
        source is T result ? result : default;

    /// <summary>
    /// Checks if an object is of a specified type.
    /// </summary>
    /// <typeparam name="T">The type to check against.</typeparam>
    /// <param name="source">The object to check.</param>
    /// <returns>True if the object is of type T; otherwise false.</returns>
    public static bool Is<T> ( this object source ) =>
        source is T;

    /// <summary>
    /// Checks if an object is not of a specified type.
    /// </summary>
    /// <typeparam name="T">The type to check against.</typeparam>
    /// <param name="source">The object to check.</param>
    /// <returns>True if the object is not of type T; otherwise false.</returns>
    public static bool IsNot<T> ( this object source ) =>
        source.IsNotNull() && !Is<T>( source );

    /// <summary>
    /// Checks if an object's runtime type matches a specified type.
    /// </summary>
    /// <typeparam name="T">The type to check against.</typeparam>
    /// <param name="source">The object to check.</param>
    /// <returns>True if the object's type matches T; otherwise false.</returns>
    public static bool IsTypeOf<T> ( this object source ) =>
        source.IsNotNull( ) && source.GetType( ) == typeof( T );


    /// <summary>
    /// Checks if an object's runtime type does not match a specified type.
    /// </summary>
    /// <typeparam name="T">The type to check against.</typeparam>
    /// <param name="source">The object to check.</param>
    /// <returns>True if the object's type does not match T; otherwise false.</returns>
    public static bool IsNotTypeOf<T> ( this object source ) =>
        source.IsNotNull( ) && !source.IsTypeOf<T>( );
}


