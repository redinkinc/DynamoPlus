/*
 *  This file is part of DynamoPlus.
 *  
 *  Copyright (c) 2014-2015 Technische Universitaet Muenchen, 
 *  Chair of Computational Modeling and Simulation (https://www.cms.bgu.tum.de/)
 *  LEONHARD OBERMEYER CENTER (http://www.loc.tum.de)
 *  
 *  Developed by Fabian Ritter (mailto:mail@redinkinc.de), Florian Englberger
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
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus

{
    /// <summary>
    /// 
    /// </summary>
    public class ZoneList:AbsElement
    {
        /// <summary>
        /// The Zones Contained in the ZoneList
        /// </summary>
        public List<Zone> Zones { get; set; }

        //<
        /// <summary>
        /// A EnergyPlus ZoneList Element.
        /// </summary>
        /// <param name="name">Name of the ZoneList</param>
        /// <param name="zones">Zones to be added to ZoneList</param>
        public ZoneList(string name, List<Zone> zones)
        {
            Name = name;
            Zones = zones;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>the name of zonelist and number of zones</returns>
        public override string ToString()
        {
            return $"Zonelist: {Name}\n Zones: {Zones.Count}";
        }

        /// <summary>
        /// Writes the properties of the Surface into one string
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override string Write()
        {
            var temp = "Zonelist,\n";
            temp += Name;
            foreach (var zone in Zones)
            {
                temp += ",\n" + zone.Name;
            }
            temp += ";\n";
            return temp;
        }
    }
}
