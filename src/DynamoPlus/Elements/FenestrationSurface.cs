/*
 *  This file is part of DynamoPlus.
 *  
 *  Copyright (c) 2014-2015 Technische Universitaet Muenchen, 
 *  Chair of Computational Modeling and Simulation (https://www.cms.bgu.tum.de/)
 *  LEONHARD OBERMEYER CENTER (www.loc.tum.de)
 *  
 *  Developed by Fabian Ritter, Florian Englberger
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus

{
    //At the Moment to build Standard Windows, but easily extendable to other windows or doors, etc.
    /// <summary>
    /// Fenestration Surfaces are Openings like Windows ore doors
    /// </summary>
    public class FenestrationSurface:AbsElement
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Point> PointList { get; set; }
        private string Type { get; set; }
        private string ConstructionName { get; set; }
        private string SurfaceName { get; set; }
        /// <summary>
        /// Surface where the FenestrationSurface is lying in
        /// </summary>
        public Surface Surface { get; set; }
        /// <summary>
        /// Corresponding Shading Overhang
        /// </summary>
        public ShadingOverhang ShadingOverhang { get; set; }

        private FenestrationSurface(Surface surface, List<Point> points)
        {
            Name = surface.Name + " - Window " + surface.FenestrationSurfacesNumber;

            Type = "Window";
            ConstructionName = "000 Standard Window";
            SurfaceName = surface.Name;
            Surface = surface;
            PointList = points;
            //CheckAngle(surface);
            surface.FenestrationSurfacesNumber++;
            
        }

        //Get 'Fenestration Surface by Selected Surface
        /// <summary>
        /// Adds a Fenestration Surface in a Surface
        /// </summary>
        /// <param name="facelist"></param>
        /// <param name="surface"></param>
        /// <returns></returns>
        public static List<FenestrationSurface> FenestrationSurfaceBySurfacelist (List<Face> facelist, Surface surface)
        {
            return facelist.Select(face => face.Vertices.Select(
                revitVertex => Point.ByCoordinates(revitVertex.PointGeometry.X/1000, revitVertex.PointGeometry.Y/1000, revitVertex.PointGeometry.Z/1000)).ToList()).
                Select(points => new FenestrationSurface(surface, points)).ToList();
        }

        /// <summary>
        /// Adds EnergyPlus Windows (FenestrationSurface) to a list of rectangular EnergyPlus Surfaces by a given percentage.
        /// </summary>
        /// <param name="surfaces">A list of Faces where the windows should be added to.</param>
        /// <param name="percentage">The glazing percentage (factor). Has to be between 0.05 and 0.95!</param>
        /// <returns></returns>
        public static List<FenestrationSurface> ByGlazingPercentage(List<Surface> surfaces, double percentage = 0.5)
        {
            // check for the right percentage value and throw an exeption if fails
            if (percentage < 0.05 || percentage > 0.95)
            {
                throw new ArgumentException("The glazing percentage has to be between 0.05 and 0.95");
            }

            var fenestrationsurfaceList = new List<FenestrationSurface>();

            // iterate over the surfaces
            foreach (var surface in surfaces)
            {
                
                var vec1 = Vector.ByTwoPoints(surface.Points[0], surface.Points[1]);
                var vec2 = Vector.ByTwoPoints(surface.Points[0], surface.Points[3]);
                //calculate the distance from the border (offset) from the given surface
                var length = vec1.Length;
                var height = vec2.Length;
                var dist = ((height + length) - Math.Sqrt(Math.Pow(height, 2) + Math.Pow(length, 2) + (4*percentage - 2) * height * length))/4;

                //calculate new points for the FenestrationSurface
                var points = new List<Point>();
                var coordSystem = CoordinateSystem.ByOriginVectors(surface.Points[0], vec1, vec2);
                points.Add(Point.ByCartesianCoordinates(coordSystem, dist, dist));
                points.Add(Point.ByCartesianCoordinates(coordSystem, length - dist, dist));
                points.Add(Point.ByCartesianCoordinates(coordSystem, length - dist, height-dist));
                points.Add(Point.ByCartesianCoordinates(coordSystem, dist, height - dist));

                //add to List
                fenestrationsurfaceList.Add(new FenestrationSurface(surface, points));
            }
            //return List
            return fenestrationsurfaceList;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString() => Name;

        /// <summary>
        /// Writes the data into one string
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override string Write()
        {
            var temp = "\nFenestrationSurface:Detailed,\n";
            temp += Name + ",  !Name\n";
            temp += Type + ",  !Surface Type\n";
            temp += ConstructionName + ",  !Construction Name\n";
            temp += SurfaceName + ",  !Building Surface Name\n";
            temp += ",        !- Outside Boundary Condition Object\n";
            temp += ",        !- View Factor to Ground\n";
            temp += ",        !- Shading Control Name\n";
            temp += ",        !- Frame and Divider Name\n";
            temp += ",        !- Multiplier\n";
            temp += ",        !- Number of Vertices\n";
            for (var i = 0; i < PointList.Count; i++)
            {
                temp += PointList[i].X.ToString(CultureInfo.InvariantCulture) + ", " + PointList[i].Y.ToString(CultureInfo.InvariantCulture) + ", " + PointList[i].Z.ToString(CultureInfo.InvariantCulture);
                if (i != PointList.Count - 1)
                {
                    temp += ", ";
                }
                else
                {
                    temp += ";\n";
                }
            }

            return temp;
        }
    }
}
