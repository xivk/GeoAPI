using System;

namespace GeoAPI.Geometries
{
    /// <summary>
    /// Provides constants representing the dimensions of a point, a curve and a surface.
    /// </summary>
    /// <remarks>
    /// Also provides constants representing the dimensions of the empty geometry and
    /// non-empty geometries, and the wildcard constant <see cref="Dontcare"/> meaning "any dimension".
    /// These constants are used as the entries in <see cref="IntersectionMatrix"/>s.
    /// </remarks>
    public enum Dimension
    {
        /// <summary>
        /// Dimension value of a point (0).
        /// </summary>
        Point = 0,

        /// <summary>
        /// Dimension value of a curve (1).
        /// </summary>
        Curve = 1,

        /// <summary>
        /// Dimension value of a surface (2).
        /// </summary>
        Surface = 2,

        /// <summary>
        /// Dimension value of a empty point (-1).
        /// </summary>
        False = -1,

        /// <summary>
        /// Dimension value of non-empty geometries (= {Point,Curve,A}).
        /// </summary>
        True = -2,

        /// <summary>
        /// Dimension value for any dimension (= {False, True}).
        /// </summary>
        Dontcare = -3
    }

    /// <summary>
    /// Class containing static methods for conversions
    /// between dimension values and characters.
    /// </summary>
    public class DimensionUtility
    {
        /// <summary>
        /// Converts the dimension value to a dimension symbol,
        /// for example, <c>True => 'T'</c>
        /// </summary>
        /// <param name="dimensionValue">Number that can be stored in the <c>IntersectionMatrix</c>.
        /// Possible values are <c>True, False, Dontcare, 0, 1, 2</c>.</param>
        /// <returns>Character for use in the string representation of an <c>IntersectionMatrix</c>.
        /// Possible values are <c>T, F, * , 0, 1, 2</c>.</returns>
        public static char ToDimensionSymbol(Dimension dimensionValue)
        {
            switch (dimensionValue)
            {
                case Dimension.False:
                    return 'F';
                case Dimension.True:
                    return 'T';
                case Dimension.Dontcare:
                    return '*';
                case Dimension.Point:
                    return '0';
                case Dimension.Curve:
                    return '1';
                case Dimension.Surface:
                    return '2';
                default:
                    throw new ArgumentOutOfRangeException
                        ("Unknown dimension value: " + dimensionValue);
            }
        }

        /// <summary>
        /// Converts the dimension symbol to a dimension value,
        /// for example, <c>'*' => Dontcare</c>
        /// </summary>
        /// <param name="dimensionSymbol">Character for use in the string representation of an <c>IntersectionMatrix</c>.
        /// Possible values are <c>T, F, * , 0, 1, 2</c>.</param>
        /// <returns>Number that can be stored in the <c>IntersectionMatrix</c>.
        /// Possible values are <c>True, False, Dontcare, 0, 1, 2</c>.</returns>
        public static Dimension ToDimensionValue(char dimensionSymbol)
        {
            switch (Char.ToUpper(dimensionSymbol))
            {
                case 'F':
                    return Dimension.False;
                case 'T':
                    return Dimension.True;
                case '*':
                    return Dimension.Dontcare;
                case '0':
                    return Dimension.Point;
                case '1':
                    return Dimension.Curve;
                case '2':
                    return Dimension.Surface;
                default:
                    throw new ArgumentOutOfRangeException
                        ("Unknown dimension symbol: " + dimensionSymbol);
            }
        }
    }
}
