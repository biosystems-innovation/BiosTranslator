namespace Infrastructure.Extensions
{
    public static class BoolExtensions
    {
        /// <summary>Converts a nullable boolean source into a source of <see cref="bool" /> type.</summary>
        /// <param name="source">The source nullable bool.</param>
        /// <returns><c>False</c> if the given source is <c>null</c>; otherwise the current boolean source.</returns>
        public static bool ToBool ( this bool? source )
        {
            return source.HasValue && source.Value;
        }

        /// <summary>Converts a nullable <see cref="bool" /> to an <see cref="int" />.</summary>
        /// <param name="source">The source.</param>
        /// <returns><c>1</c> if <c>True</c>; otherwise <c>0</c>.</returns>
        public static int ToInt ( this bool? source )
        {
            return source.ToBool( ) ? 1 : 0;
        }
    }
}
