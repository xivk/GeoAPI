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
using GeoAPI.Coordinates;
using NPack.Interfaces;

namespace GeoAPI.Geometries
{
    /// <summary>
    /// <c>Geometry</c> classes support the concept of applying
    /// an <c>IGeometryComponentFilter</c> filter to the <c>Geometry</c>.
    /// The filter is applied to every component of the <c>Geometry</c>
    /// which is itself a <c>Geometry</c>.
    /// (For instance, all the LinearRings in Polygons are visited.)
    /// An <c>IGeometryComponentFilter</c> filter can either
    /// record information about the <c>Geometry</c>
    /// or change the <c>Geometry</c> in some way.
    /// <c>IGeometryComponentFilter</c> is an example of the Gang-of-Four Visitor pattern.
    /// </summary>    
    public interface IGeometryComponentFilter<TCoordinate>
        where TCoordinate : ICoordinate<TCoordinate>, IEquatable<TCoordinate>, 
                            IComparable<TCoordinate>, IConvertible, 
                            IComputable<Double, TCoordinate>
    {
        /// <summary>
        /// Performs an operation with or on <c>geom</c>.
        /// </summary>
        /// <param name="geom">A <c>Geometry</c> to which the filter is applied.</param>
        void Filter(IGeometry<TCoordinate> geom);
    }
}
