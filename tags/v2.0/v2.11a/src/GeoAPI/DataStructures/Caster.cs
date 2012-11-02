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
using System.Collections;
using System.Collections.Generic;

namespace GeoAPI.DataStructures
{
    public static class Caster
    {
        public static IEnumerable<T> Upcast<T, U>(IEnumerable<U> items)
            where U : T
        {
            if (items == null)
                yield break;

            foreach (U item in items)
            {
                yield return item;
            }
        }

        public static IEnumerable<T> Downcast<T, U>(IEnumerable<U> items)
            where T : U
        {

            if (items == null)
                yield break;

            foreach (T item in items)
            {
                yield return item;
            }
        }

        public static IEnumerable<T> Cast<T>(IEnumerable enumerable)
        {

            if (enumerable == null)
                yield break;

            foreach (T item in enumerable)
            {
                yield return item;
            }
        }

        public static IEnumerable<T> CastNoNulls<T>(IEnumerable enumerable)
        {
            if (enumerable == null)
                yield break;

            foreach (Object item in enumerable)
            {
                if (item is T)
                {
                    yield return (T)item;
                }
            }
        }
    }
}
