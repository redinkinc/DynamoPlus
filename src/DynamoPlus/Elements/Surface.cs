/*
 *  This file is part of DynamoPlus.
 *  
 *  Copyright (c) 2014-2015 Technische Universitaet Muenchen, 
 *  Chair of Computational Modeling and Simulation (https://www.cms.bgu.tum.de/)
 *  LEONHARD OBERMEYER CENTER (www.loc.tum.de)
 *  
 *  Developed by Fabian Ritter (Contact: mailto:mail@redinkinc.de) and Florian Englberger
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
using System.Linq;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus
{
    
    /// <summary>
    /// Surfaces are surfaces of the main building like walls floors etc.
    /// </summary>
    public class Surface:AbsElement
    {

        /// <summary>
        /// 
        /// </summary>
        public List<Point> Points { get; set; }
        private string Type { get; set; }
        private string ConstructionName { get; set; }
        private string ZoneName { get; set; }
        private string BoundaryCondition { get; set; }
        //Needed to turn the wall around if necessary
        /// <summary>
        /// Possibility to turn the surface if the normal vector points into the wrong direction
        /// </summary>
        private bool ReverseDirection { get; set; }
        
        /// <summary>
        /// Counts the FenestrationSurfaces related to the surface. Each Surface counts its one FensestrationsSurfaces
        /// </summary>
        public int FenestrationSurfacesNumber { get; set; }
        private bool SunExposed;
        private bool WindExposed;

        /// <summary>
        /// Adds a surface by a List of Points
        /// </summary>
        /// <param name="points"></param>
        /// <param name="zone"></param>
        /// <param name="constructionName">Defines the construction of the Surface</param>
        /// <param name="usingType">sets the type of the construction [external, internal, roof, ceiling, floor]</param>
        /// <param name="reverseDirection">Possibility to turn the wall by 180 degree</param>
        [IsVisibleInDynamoLibrary(false)]
        public Surface(List<Point> points, Zone zone, string constructionName = "default", string usingType = "external", bool reverseDirection = false)
        {
            Name = zone.Name + " - Surface " + zone.SurfaceNumber;
            ZoneName = zone.Name;
            ReverseDirection = reverseDirection;
            FenestrationSurfacesNumber = 1;
            Points = points;
            ConstructionName = constructionName;
            zone.Surfaces.Add(this);
            zone.SurfaceNumber++;

            //Defines the properties related to the type of the surface. Perhaps a possibility to change the propertiers should be implemented
            switch (usingType)
            {
                case "inner":
                    Type = "Wall";
                    if (ConstructionName == "default")
                    { 
                        ConstructionName = "000 Interior Wall"; 
                    }
                    BoundaryCondition = "Adiabatic";
                    SunExposed = false;
                    WindExposed = false;
                    break;
                case "external":
                    Type = "Wall";
                    if (ConstructionName == "default")
                    { 
                        ConstructionName = "001 Exterior Wall"; 
                    }
                    BoundaryCondition = "Outdoors";
                    SunExposed = true;
                    WindExposed = true;
                    break;
                case "floor":
                    Type = "Floor";
                    if (ConstructionName == "default")
                    { 
                        ConstructionName = "002 Floor"; 
                    }
                    BoundaryCondition = "Ground";
                    SunExposed = false;
                    WindExposed = false;
                    break;
                case "ceiling":
                    Type = "Ceiling";
                    if (ConstructionName == "default")
                    { 
                        ConstructionName = "002 Floor"; 
                    }
                    BoundaryCondition = "Adiabatic";
                    SunExposed = false;
                    WindExposed = false;
                    break;
                case "roof":
                    Type = "Roof";
                    if (ConstructionName == "default")
                    { 
                        ConstructionName = "003 Roof";
                    }
                    BoundaryCondition = "Outdoors";
                    SunExposed = true;
                    WindExposed = true;
                    break;
                default:
                    Type = "Wall";
                    if (ConstructionName == "default")
                    { 
                        ConstructionName = "000 Exterior Wall";
                    }
                    BoundaryCondition = "Outdoors";
                    SunExposed = true;
                    WindExposed = true;
                    break;
            }

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
         
        // Retruns BuildingSurface:Detailed, http://bigladdersoftware.com/epx/docs/8-0/input-output-reference/page-011.html#buildingsurfacedetailed.
        /// <summary>
        /// Writes the properties of the surface into one string
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override string Write()
        {
            var temp = "\nBuildingSurface:Detailed,\n";
            temp += Name + ",  !Name\n";
            temp += Type + ",  !Surface Type\n";
            temp += ConstructionName + ",  !Construction Name\n";
            temp += ZoneName + ",  !Zone Name\n";
            temp += BoundaryCondition + ", !Outside Boundary Condition\n";
            temp += ",              !- Outside Boundary Condition Object\n";
            temp +=  (SunExposed) ? "SunExposed" : "NoSun";
            temp += ",    !- Sun Exposure\n";
            temp += (WindExposed) ? "WindExposed" : "NoWind";
            temp += ",   !- Wind Exposure\n";
            temp += ",              !- View Factor to Ground\n";
            temp += Points.Count + ",              !- Number of Vertices\n";
            for (var i = 0; i < Points.Count; i++)
            {
                temp += Points[i].X.ToString(CultureInfo.InvariantCulture) + ", " + Points[i].Y.ToString(CultureInfo.InvariantCulture) + ", " + Points[i].Z.ToString(CultureInfo.InvariantCulture);

                if (i != Points.Count - 1)
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
        // Draws the surface in Dynamo
        //[FR] has to be moved into a geometry handler
        /// <summary>
        /// Draws the surfaces in Dynamo
        /// </summary>
        /// <param name="surfaces"></param>
        /// <returns></returns>
        public static List<PolyCurve> DrawSurfaces(List<Surface> surfaces)
        {
            var rectangles = new List<PolyCurve>();

            foreach (var surface in surfaces)
            {
                var pointList = surface.Points.Select(p => Point.ByCoordinates(p.X, p.Y, p.Z)).ToList();
                rectangles.Add(PolyCurve.ByPoints(pointList, true));
            }

            return rectangles;
        }

        /// <summary>
        /// Adds Surfaces by a list of Revit Surfaces
        /// </summary>
        /// <param name="facelist"></param>
        /// <param name="zone"></param>
        /// <param name="constructionName"></param>
        /// <param name="boundaryCondition"></param>
        /// <returns></returns>
        public static List<Surface> SurfaceBySurfaceList (List<Face> facelist, Zone zone, string constructionName = "default", string boundaryCondition="identical")
        {
            var surfaceList = new List<Surface>();

            //ConstructionName = "default";
            foreach (Face face in facelist)
            {
                var points = new List<Point>();

                foreach (var revitVertex in face.Vertices)
                {
                    //Translate Vertex to Point. Can't find a way to avoid this. Deviding through 1000 because of differents units(Revit->EPlus)
                    //Don't know if this is right
                    var point = Point.ByCoordinates(revitVertex.PointGeometry.X/1000,
                        revitVertex.PointGeometry.Y/1000, revitVertex.PointGeometry.Z/1000);
                    points.Add(point);
                }
                surfaceList.Add(new Surface(points, zone, constructionName, boundaryCondition));
            }
          
           return surfaceList;
        }

        
    }
}
