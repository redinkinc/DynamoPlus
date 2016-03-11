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

using System;
using System.Collections.Generic;
using System.Globalization;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus.Geometry

{
    //At the Moment to build Standard Windows, but easily extendable to other windows or doors, etc.
    /// <summary>
    /// Fenestration Surfaces are Openings like Windows ore doors
    /// </summary>
    public class FenestrationSurface:AbsElement
    {
        /// <summary>
        /// The EnergyPlus Element FenestrationSurface.
        /// </summary>
        public Surface Surface { get; set; }
        private string Type { get; set; }
        private string ConstructionName { get; set; }
        private string SurfaceName { get; set; }
        /// <summary>
        /// Surface where the FenestrationSurface is lying in
        /// </summary>
        public BuildingSurface BuildingSurface { get; set; }
        /// <summary>
        /// Corresponding Shading Overhang
        /// </summary>
        public ShadingOverhang ShadingOverhang { get; set; }

        private int Multiplier { get; set; }

        private FenestrationSurface(Surface surface, BuildingSurface buildingSurface, int multiplier, string constructionName = "default")
        {
            Name = buildingSurface.Name + " - Window " + buildingSurface.FenestrationSurfacesNumber;

            Type = "Window";

            ConstructionName = constructionName == "default"
                ? "000 Standard Window"
                : constructionName;

            SurfaceName = buildingSurface.Name;
            BuildingSurface = buildingSurface;
            Surface = surface;
            buildingSurface.FenestrationSurfacesNumber++;

            Multiplier = multiplier;
        }

        //Get 'Fenestration Surface by Selected Surface
        /// <summary>
        /// Adds a Fenestration Surface in a Surface
        /// </summary>
        /// <param name="surface">A Dynamo Surface</param>
        /// <param name="buildingSurface">The Dynamo BuildingSurface that the window is placed on.</param>
        /// <returns></returns>
        public static FenestrationSurface FenestrationSurfaceBySurface(Surface surface, BuildingSurface buildingSurface)
        {
            return new FenestrationSurface(surface, buildingSurface, 1);
        }

        //Get 'Fenestration Surface by Selected Surface
        /// <summary>
        /// Adds a Fenestration Surface in a Surface
        /// </summary>
        /// <param name="surface">A Dynamo Surface</param>
        /// <param name="buildingSurface">The Dynamo BuildingSurface that the window is placed on.</param>
        /// <param name="constructionName">The ConstructionName for the Window.</param>
        /// <param name="multiplier">The Multiplier for the window.</param>
        /// <returns></returns>
        public static FenestrationSurface FenestrationSurfaceBySurface(Surface surface, BuildingSurface buildingSurface, string constructionName, int multiplier)
        {
            return new FenestrationSurface(surface, buildingSurface, multiplier, constructionName);
        }

        /// <summary>
        /// Adds EnergyPlus Windows (FenestrationSurface) to a list of rectangular EnergyPlus Surfaces by a given percentage.
        /// </summary>
        /// <param name="buildingSurfaces">A list of BuildingSurfaces where the windows should be added to.</param>
        /// <param name="percentage">The glazing percentage (factor). Has to be between 0.05 and 0.95!</param>
        /// <returns></returns>
        public static List<FenestrationSurface> ByGlazingPercentage(List<BuildingSurface> buildingSurfaces, double percentage = 0.5)
        {
            // check for the right percentage value and throw an exeption if fails
            if (percentage < 0.05 || percentage > 0.95)
            {
                throw new ArgumentException("The glazing percentage has to be between 0.05 and 0.95");
            }

            var fenestrationsurfaceList = new List<FenestrationSurface>();

            // iterate over the surfaces
            foreach (var buildingSurface in buildingSurfaces)
            {
                
                var vec1 = Vector.ByTwoPoints(buildingSurface.Surface.Vertices[0].PointGeometry, buildingSurface.Surface.Vertices[1].PointGeometry);
                var vec2 = Vector.ByTwoPoints(buildingSurface.Surface.Vertices[0].PointGeometry, buildingSurface.Surface.Vertices[3].PointGeometry);
                //calculate the distance from the border (offset) from the given Surface
                var length = vec1.Length;
                var height = vec2.Length;
                var dist = ((height + length) - Math.Sqrt(Math.Pow(height, 2) + Math.Pow(length, 2) + (4*percentage - 2) * height * length))/4;

                //calculate new points for the FenestrationSurface
                var points = new List<Point>();
                var coordSystem = CoordinateSystem.ByOriginVectors(buildingSurface.Surface.Vertices[0].PointGeometry, vec1, vec2);
                points.Add(Point.ByCartesianCoordinates(coordSystem, dist, dist));
                points.Add(Point.ByCartesianCoordinates(coordSystem, length - dist, dist));
                points.Add(Point.ByCartesianCoordinates(coordSystem, length - dist, height-dist));
                points.Add(Point.ByCartesianCoordinates(coordSystem, dist, height - dist));

                var surface = Surface.ByPerimeterPoints(points);

                //add to List
                fenestrationsurfaceList.Add(new FenestrationSurface(surface, buildingSurface, 1));
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
        /// <returns>The String to be added in the final idf File.</returns>
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
            temp += Surface.Vertices.Length + ",              !- Number of Vertices\n";
            for (var i = 0; i < Surface.Vertices.Length; i++)
            {
                temp += Surface.Vertices[i].PointGeometry.X.ToString(CultureInfo.InvariantCulture) + ", " + Surface.Vertices[i].PointGeometry.Y.ToString(CultureInfo.InvariantCulture) + ", " + Surface.Vertices[i].PointGeometry.Z.ToString(CultureInfo.InvariantCulture);

                if (i != Surface.Vertices.Length - 1)
                {
                    temp += $", !- XYZ Coordinates Point {i + 1}\n";
                }
                else
                {
                    temp += $"; !- XYZ Coordinates Point {i + 1}\n";
                }
            }

            return temp;
        }
    }
}
