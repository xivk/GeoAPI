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
using GeoAPI.CoordinateSystems;
using GeoAPI.Geometries.Prepared;
using GeoAPI.Indexing;
using NPack;
using NPack.Interfaces;

namespace GeoAPI.Geometries
{
    public interface IGeometry<TCoordinate> : IGeometry, IBoundable<IExtents<TCoordinate>>,
                                              ISpatialOperator<TCoordinate>,
                                              ISpatialRelation<TCoordinate>,
                                              IComparable<IGeometry<TCoordinate>>,
                                              IVertexStream<TCoordinate, DoubleComponent>
        where TCoordinate : ICoordinate<TCoordinate>, IEquatable<TCoordinate>, IComparable<TCoordinate>,
                            IComputable<Double, TCoordinate>, IConvertible
    {
        //void Apply(ICoordinateFilter<TCoordinate> filter);
        //void Apply(IGeometryFilter<TCoordinate> filter);
        //void Apply(IGeometryComponentFilter<TCoordinate> filter);
        //IPoint<TCoordinate> InteriorPoint { get; }
        new IPoint<TCoordinate> PointOnSurface { get; }

        new IPoint<TCoordinate> Centroid { get; }

        new IGeometry<TCoordinate> Clone();

        new ICoordinateSequence<TCoordinate> Coordinates { get; }

        new IGeometry<TCoordinate> Envelope { get; }

        new IExtents<TCoordinate> Extents { get; }

        new IGeometryFactory<TCoordinate> Factory { get; }

        new IPrecisionModel<TCoordinate> PrecisionModel { get; }

        IGeometry<TCoordinate> Project(ICoordinateSystem<TCoordinate> toCoordinateSystem);

        /// <summary>
        /// Gets the spatial reference system associated with the geometry.
        /// </summary>
        new ICoordinateSystem<TCoordinate> SpatialReference { get; }

        /// <summary>
        /// Function to return a prepared geometry made of this geometry
        /// </summary>
        /// <returns>The prepared geometry</returns>
        new IPreparedGeometry<TCoordinate> ToPreparedGeometry();
    }
}