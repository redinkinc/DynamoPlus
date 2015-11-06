/*
 *  This file is part of DynamoPlus.
 *  
 *  Copyright (c) 2014-2015 Technische Universitaet Muenchen, 
 *  Chair of Computational Modeling and Simulation (https://www.cms.bgu.tum.de/)
 *  LEONHARD OBERMEYER CENTER (http://www.loc.tum.de)
 *  
 *  Developed by Fabian Ritter (contact: mailto:mail@redinkinc.de) and Florian Englberger
 * 
 *  DynamoPlus is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *  
 *  DynamoPlus is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  
 *  You should have received a copy of the GNU General Public License
 *  along with DynamoPlus. If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;
using ProtoCore.Lang;

namespace DynamoPlus
{
    /// <summary>
    /// Additional Functions for creating DynamoPlus Elemtents.
    /// </summary>
    public static class Create
    {
        /// <summary>
        /// Creates Zones from geometry input.
        /// </summary>
        /// <returns>All Elements from the geometry.</returns>
        [IsVisibleInDynamoLibrary(false)] //not working yet, so hidden in library...
        public static Elements FromGeometry(List<Geometry> geometry)
        {
            var elements = new Elements();
            var surfaces = new List<List<Surface>>();
            foreach (var geom in geometry)
            {
                //var top = new Topology( , );
                //var faces = top.Faces(geom);
            }

            return elements;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geometrySurfaces"></param>
        /// <returns></returns>
        [MultiReturn(new[] {"Zones", "BuildingSurfaces"})]
        public static Dictionary<string, object> FromSurfaces(List<List<Surface>> geometrySurfaces)
        {
            //var elements = new Elements();
            var zones = new List<Zone>();
            var buildingSurfaces = new List<BuildingSurface>();
            var counter = 1;

            foreach (var geometry in geometrySurfaces)
            {
                var bb = BoundingBox.ByGeometry(geometry);
                var orientation = bb.ContextCoordinateSystem.XAxis.AngleBetween(Vector.ByCoordinates(1, 0, 0));
                var zone = new Zone("Zone" + counter, orientation, bb.MinPoint);
                counter++;

                zones.Add(zone);

                var p = bb.MinPoint.Add(bb.MaxPoint.AsVector());
                var centerpoint = Point.ByCoordinates(p.X/2, p.Y/2, p.Z/2);

                foreach (var surface in geometry)
                {
                    if (surface.NormalAtParameter(0.5, 0.5).AngleBetween(Vector.ByCoordinates(0, 0, 1)) < 0.01)
                    {
                        buildingSurfaces.Add(surface.PointAtParameter(0.5, 0.5).Z > centerpoint.Z
                            ? new BuildingSurface(surface, zone, "default", "Roof")
                            : new BuildingSurface(surface, zone, "default", "Floor", "default", "Ground"));
                    }
                    else
                    {
                        buildingSurfaces.Add(new BuildingSurface(surface, zone));
                    }
                }

                //Not working as intended...
                //foreach (var surface in geometry)
                //{
                //    if (GetNormal(surface).IsAlmostEqualTo(Vector.ByCoordinates(0, 0, 1)))
                //    {
                //        buildingSurfaces.Add(new BuildingSurface(surface, zone, "default", "Roof"));
                //    }
                //    else if (GetNormal(surface).IsAlmostEqualTo(Vector.ByCoordinates(0, 0, -1)))
                //    {
                //        buildingSurfaces.Add(new BuildingSurface(surface, zone, "default", "Floor"));
                //    }
                //    else
                //    {
                //        buildingSurfaces.Add(new BuildingSurface(surface, zone));
                //    }
                //}
            }

            //elements.AddZones(zones);
            //elements.AddBuildingSurfaces(buildingSurfaces);
            //return elements; // elements seems to be better, but for further editing zones and buildingsurfaces are retruned seperately...

            return new Dictionary<string, object>
            {
                { "Zones", zones },
                { "BuildingSurfaces", buildingSurfaces }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buildingSurfaces"></param>
        /// <returns></returns>
        public static List<BuildingSurface> FindCorelatingBuildingSurfaces(List<BuildingSurface> buildingSurfaces)
        {
            for (int i = 0; i < buildingSurfaces.Count; i++)
            {
                for (int j = buildingSurfaces.Count - 1; j > i; j--)
                {
                    if (buildingSurfaces[i].Surface.IsAlmostEqualTo(buildingSurfaces[j].Surface))
                    {
                        buildingSurfaces[j].SetRelation(buildingSurfaces[i]);
                        buildingSurfaces[i].SetRelation(buildingSurfaces[j]);
                    }
                }
            }

            return buildingSurfaces;
        }

        private static Vector GetNormal(Surface surface)
        {
            var p1 = surface.Vertices[0].PointGeometry;
            var p2 = surface.Vertices[1].PointGeometry;
            var p3 = surface.Vertices[2].PointGeometry;
            var normal = Vector.ByTwoPoints(p1, p2).Cross(Vector.ByTwoPoints(p2, p3));

            return normal;
        }
    }
}
