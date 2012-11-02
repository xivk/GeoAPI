// Portions copyright 2005 - 2007: Diego Guidi
// Portions copyright 2006 - 2008: Rory Plaire (codekaizen@gmail.com)
//
// This file is part of GeoAPI.Net.
// GeoAPI.Net is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// GeoAPI.Net is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with GeoAPI.Net; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using System.Text;

namespace GeoAPI.Geometries
{
    /// <summary>  
    /// A Dimensionally Extended Nine-Intersection Model (DE-9IM) matrix. This class
    /// can used to represent both computed DE-9IM's (like 212FF1FF2) as well as
    /// patterns for matching them (like T*T******). 
    /// </summary>  
    /// <remarks>
    /// <para>
    /// Methods are provided to:
    /// - Set and query the elements of the matrix in a convenient fashion.
    /// - Convert to and from the standard String representation (specified in SFS Section 2.1.13.2).
    /// - Test to see if a matrix matches a given pattern String.
    /// </para>
    /// <para>
    /// For a description of the DE-9IM, see the 
    /// <see href="http://www.opengis.org/techno/specs.htm">
    /// OpenGIS Simple Features Specification for SQL.
    /// </see>
    /// </para>
    /// <para>
    /// The implementation of the intersection matrix is layed out in the following manner, 
    /// which is helpful when using <see cref="IntersectionMatrix.Item(Int32, Int32)"/> 
    /// to access the dimension values:
    /// </para>
    /// <table>
    ///     <colgroup span="1"/>
    ///     <colgroup span="3"/>
    ///     <thead>
    ///         <tr>
    ///             <th>&nbsp;</th>
    ///             <th scope="col">Interior (col 0)</th>
    ///             <th scope="col">Boundary (col 1)</th>
    ///             <th scope="col">Exterior (col 2)</th>
    ///         </tr>
    ///     </thead>
    ///     <tbody>
    ///       <tr>
    ///          <th scope="row">Interior (row 0)</th>
    ///          <td>dim(I(a) &#x2229; I(b))</td>
    ///          <td>dim(I(a) &#x2229; B(b))</td>
    ///          <td>dim(I(a) &#x2229; E(b))</td>
    ///       </tr>
    ///       <tr>
    ///          <th scope="row">Boundary (row 1)</th>
    ///          <td>dim(B(a) &#x2229; I(b))</td>
    ///          <td>dim(B(a) &#x2229; B(b))</td>
    ///          <td>dim(B(a) &#x2229; E(b))</td>
    ///       </tr>
    ///       <tr>
    ///          <th scope="row">Exterior (row 2)</th>
    ///          <td>dim(E(a) &#x2229; I(b))</td>
    ///          <td>dim(E(a) &#x2229; B(b))</td>
    ///          <td>dim(E(a) &#x2229; E(b))</td>
    ///      </tr>
    ///     </tbody>
    /// </table>
    /// </remarks>     
    public class IntersectionMatrix : IEquatable<IntersectionMatrix>
    {
        private const Int32 MatrixLength = 9;
        private const Int32 MatrixRows = 3;
        private const Int32 MatrixColumns = 3;

        /// <summary>  
        /// Internal representation of this <see cref="IntersectionMatrix" />.
        /// </summary>
        private readonly Dimensions[] _matrix;

        /// <summary>  
        /// Creates an <see cref="IntersectionMatrix" /> with <see langword="null"/> location values.
        /// </summary>
        public IntersectionMatrix()
        {
            _matrix = new Dimensions[MatrixLength];
            SetAll(Dimensions.False);
        }

        /// <summary>
        /// Creates an <see cref="IntersectionMatrix" /> with the given dimension
        /// symbols.
        /// </summary>
        /// <param name="elements">A string of nine dimension symbols in row major order.</param>
        public IntersectionMatrix(String elements)
            : this()
        {
            Set(elements);
        }

        public override Boolean Equals(Object obj)
        {
            IntersectionMatrix other = obj as IntersectionMatrix;
            return Equals(other);
        }

        /// <summary>
        /// Gets the <see cref="Dimensions"/> value at the given
        /// <paramref name="row"/> and <paramref name="column"/>.
        /// </summary>
        /// <param name="row">The row to retrieve.</param>
        /// <param name="column">The column to retrieve.</param>
        /// <returns>The <see cref="Dimensions"/> value at the given index.</returns>
        public Dimensions this[Int32 row, Int32 column]
        {
            get
            {
                if (row < 0 || row >= MatrixRows)
                {
                    throw new ArgumentOutOfRangeException("row", row,
                        "Parameter must be between 0 and " + MatrixRows + " inclusive.");
                }

                if (column < 0 || column >= MatrixColumns)
                {
                    throw new ArgumentOutOfRangeException("column", column,
                        "Parameter must be between 0 and " + MatrixColumns + " inclusive.");
                }

                return _matrix[row * MatrixColumns + column];
            }
            private set
            {
                _matrix[row * MatrixColumns + column] = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Dimensions"/> value at the given index, which is ordered
        /// the same as an OGC SFS standared DE-9IM string.
        /// </summary>
        /// <param name="index">The index of the dimension result to retrieve.</param>
        /// <returns></returns>
        public Dimensions this[Int32 index]
        {
            get
            {
                if (index < 0 || index >= MatrixLength)
                {
                    throw new ArgumentOutOfRangeException("index", index,
                        "Parameter must be between 0 and " + MatrixLength + " inclusive.");
                }

                return _matrix[index];
            }
        }

        /// <summary> 
        /// Creates an <see cref="IntersectionMatrix" /> with the same elements as
        /// <c>other</c>.
        /// </summary>
        /// <param name="other">An <see cref="IntersectionMatrix" /> to copy.</param>         
        public IntersectionMatrix(IntersectionMatrix other)
            : this()
        {
            this[(Int32)Locations.Interior, (Int32)Locations.Interior] =
                other[(Int32)Locations.Interior, (Int32)Locations.Interior];
            this[(Int32)Locations.Interior, (Int32)Locations.Boundary] =
                other[(Int32)Locations.Interior, (Int32)Locations.Boundary];
            this[(Int32)Locations.Interior, (Int32)Locations.Exterior] =
                other[(Int32)Locations.Interior, (Int32)Locations.Exterior];
            this[(Int32)Locations.Boundary, (Int32)Locations.Interior] =
                other[(Int32)Locations.Boundary, (Int32)Locations.Interior];
            this[(Int32)Locations.Boundary, (Int32)Locations.Boundary] =
                other[(Int32)Locations.Boundary, (Int32)Locations.Boundary];
            this[(Int32)Locations.Boundary, (Int32)Locations.Exterior] =
                other[(Int32)Locations.Boundary, (Int32)Locations.Exterior];
            this[(Int32)Locations.Exterior, (Int32)Locations.Interior] =
                other[(Int32)Locations.Exterior, (Int32)Locations.Interior];
            this[(Int32)Locations.Exterior, (Int32)Locations.Boundary] =
                other[(Int32)Locations.Exterior, (Int32)Locations.Boundary];
            this[(Int32)Locations.Exterior, (Int32)Locations.Exterior] =
                other[(Int32)Locations.Exterior, (Int32)Locations.Exterior];
        }

        /// <summary> 
        /// Adds one matrix to another.
        /// Addition is defined by taking the maximum dimension value of each position
        /// in the summand matrices.
        /// </summary>
        /// <param name="im">The matrix to add.</param>        
        public void Add(IntersectionMatrix im)
        {
            for (Int32 i = 0; i < MatrixRows; i++)
            {
                for (Int32 j = 0; j < MatrixColumns; j++)
                {
                    SetAtLeast((Locations)i, (Locations)j, im.Get((Locations)i, (Locations)j));
                }
            }
        }

        /// <summary>  
        /// Returns true if the dimension value satisfies the dimension symbol.
        /// </summary>
        /// <param name="actualDimensionValue">
        /// A <see cref="Dimensions"/> value that can be stored in 
        /// the <see cref="IntersectionMatrix" />. 
        /// </param>
        /// <param name="requiredDimensionSymbol">
        /// A character used in the String
        /// representation of an <see cref="IntersectionMatrix" />. 
        /// Possible values are <c>'T', 'F', '*' , '0', '1', '2'</c>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the dimension symbol encompasses the dimension value.        
        /// </returns>        
        public static Boolean Matches(Dimensions actualDimensionValue, Char requiredDimensionSymbol)
        {
            if (requiredDimensionSymbol == '*')
            {
                return true;
            }
            if (requiredDimensionSymbol == 'T' &&
                (actualDimensionValue >= Dimensions.Point || actualDimensionValue == Dimensions.True))
            {
                return true;
            }
            if (requiredDimensionSymbol == 'F' && actualDimensionValue == Dimensions.False)
            {
                return true;
            }
            if (requiredDimensionSymbol == '0' && actualDimensionValue == Dimensions.Point)
            {
                return true;
            }
            if (requiredDimensionSymbol == '1' && actualDimensionValue == Dimensions.Curve)
            {
                return true;
            }
            if (requiredDimensionSymbol == '2' && actualDimensionValue == Dimensions.Surface)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if each of the actual dimension symbols satisfies the
        /// corresponding required dimension symbol.
        /// </summary>
        /// <param name="actualDimensionSymbols">
        /// Nine dimension symbols to validate.
        /// Possible values are <c>'T', 'F', '*' , '0', '1', '2'</c>.
        /// </param>
        /// <param name="requiredDimensionSymbols">
        /// Nine dimension symbols to validate
        /// against. Possible values are <c>'T', 'F', '*' , '0', '1', '2'</c>.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if each of the required dimension
        /// symbols encompass the corresponding actual dimension symbol.
        /// </returns>
        public static Boolean Matches(String actualDimensionSymbols, String requiredDimensionSymbols)
        {
            IntersectionMatrix m = new IntersectionMatrix(actualDimensionSymbols);
            return m.Matches(requiredDimensionSymbols);
        }

        /// <summary>
        /// Changes the value of one of this <see cref="IntersectionMatrix" /> elements.
        /// </summary>
        /// <param name="row">
        /// The row of this <see cref="IntersectionMatrix" />,
        /// indicating the interior, boundary or exterior of the first <see cref="IGeometry{TCoordinate}"/>
        /// </param>
        /// <param name="column">
        /// The column of this <see cref="IntersectionMatrix" />,
        /// indicating the interior, boundary or exterior of the second <see cref="IGeometry{TCoordinate}"/>
        /// </param>
        /// <param name="dimensionValue">The new value of the element</param>        
        public void Set(Locations row, Locations column, Dimensions dimensionValue)
        {
            this[(Int32)row, (Int32)column] = dimensionValue;
        }

        /// <summary>
        /// Changes the elements of this <see cref="IntersectionMatrix" /> to the
        /// dimension symbols in <c>dimensionSymbols</c>.
        /// </summary>
        /// <param name="dimensionSymbols">
        /// Nine dimension symbols to which to set this <see cref="IntersectionMatrix" />
        /// s elements. Possible values are <c>'T', 'F', '*' , '0', '1', '2'</c>.
        /// </param>
        public void Set(String dimensionSymbols)
        {
            if (dimensionSymbols.Length != MatrixLength)
            {
                throw new ArgumentException(
                    "There must be 9 or fewer characters in the symbol string.");
            }

            for (Int32 i = 0; i < dimensionSymbols.Length; i++)
            {
                Int32 row = i / MatrixColumns;
                Int32 col = i % MatrixColumns;
                this[row, col] = DimensionTypeConverter.ToDimensionValue(dimensionSymbols[i]);
            }
        }

        /// <summary>
        /// Changes the specified element to <paramref name="minimumDimensionValue"/> 
        /// if the element is less.
        /// </summary>
        /// <param name="row">
        /// The row of this <see cref="IntersectionMatrix" />, 
        /// indicating the interior, boundary or exterior of the first 
        /// <see cref="IGeometry{TCoordinate}"/>.
        /// </param>
        /// <param name="column">
        /// The column of this <see cref="IntersectionMatrix" />, 
        /// indicating the interior, boundary or exterior of the second 
        /// <see cref="IGeometry{TCoordinate}"/>.
        /// </param>
        /// <param name="minimumDimensionValue">
        /// The dimension value with which to compare the
        /// element. The order of dimension values from least to greatest is
        /// <see cref="Dimensions.True"/>, <see cref="Dimensions.False"/>, 
        /// <see cref="Dimensions.DontCare"/>, <see cref="Dimensions.Point"/>, 
        /// <see cref="Dimensions.Curve"/>, <see cref="Dimensions.Surface"/>.
        /// </param>
        public void SetAtLeast(Locations row, Locations column, Dimensions minimumDimensionValue)
        {
            if (this[(Int32)row, (Int32)column] < minimumDimensionValue)
            {
                this[(Int32)row, (Int32)column] = minimumDimensionValue;
            }
        }

        /// <summary>
        /// <para>
        /// If <paramref name="row"/> >= 0 and <paramref name="column"/> >= 0, 
        /// changes the specified element to <paramref name="minimumDimensionValue"/>
        /// if the element is less.
        /// </para>
        /// <para>
        /// Does nothing if <paramref name="row"/> is smaller to 0 or 
        /// <paramref name="column"/> is smaller to 0.
        /// </para>
        /// </summary>
        /// <param name="row">The row of the matrix to compare.</param>
        /// <param name="column">The column of the matrix to compare.</param>
        public void SetAtLeastIfValid(Locations row, Locations column, Dimensions minimumDimensionValue)
        {
            if (row >= Locations.Interior && column >= Locations.Interior)
            {
                SetAtLeast(row, column, minimumDimensionValue);
            }
        }

        /// <summary>
        /// For each element in this <see cref="IntersectionMatrix" />, changes the
        /// element to the corresponding minimum dimension symbol if the element is
        /// less.
        /// </summary>
        /// <param name="minimumDimensionSymbols"> 
        /// Nine dimension symbols with which to
        /// compare the elements of this <see cref="IntersectionMatrix" />. The
        /// order of dimension values from least to greatest is <c>Dontcare, True, False, 0, 1, 2</c>.
        /// </param>
        public void SetAtLeast(String minimumDimensionSymbols)
        {
            if (minimumDimensionSymbols.Length != MatrixLength)
            {
                throw new ArgumentException(
                    "There must be 9 or fewer characters in the symbol string.");
            }

            for (Int32 i = 0; i < minimumDimensionSymbols.Length; i++)
            {
                Int32 row = i / MatrixColumns;
                Int32 col = i % MatrixColumns;
                SetAtLeast((Locations)row, (Locations)col,
                           DimensionTypeConverter.ToDimensionValue(minimumDimensionSymbols[i]));
            }
        }

        /// <summary>  
        /// Changes the elements of this <see cref="IntersectionMatrix" /> to <c>dimensionValue</c>.
        /// </summary>
        /// <param name="dimensionValue">
        /// The dimension value to which to set this <see cref="IntersectionMatrix" />
        /// s elements. Possible values <c>True, False, Dontcare, 0, 1, 2}</c>.
        /// </param>         
        public void SetAll(Dimensions dimensionValue)
        {
            for (Int32 ai = 0; ai < MatrixRows; ai++)
            {
                for (Int32 bi = 0; bi < MatrixColumns; bi++)
                {
                    this[ai, bi] = dimensionValue;
                }
            }
        }

        /// <summary>
        /// Returns the value of one of this <see cref="IntersectionMatrix" />s
        /// elements.
        /// </summary>
        /// <param name="row">
        /// The row of this <see cref="IntersectionMatrix" />, indicating
        /// the interior, boundary or exterior of the first <see cref="IGeometry{TCoordinate}"/>.
        /// </param>
        /// <param name="column">
        /// The column of this <see cref="IntersectionMatrix" />,
        /// indicating the interior, boundary or exterior of the second <see cref="IGeometry{TCoordinate}"/>.
        /// </param>
        /// <returns>The dimension value at the given matrix position.</returns>
        public Dimensions Get(Locations row, Locations column)
        {
            return this[(Int32)row, (Int32)column];
        }

        /// <summary>
        /// See methods Get(Int32, Int32) and Set(Int32, Int32, Int32 value)
        /// </summary>         
        public Dimensions this[Locations row, Locations column]
        {
            get { return Get(row, column); }
            set { Set(row, column, value); }
        }

        /// <summary>
        /// Returns <see langword="true" /> if this <see cref="IntersectionMatrix" /> is FF*FF****.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the two <see cref="IGeometry{TCoordinate}"/>'s related by
        /// this <see cref="IntersectionMatrix" /> are disjoint.
        /// </returns>
        public Boolean IsDisjoint()
        {
            return
                this[(Int32)Locations.Interior, (Int32)Locations.Interior] == Dimensions.False &&
                this[(Int32)Locations.Interior, (Int32)Locations.Boundary] == Dimensions.False &&
                this[(Int32)Locations.Boundary, (Int32)Locations.Interior] == Dimensions.False &&
                this[(Int32)Locations.Boundary, (Int32)Locations.Boundary] == Dimensions.False;
        }

        /// <summary>
        /// Returns <see langword="true" /> if <c>isDisjoint</c> returns false.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the two <see cref="IGeometry{TCoordinate}"/>'s related by
        /// this <see cref="IntersectionMatrix" /> intersect.
        /// </returns>
        public Boolean IsIntersects()
        {
            return !IsDisjoint();
        }

        /// <summary>
        /// Returns <see langword="true" /> if this <see cref="IntersectionMatrix" /> is
        /// FT*******, F**T***** or F***T****.
        /// </summary>
        /// <param name="lhs">The dimension of the first <see cref="IGeometry{TCoordinate}"/>.</param>
        /// <param name="rhs">The dimension of the second <see cref="IGeometry{TCoordinate}"/>.</param>
        /// <returns>
        /// <see langword="true" /> if the two <see cref="IGeometry{TCoordinate}"/>s 
        /// related by this <see cref="IntersectionMatrix" /> touch; Returns 
        /// <see langword="false"/> if both <see cref="IGeometry{TCoordinate}"/>s 
        /// are points.
        /// </returns>
        public Boolean IsTouches(Dimensions lhs, Dimensions rhs)
        {
            if (lhs > rhs)
            {
                //no need to get transpose because pattern matrix is symmetrical
                return IsTouches(rhs, lhs);
            }

            if ((lhs == Dimensions.Surface && rhs == Dimensions.Surface) ||
                (lhs == Dimensions.Curve && rhs == Dimensions.Curve) ||
                (lhs == Dimensions.Curve && rhs == Dimensions.Surface) ||
                (lhs == Dimensions.Point && rhs == Dimensions.Surface) ||
                (lhs == Dimensions.Point && rhs == Dimensions.Curve))
            {
                return this[(Int32)Locations.Interior, (Int32)Locations.Interior] == Dimensions.False &&
                       (Matches(this[(Int32)Locations.Interior, (Int32)Locations.Boundary], 'T') ||
                        Matches(this[(Int32)Locations.Boundary, (Int32)Locations.Interior], 'T') ||
                        Matches(this[(Int32)Locations.Boundary, (Int32)Locations.Boundary], 'T'));
            }

            return false;
        }

        /// <summary>
        /// Returns <see langword="true" /> if this <see cref="IntersectionMatrix" /> is
        ///  T*T****** (for a point and a curve, a point and an area or a line
        /// and an area) 0******** (for two curves).
        /// </summary>
        /// <param name="lhs">
        /// The dimension of the first <see cref="IGeometry{TCoordinate}"/>.
        /// </param>
        /// <param name="rhs">
        /// The dimension of the second <see cref="IGeometry{TCoordinate}"/>.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if the two <see cref="IGeometry{TCoordinate}"/>
        /// s related by this <see cref="IntersectionMatrix" /> cross. For this
        /// function to return <see langword="true" />, the <see cref="IGeometry{TCoordinate}"/>s must
        /// be a point and a curve; a point and a surface; two curves; or a curve
        /// and a surface.
        /// </returns>
        public Boolean IsCrosses(Dimensions lhs, Dimensions rhs)
        {
            if ((lhs == Dimensions.Point && rhs == Dimensions.Curve) ||
                (lhs == Dimensions.Point && rhs == Dimensions.Surface) ||
                (lhs == Dimensions.Curve && rhs == Dimensions.Surface))
            {
                return Matches(this[(Int32)Locations.Interior, (Int32)Locations.Interior], 'T') &&
                       Matches(this[(Int32)Locations.Interior, (Int32)Locations.Exterior], 'T');
            }

            if ((lhs == Dimensions.Curve && rhs == Dimensions.Point) ||
                (lhs == Dimensions.Surface && rhs == Dimensions.Point) ||
                (lhs == Dimensions.Surface && rhs == Dimensions.Curve))
            {
                return Matches(this[(Int32)Locations.Interior, (Int32)Locations.Interior], 'T') &&
                       Matches(this[(Int32)Locations.Exterior, (Int32)Locations.Interior], 'T');
            }

            if (lhs == Dimensions.Curve && rhs == Dimensions.Curve)
            {
                return this[(Int32)Locations.Interior, (Int32)Locations.Interior] == 0;
            }

            return false;
        }

        /// <summary>  
        /// Returns <see langword="true" /> if this <see cref="IntersectionMatrix" /> is
        /// T*F**F***.
        /// </summary>
        /// <returns><see langword="true" /> if the first <see cref="IGeometry{TCoordinate}"/> is within the second.</returns>
        public Boolean IsWithin()
        {
            return Matches(this[(Int32)Locations.Interior, (Int32)Locations.Interior], 'T') &&
                   this[(Int32)Locations.Interior, (Int32)Locations.Exterior] == Dimensions.False &&
                   this[(Int32)Locations.Boundary, (Int32)Locations.Exterior] == Dimensions.False;
        }

        /// <summary> 
        /// Returns <see langword="true" /> if this <see cref="IntersectionMatrix" /> is
        /// T*****FF*.
        /// </summary>
        /// <returns><see langword="true" /> if the first <see cref="IGeometry{TCoordinate}"/> contains the second.</returns>
        public Boolean IsContains()
        {
            return Matches(this[(Int32)Locations.Interior, (Int32)Locations.Interior], 'T') &&
                   this[(Int32)Locations.Exterior, (Int32)Locations.Interior] == Dimensions.False &&
                   this[(Int32)Locations.Exterior, (Int32)Locations.Boundary] == Dimensions.False;
        }

        /// <summary>
        /// Returns <see langword="true" /> if this <see cref="IntersectionMatrix" /> is <c>T*****FF*</c>
        /// or <c>*T****FF*</c> or <c>***T**FF*</c> or <c>****T*FF*</c>.
        /// </summary>
        /// <returns><see langword="true" /> if the first <see cref="IGeometry{TCoordinate}"/> covers the second</returns>
        public Boolean IsCovers()
        {
            Boolean hasPointInCommon = Matches(this[(Int32)Locations.Interior, (Int32)Locations.Interior], 'T')
                                       || Matches(this[(Int32)Locations.Interior, (Int32)Locations.Boundary], 'T')
                                       || Matches(this[(Int32)Locations.Boundary, (Int32)Locations.Interior], 'T')
                                       || Matches(this[(Int32)Locations.Boundary, (Int32)Locations.Boundary], 'T');

            return hasPointInCommon &&
                   this[(Int32)Locations.Exterior, (Int32)Locations.Interior] == Dimensions.False &&
                   this[(Int32)Locations.Exterior, (Int32)Locations.Boundary] == Dimensions.False;
        }

        /// <summary> 
        /// Returns <see langword="true" /> if this <see cref="IntersectionMatrix" /> is T*F**FFF*.
        /// </summary>
        /// <param name="lhs">The dimension of the first <see cref="IGeometry{TCoordinate}"/>.</param>
        /// <param name="rhs">The dimension of the second <see cref="IGeometry{TCoordinate}"/>.</param>
        /// <returns>
        /// <see langword="true" /> if the two <see cref="IGeometry{TCoordinate}"/>
        /// s related by this <see cref="IntersectionMatrix" /> are equal; the
        /// <see cref="IGeometry{TCoordinate}"/>s must have the same dimension for this function
        /// to return <see langword="true" />.
        /// </returns>
        public Boolean IsEquals(Dimensions lhs, Dimensions rhs)
        {
            if (lhs != rhs)
            {
                return false;
            }

            return Matches(this[(Int32)Locations.Interior, (Int32)Locations.Interior], 'T') &&
                   this[(Int32)Locations.Exterior, (Int32)Locations.Interior] == Dimensions.False &&
                   this[(Int32)Locations.Interior, (Int32)Locations.Exterior] == Dimensions.False &&
                   this[(Int32)Locations.Exterior, (Int32)Locations.Boundary] == Dimensions.False &&
                   this[(Int32)Locations.Boundary, (Int32)Locations.Exterior] == Dimensions.False;
        }

        /// <summary>
        /// Returns <see langword="true" /> if this <see cref="IntersectionMatrix" /> is
        ///  T*T***T** (for two points or two surfaces)
        ///  1*T***T** (for two curves).
        /// </summary>
        /// <param name="lhs">The dimension of the first <see cref="IGeometry{TCoordinate}"/>.</param>
        /// <param name="rhs">The dimension of the second <see cref="IGeometry{TCoordinate}"/>.</param>
        /// <returns>
        /// <see langword="true" /> if the two <see cref="IGeometry{TCoordinate}"/>
        /// s related by this <see cref="IntersectionMatrix" /> overlap. For this
        /// function to return <see langword="true" />, the <see cref="IGeometry{TCoordinate}"/>s must
        /// be two points, two curves or two surfaces.
        /// </returns>
        public Boolean IsOverlaps(Dimensions lhs, Dimensions rhs)
        {
            if ((lhs == Dimensions.Point && rhs == Dimensions.Point) ||
                (lhs == Dimensions.Surface && rhs == Dimensions.Surface))
            {
                return Matches(this[(Int32)Locations.Interior, (Int32)Locations.Interior], 'T') &&
                       Matches(this[(Int32)Locations.Interior, (Int32)Locations.Exterior], 'T') &&
                       Matches(this[(Int32)Locations.Exterior, (Int32)Locations.Interior], 'T');
            }

            if (lhs == Dimensions.Curve && rhs == Dimensions.Curve)
            {
                return this[(Int32)Locations.Interior, (Int32)Locations.Interior] == Dimensions.Curve &&
                       Matches(this[(Int32)Locations.Interior, (Int32)Locations.Exterior], 'T') &&
                       Matches(this[(Int32)Locations.Exterior, (Int32)Locations.Interior], 'T');
            }

            return false;
        }

        /// <summary> 
        /// Returns whether the elements of this <see cref="IntersectionMatrix" />
        /// satisfies the required dimension symbols.
        /// </summary>
        /// <param name="requiredDimensionSymbols"> 
        /// Nine dimension symbols with which to
        /// compare the elements of this <see cref="IntersectionMatrix" />. Possible
        /// values are <c>{T, F, * , 0, 1, 2}</c>.
        /// </param>
        /// <returns>
        /// <see langword="true" /> if this <see cref="IntersectionMatrix" />
        /// matches the required dimension symbols.
        /// </returns>
        public Boolean Matches(String requiredDimensionSymbols)
        {
            if (requiredDimensionSymbols == null)
            {
                throw new ArgumentNullException("requiredDimensionSymbols");
            }

            if (requiredDimensionSymbols.Length != 9)
            {
                throw new ArgumentException("Should be length 9: " + requiredDimensionSymbols);
            }

            for (Int32 ai = 0; ai < MatrixRows; ai++)
            {
                for (Int32 bi = 0; bi < MatrixColumns; bi++)
                {
                    if (!Matches(this[ai, bi], requiredDimensionSymbols[MatrixColumns * ai + bi]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>  
        /// Transposes this IntersectionMatrix.
        /// </summary>
        /// <returns>This <see cref="IntersectionMatrix" /> as a convenience.</returns>
        public IntersectionMatrix Transpose()
        {
            Dimensions temp = this[1, 0];
            this[1, 0] = this[0, 1];
            this[0, 1] = temp;

            temp = this[2, 0];
            this[2, 0] = this[0, 2];
            this[0, 2] = temp;

            temp = this[2, 1];
            this[2, 1] = this[1, 2];
            this[1, 2] = temp;

            return this;
        }

        /// <summary>
        /// Returns a nine-character <see cref="String"/> representation of 
        /// this <see cref="IntersectionMatrix" />.
        /// </summary>
        /// <returns>
        /// The nine dimension symbols of this <see cref="IntersectionMatrix" />
        /// in row-major order.
        /// </returns>
        public override String ToString()
        {
            StringBuilder buf = new StringBuilder("123456789");

            for (Int32 ai = 0; ai < MatrixRows; ai++)
            {
                for (Int32 bi = 0; bi < MatrixColumns; bi++)
                {
                    buf[MatrixColumns * ai + bi] = DimensionTypeConverter.ToDimensionSymbol(this[ai, bi]);
                }
            }

            return buf.ToString();
        }

        #region IEquatable<IntersectionMatrix> Members

        public Boolean Equals(IntersectionMatrix other)
        {
            if (other == null)
            {
                return false;
            }

            for (Int32 i = 0; i < _matrix.Length; i++)
            {
                if (_matrix[i] != other._matrix[i])
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}