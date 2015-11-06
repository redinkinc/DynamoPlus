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


using Autodesk.DesignScript.Runtime;

namespace DynamoPlus
{
    /// <summary>
    /// 
    /// </summary>
    public class EquipmentList
    {
        private string Name { get; set; }
        private string EquipmentObject { get; set; }
        private string EquipmentName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="zone"></param>
        public EquipmentList(Zone zone)
        {
            Name = zone.Name + " Equipment";
            EquipmentObject = "ZoneHVAC:IdealLoadsAirSystem";
            EquipmentName = zone.Name + " Purchased Air";
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
        /// 
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public string Write()
        {
            string temp = "\nZoneHVAC:EquipmentList,\n";
            temp += Name + ",      !- Name\n";
            temp += EquipmentObject + ",  !- Zone Equipment 1 Object Type\n";
            temp += EquipmentName + ",  !- Zone Equipment 1 Name\n";
            temp += "1, !- Zone Equipment 1 Cooling Sequence\n";
            temp += "1; !- Zone Equipment 1 Heating or No-Load Sequence\n";

            return temp;
        }
    }
}
