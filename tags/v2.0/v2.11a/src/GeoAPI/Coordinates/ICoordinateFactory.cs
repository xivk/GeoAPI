﻿// Portions copyright 2005 - 2007: Diego Guidi
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
using NPack;
using NPack.Interfaces;

namespace GeoAPI.Coordinates
{
    public interface ICoordinateFactory
    {
        ICoordinate Create(Double x, Double y);
        ICoordinate Create(Double x, Double y, Double m);
        ICoordinate Create(Double x, Double y, Double val, bool valIsM);
        ICoordinate Create(params Double[] coordinates);
        ICoordinate Create(IVector<DoubleComponent> coordinate);
        ICoordinate Create3D(Double x, Double y, Double z);
        ICoordinate Create3D(Double x, Double y, Double z, Double m);
        ICoordinate Create3D(Double x, Double y, Double z, Double val, bool valIsM);
        ICoordinate Create3D(params Double[] coordinates);

        IAffineTransformMatrix<DoubleComponent> CreateTransform(ICoordinate scaleVector, 
                                                                ICoordinate rotationAxis,
                                                                Double rotation, 
                                                                ICoordinate translateVector);
        ICoordinate Homogenize(ICoordinate coordinate);
        ICoordinate Dehomogenize(ICoordinate coordinate);
        IPrecisionModel PrecisionModel { get; }
        IPrecisionModel CreatePrecisionModel(PrecisionModelType type);
        IPrecisionModel CreatePrecisionModel(Double scale);
    }
}
