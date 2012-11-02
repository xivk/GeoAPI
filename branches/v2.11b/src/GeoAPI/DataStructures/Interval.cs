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

namespace GeoAPI.DataStructures
{
    /// <summary> 
    /// Represents an (1-dimensional) closed interval on the Real number line (ℝ).
    /// </summary>
    public class Interval : IEquatable<Interval>, IComparable<Interval>,
                            IContainable<Interval>, IIntersectable<Interval>, IExpandable<Interval>
    {
        ///<summary>
        /// An empty interval [0, 0]
        ///</summary>
        public static readonly Interval Zero = new Interval(0, 0);
        /// <summary>
        /// An open interval ]NegativeInfinity, PositiveInfinity[
        /// </summary>
        public static readonly Interval Infinite = new Interval(Double.NegativeInfinity, Double.PositiveInfinity);

        private readonly Double _min;
        private readonly Double _max;

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="min">the lower bound of the interval</param>
        /// <param name="max">the upper bound of the interval</param>
        public Interval(Double min, Double max)
        {
            Normalize(ref min, ref max);

            _min = min;
            _max = max;
        }

        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="interval">the interval</param>
        public Interval(Interval interval)
        {
            _min = interval.Min;
            _max = interval.Max;
        }

        public override string ToString()
        {
            return String.Format("[{0} ... {1}]", Min, Max);
        }

        ///<summary>
        /// 
        ///</summary>
        public Double Center
        {
            get
            {
                return Equals(Infinite)
                    ? 0.0d
                    : (_min + _max) / 2d;
            }
        }

        /// <summary>
        /// Gets the lower bound of the interval
        /// </summary>
        public Double Min
        {
            get { return _min; }
        }

        /// <summary>
        /// Gets the upper bound of the interval
        /// </summary>
        public Double Max
        {
            get { return _max; }
        }

        /// <summary>
        /// Gets the width of the interval
        /// </summary>
        public Double Width
        {
            get { return Max - Min; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        public Interval ExpandToInclude(Interval interval)
        {
            double max = interval.Max > Max ? interval.Max : Max;

            double min = interval.Min < Min ? interval.Min : Min;

            return new Interval(min, max);
        }
        /*
        public Boolean Overlaps(Interval interval)
        {
            return Overlaps(interval.Min, interval.Max);
        }

        public Boolean Overlaps(Double min, Double max)
        {
            return Min <= max && Max >= min;
        }
        */
        public Boolean Intersects(Interval other)
        {
            return Intersects(other.Min, other.Max);
            //return Overlaps(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Boolean Intersects(Double min, Double max)
        {
            return Min <= max && Max >= min;
        }

        public Boolean Contains(Interval interval)
        {
            return Contains(interval.Min, interval.Max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public Boolean Contains(Double min, Double max)
        {
            return (min >= Min && max <= Max);
        }

        /// <summary>
        /// Checks whether a value is within the bounds of the interval
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>true if the value is within the bounds of this interval </returns>
        public Boolean Contains(Double value)
        {
            return (value >= Min && value <= Max);
        }

        private static void Normalize(ref Double min, ref Double max)
        {
            if (min <= max) return;
            Double temp = max;
            max = min;
            min = temp;
        }

        #region IEquatable<Interval> Members

        public Boolean Equals(Interval other)
        {
            return other._max == _max &&
                   other._min == _min;
        }

        #endregion

        #region IComparable<Interval> Members

        public Int32 CompareTo(Interval other)
        {
            Int32 maxCompare = _max.CompareTo(other._max);

            if (maxCompare != 0)
            {
                return maxCompare;
            }

            // The max on this interval is equal to the other interval's max,
            // so let's compare mins
            return _min.CompareTo(other._min);
        }

        #endregion

        #region operators
        public static bool operator==(Interval lhs, Interval rhs)
        {
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Interval lhs, Interval rhs)
        {
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return true;
            return !lhs.Equals(rhs);
        }

        #endregion
    }
}