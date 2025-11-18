using System.Globalization;
using System.Text.RegularExpressions;

namespace Infrastructure.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty ( this string value )
    {
        return string.IsNullOrEmpty( value );
    }

    public static bool IsNullOrWhiteSpace ( this string value )
    {
        return string.IsNullOrWhiteSpace( value );
    }

    public static bool IsValidDirectory ( this string value )
    {
        if ( value.IsNullOrEmpty( ) ) return false;
        try
        {
            if ( Directory.Exists( value ) )
            {
                return true;
            }
            else
            {
                Directory.CreateDirectory( value );
            }

            return true;

        }
        catch ( Exception e )
        {
            return false;
        }

    }

    public static int? GetIntValue ( this string value )
    {
        return int.TryParse( value, out int result ) ? result : null;
    }

    public static decimal? GetDecimalValue ( this string? origin )
    {
        if ( origin.IsNullOrEmpty( ) ) { return null; }

        var conversion = decimal.TryParse( origin, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal parsedValue );
        if ( conversion ) return parsedValue;

        const string comma = ",";
        const string dot = ".";
        if ( origin.Contains( comma ) )
        {
            return GetDecimalValue( origin.Replace( comma, dot ) );
        }

        return null;
    }

    public static double? GetDoubleFromString ( this string? numberStr )
    {
        if ( numberStr.IsNullOrEmpty( ) )
            return null;
        else
        {
            var points = numberStr.Count( c => c == '.' );
            var commas = numberStr.Count( c => c == ',' );

            if ( points == 0 && commas == 1 ) numberStr = numberStr.Replace( ",", "." );

            // No thousand separator
            if ( double.TryParse( numberStr, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double resultInvariant ) )
            {
                return resultInvariant;
            }

            // Point for thousands, comma for decimals
            if ( double.TryParse( numberStr, NumberStyles.Number, new CultureInfo( "es-ES" ), out double resultEsEs ) )
            {
                return resultEsEs;
            }

            // Comma for thousands, point for decimals
            if ( double.TryParse( numberStr, NumberStyles.Number, new CultureInfo( "en-US" ), out double resultEnUs ) )
            {
                return resultEnUs;
            }

            return null;
        }
    }

    /// <summary>
    /// https://stackoverflow.com/questions/9806967/formatting-any-string-to-string-yyyy-mm-dd
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static string ToDate ( this string? origin, string sourceFormat = "yyyyMMdd", string destinationFormat = "yyyy/MM/dd", IFormatProvider formatProvider = null )
    {
        if ( origin.IsNull( ) ) return string.Empty;
        if ( formatProvider.IsNull( ) ) formatProvider = CultureInfo.InvariantCulture;
        try
        {
            DateTime dt = DateTime.ParseExact( origin, sourceFormat, formatProvider );
            return dt.ToString( destinationFormat );
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Get separators string and apply a " - " between each of them
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string? FormatSeparators ( this string? value )
    {
        if ( value.IsNull( ) ) return null;

        string pattern = @"[, ;:]|\\[a-zA-Z]";
        string result = "";

        MatchCollection matches = Regex.Matches( value, pattern );

        foreach ( Match match in matches )
        {
            result += match.Value;

            if ( match.Index < matches.Count - 1 )
            {
                result += " - ";
            }
        }

        return result;
    }

    public static bool EqualsIgnoreCase ( this string? lhs, string? rhs )
    {
        if ( lhs.IsNull( ) && rhs.IsNull( ) ) return true;
        if ( lhs.IsNull( ) || rhs.IsNull( ) ) return false;

        return lhs.Equals( rhs, StringComparison.OrdinalIgnoreCase );
    }


    public static bool IsValidFileName ( this string? fileName )
    {
        if ( fileName.IsNull( ) ) return false;

        char[ ] invalidChars = { '<', '>', ':', '"', '/', '\\', '|', '?', '*', '#', '+', '&' };

        return !invalidChars.Any( c => fileName.Contains( c ) );
    }

}