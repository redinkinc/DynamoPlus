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

using System.Collections.Generic;
using System.Globalization;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus
{

    /// <summary>
    /// Shading Surfaces are building or objects which shadow the main building 
    /// </summary>
    public class ShadingSurface:AbsElement
    {
        private List<Point> PointList { get; set; }      
        private bool IsFixed { get; set; }

        /// <summary>
        /// Adds a shading surface by points and name
        /// </summary>
        /// <param name="points"></param>
        /// <param name="name"></param>
        /// <param name="Fixed">Defines if the ShadingSurface is oriented on the global (false) or relative (true) coordinate system</param>
        public ShadingSurface(List<Point> points, string name, bool Fixed = false)
        {
            PointList = points;
            Name = name;
            IsFixed = Fixed;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Writes the properties of the surface into one string
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override string Write()
        {
            string temp;
            if (IsFixed == false)
            {
                temp = "\nShading:Building:Detailed,\n";
            }
            else
            {
                temp = "\nShading:Site:Detailed,\n";

            }

            temp += Name + ",  !- Name\n";
            temp += ",                !- Shadowing Transmittance & Schedule \n";
            temp += PointList.Count + ",    !- Number of Vertices\n";

            for (var i = 0; i < PointList.Count; i++)
            {

                temp += PointList[i].X.ToString(CultureInfo.InvariantCulture) + ", " + PointList[i].Y.ToString(CultureInfo.InvariantCulture) +
                        ", " + PointList[i].Z.ToString(CultureInfo.InvariantCulture);

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

        //[FR] move to building
        /// <summary>
        /// Adds Shading Surfaces by a List of Dynamo Surfaces
        /// </summary>
        /// <param name="surfaces"></param>
        /// <returns></returns>
        public static List<ShadingSurface> ShadingSurfacesBySurfaceList(
            List<Surface> surfaces)
        {
            var i = 0;
            var shadingSurfaces = new List<ShadingSurface>();
            foreach (var revitSurface in surfaces)
            {
                var points = new List<Point>();
                foreach (var revitVertex in revitSurface.Vertices)
                {
                    //Converts Vertex to point, should go easier
                    var point = Point.ByCoordinates(revitVertex.PointGeometry.X/1000,
                        revitVertex.PointGeometry.Y/1000, revitVertex.PointGeometry.Z/1000);
                    points.Add(point);
                }
                var name = "ShadingSurface " + i;
                i++;
                var shadingSurface = new ShadingSurface(points, name);
                //points.Clear();
                shadingSurfaces.Add(shadingSurface);

            }
            return shadingSurfaces;
        }

    }
}