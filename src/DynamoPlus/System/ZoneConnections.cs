/*
 *  This file is part of DynamoPlus.
 *  
 *  Copyright (c) 2014-2015 Technische Universitaet Muenchen, 
 *  Chair of Computational Modeling and Simulation (https://www.cms.bgu.tum.de/)
 *  LEONHARD OBERMEYER CENTER (www.loc.tum.de)
 *  
 *  Developed by Fabian Ritter (Contact mailto:mail@redinkinc.de) and Florian Englberger
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

using Autodesk.DesignScript.Runtime;
using DynamoPlus.Geometry;

namespace DynamoPlus.System
{
    /// <summary>
    /// 
    /// </summary>
    public class ZoneConnections
    {
        private string ZoneName { get; set; }
        private string EquipmentList { get; set; }
        private string InletNode { get; set; }
        private string AirExhaustNode { get; set; }
        private string AirNodeName { get; set; }
        private string ReturnAirNodeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        public ZoneConnections(Zone zone)
        {
            ZoneName = zone.Name;
            EquipmentList = zone.Name + " Equipment";
            InletNode = zone.Name + " Supply Inlet";
            AirExhaustNode = "";
            AirNodeName = zone.Name + " Air Node";
            ReturnAirNodeName = zone.Name + " Return Outlet";
        }

        ///<summary>
        /// Returns the Name of the Zone of the ZoneConnection.
        /// </summary>
        ///<returns>Zonename</returns>
        /// 
        public override string ToString()
        {
            return "Zone Connection for Zone: " + ZoneName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public string Write()
        {
            string temp = "\nZoneHVAC:EquipmentConnections,\n";
            temp += ZoneName + ",   !- Zone Name\n";
            temp += EquipmentList + ",  !- Zone Conditioning Equipment List Name\n";
            temp += InletNode + ",  !- Zone Air Inlet Node or NodeList Name\n";
            temp += AirExhaustNode + ", !- Zone Air Exhaust Node or NodeList Name\n";
            temp += AirNodeName + ",    !- Zone Air Node Name\n";
            temp += ReturnAirNodeName + ";  !- Zone Return Air Node Name\n";

            return temp;
        }
    }
}
