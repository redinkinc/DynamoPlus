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
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus.Geometry
{
    
    /// <summary>
    /// BuildingSurfaces are surfaces of the main building like walls, floors etc.
    /// </summary>
    public class BuildingSurface:AbsElement
    {
        /// <summary>
        /// The Surface Geometry representing the BuildingSurface
        /// </summary>
        public Surface Surface { get; set; }
        /// <summary>
        /// The Type of the BuildingSurface
        /// </summary>
        private string Type { get; set; }
        /// <summary>
        /// The Construction of the BuildingSurface
        /// </summary>
        private string ConstructionName { get; set; }
        private string ZoneName { get; set; }
        /// <summary>
        /// The Outside Boundary Condition of the BuildingSurface
        /// </summary>
        private string BoundaryCondition { get; set; }
        /// <summary>
        /// The Outside Boundary Condition Object of the BuildingSurface
        /// </summary>
        private string BoundaryObject { get; set; }
        
        /// <summary>
        /// Counts the FenestrationSurfaces related to the Surface. Each Surface counts as one FensestrationsSurfaces
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public int FenestrationSurfacesNumber { get; set; }
        /// <summary>
        /// True if BuildingSurface is SunExposed
        /// </summary>
        private bool SunExposed;
        /// <summary>
        /// True if BuildingSurface is WindExposed
        /// </summary>
        private bool WindExposed;

        /// <summary>
        /// Adds a Surface by a List of Points
        /// </summary>
        /// <param name="surface">Dynamo Surface that represents the Building Surface</param>
        /// <param name="zone">Zone of the BuildingSurface</param>
        /// <param name="name">Name of the BuildingSurface. If "default" it will be named by Zone and a counter (like "Zonename - Surface 1").</param>
        /// <param name="constructionName">Defines the construction of the Surface</param>
        /// <param name="boundaryCondition"></param>
        /// <param name="type">sets the type of the construction [external, internal, roof, ceiling, floor]</param>
        [IsVisibleInDynamoLibrary(false)]
        public BuildingSurface(Surface surface, Zone zone, string name = "default", string type = "Wall", string constructionName = "default", string boundaryCondition = "Outdoors")
        {
            FenestrationSurfacesNumber = 1;

            if (name == "default")
            {
                Name = zone.Name + " - Surface " + zone.SurfaceNumber;
            }
            else
            {
                Name = name;
            }

            Surface = surface;
            ZoneName = zone.Name;
            zone.BuildingSurfaces.Add(this);
            zone.SurfaceNumber++;

            Type = type;
            ConstructionName = constructionName == "default" ? GetDefault(type, boundaryCondition) : constructionName;
            BoundaryCondition = boundaryCondition;
            BoundaryObject = "";

            if (boundaryCondition == "Outdoors")
            {
                SunExposed = true;
                WindExposed = true;
            }
            else
            {
                SunExposed = false;
                WindExposed = false;
            }
        }

        /// <summary>
        /// Creates an DesignScript.Geometry.Surface from a pointlist and returns the DynamoPlus.BuildingSurface.
        /// </summary>
        /// <param name="points">List of points</param>
        /// <param name="zone">the zone</param>
        /// <param name="name">Name of the BuildingSurface. If "default" it will be named by Zone and a counter (like "Zonename - Surface 1").</param>
        /// <param name="constructionName">a construction name</param>
        /// <param name="boundaryCondition">the OutsideBoundaryCondition</param>
        /// <param name="type">The Type of the BuildingSurface</param>
        /// <returns>A DynamoPlus BuildingSurface</returns>
        public BuildingSurface ByPoints(List<Point> points, Zone zone, string name = "default",
            string constructionName = "default", string boundaryCondition = "Outdoors", string type = "Wall")
        {
            var surf = Surface.ByPerimeterPoints(points);
            return new BuildingSurface(surf, zone, name, constructionName, boundaryCondition, type);
        }
        
        private string GetDefault(string type, string boundaryCondition)
        {
            string construction;
            if (type == "Wall" && boundaryCondition != "Outdoors")
            {
                construction = "000 Interior Wall";
            }
            else if (type == "Wall" && boundaryCondition == "Outdoors")
            { 
                construction = "001 Exterior Wall";
            }
            else if (type == "Floor")
            {
                construction = "002 Floor";
            }
            else if (type == "Roof")
            {
                construction = "003 Roof";
            }
            else if (type == "Ceiling")
            {
                construction = "004 Ceiling";
            }
            else
            {
                construction = "001 Exterior Wall";
            }
            return construction;
        }

        /// <summary>
        /// Sets the BuildingSurface in Relation to another one (BoundaryCondition is set to "Surface").
        /// </summary>
        /// <param name="otherBuildingSurface">The related BuildingSurface</param>
        public void SetRelation(BuildingSurface otherBuildingSurface)
        {
            BoundaryCondition = "Surface";
            BoundaryObject = otherBuildingSurface.Name;
            SunExposed = false;
            WindExposed = false;

            switch (Type)
            {
                case "Roof":
                    Type = "Ceiling";
                    ConstructionName = GetDefault(Type, "Surface");
                    break;
                case "Wall":
                    ConstructionName = GetDefault(Type, "Surface");
                    break;
                default:
                    ConstructionName = GetDefault(Type, "Surface");
                    break;
            }
        }

        /// <summary>
        /// Returns the BuildingSurface Parameter
        /// </summary>
        /// <returns>The BuildingSurface Parameter</returns>
        [MultiReturn(new[] { "Surface", "Type", "Construction Name", "Zone Name", "Outside Boundary Condition", "SunExposed", "WindExposed" })]
        public Dictionary<string, object> Inspect()
        {
            return new Dictionary<string, object>
            {
                { "Surface", Surface },
                { "Type", Type },
                { "Construction Name", ConstructionName },
                { "Zone Name", ZoneName },
                { "Outside Boundary Condition", BoundaryCondition },
                { "Outside Boundary Object", BoundaryObject },
                { "SunExposed", SunExposed },
                { "WindExposed", WindExposed }
                //{ "View Factor to Ground", viewFactorToGround }
            };
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
        /// Writes the properties of the Surface into one string
        /// </summary>
        /// <returns>BuildingSurface:Detailed as String.</returns>
        [IsVisibleInDynamoLibrary(false)]
        public override string Write()
        {
            var temp = "\nBuildingSurface:Detailed,\n";
            temp += Name + ",  !Name\n";
            temp += Type + ",  !Surface Type\n";
            temp += ConstructionName + ",  !Construction Name\n";
            temp += ZoneName + ",  !Zone Name\n";
            temp += BoundaryCondition + ", !Outside Boundary Condition\n";
            temp += BoundaryObject + ",    !- Outside Boundary Condition Object\n";
            temp +=  (SunExposed) ? "SunExposed" : "NoSun";
            temp += ",    !- Sun Exposure\n";
            temp += (WindExposed) ? "WindExposed" : "NoWind";
            temp += ",   !- Wind Exposure\n";
            temp += ",   !- View Factor to Ground\n";
            temp += Surface.Vertices.Length + ",              !- Number of Vertices\n";
            for (var i = 0; i < Surface.Vertices.Length ; i++)
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
