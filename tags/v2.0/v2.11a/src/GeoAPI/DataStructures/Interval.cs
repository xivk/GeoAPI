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
    public struct Interval : IEquatable<Interval>, IComparable<Interval>,
                             IContainable<Interval>, IIntersectable<Interval>, IExpandable<Interval>
    {
        public static readonly Interval Zero = new Interval(0, 0);
        public static readonly Interval Infinite = new Interval(Double.NegativeInfinity, Double.PositiveInfinity);

        private readonly Double _min;
        private readonly Double _max;

        public Interval(Double min, Double max)
        {
            normalize(ref min, ref max);

            _min = min;
            _max = max;
        }

        public Interval(Interval interval)
        {
            _min = interval.Min;
            _max = interval.Max;
        }

        public override string ToString()
        {
            return String.Format("[{0} ... {1}]", Min, Max);
        }

        public Double Center
        {
            get
            {
                return Equals(Infinite)
                    ? 0.0
                    : (_min + _max) / 2;
            }
        }

        public Double Min
        {
            get { return _min; }
        }

        public Double Max
        {
            get { return _max; }
        }

        public Double Width
        {
            get { return Max - Min; }
        }

        public Interval ExpandToInclude(Interval interval)
        {
            double max = interval.Max > Max ? interval.Max : Max;

            double min = interval.Min < Min ? interval.Min : Min;

            return new Interval(min, max);
        }

        public Boolean Overlaps(Interval interval)
        {
            return Overlaps(interval.Min, interval.Max);
        }

        public Boolean Overlaps(Double min, Double max)
        {
            return Min <= max && Max >= min;
        }

        public Boolean Intersects(Interval other)
        {
            return Overlaps(other);
        }

        public Boolean Contains(Interval interval)
        {
            return Contains(interval.Min, interval.Max);
        }

        public Boolean Contains(Double min, Double max)
        {
            return (min >= Min && max <= Max);
        }

        public Boolean Contains(Double value)
        {
            return (value >= Min && value <= Max);
        }

        private static void normalize(ref Double min, ref Double max)
        {
            if (min > max)
            {
                Double temp = max;
                max = min;
                min = temp;
            }
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
    }
}