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
using System.Globalization;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus
{
    //Defines the zones 
    /// <summary>
    /// Defines a Zone inside a building
    /// </summary>
    public class Zone:AbsElement
    {
        private double Orientation { get; set; }
        private Point Origin { get; set; }
        private int Multiplier { get; set; }
        /// <summary>
        /// The surfaces within the zone
        /// </summary>
        public List<BuildingSurface> BuildingSurfaces { get; set; }
        /// <summary>
        /// Counter for surfaces
        /// </summary>
        public int SurfaceNumber { get; set; }

        /// <summary>
        /// Creates a Zone by a orientation, Origin and a building
        /// </summary>
        /// <param name="name"></param>
        /// <param name="orientation">The Orientation relative to the building</param>
        /// <param name="origin">The starting point of the zone</param>
        /// <param name="multiplier"></param>
        public Zone(string name, double orientation, Point origin, int multiplier = 1)        
        {
            Name = name;
            Orientation = orientation;
            Origin = origin;
            Multiplier = multiplier;
            BuildingSurfaces = new List<BuildingSurface>();
            SurfaceNumber = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="origin"></param>
        /// <param name="orientation"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static Zone ByNameOriginAndOrientation(string name, Point origin, double orientation)
        {
            var zone = new Zone(name, orientation, origin);
            return zone;
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
        /// Writes all Information of the Building object into a string. 
        /// </summary>
        /// <returns>All Building Information as string.</returns>
        [IsVisibleInDynamoLibrary(false)]
        public override string Write()
        {
            var temp = "Zone, ";
            temp += Name + ", ";
            temp += Orientation + ", ";
            temp += Origin.X.ToString(CultureInfo.InvariantCulture) + ", " + Origin.Y.ToString(CultureInfo.InvariantCulture) + ", " + Origin.Z.ToString(CultureInfo.InvariantCulture) + ", ";
            temp += "1, ";
            temp += Multiplier + ", ";
            temp += "autocalculate, ";
            temp += "autocalculate;\n";

            return temp;
        }
    }
}
