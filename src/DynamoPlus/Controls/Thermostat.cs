/*
 *  This file is part of DynamoPlus.
 *  
 *  Copyright (c) 2014-2015 Technische Universitaet Muenchen, 
 *  Chair of Computational Modeling and Simulation (https://www.cms.bgu.tum.de/)
 *  LEONHARD OBERMEYER CENTER (https://www.loc.tum.de)
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

namespace DynamoPlus.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class Thermostat : AbsElement
    {
        /// <summary>
        /// 
        /// </summary>
        public string ZoneName { get; set; }
        private string HeatingSpTempScheduleName { get; set; }
        private string CoolingSpTempScheduleName { get; set; }

        /// <summary>
        /// Creates a Thermostat with a DualSetpoint setpoint.
        /// </summary>
        /// <param name="zoneName">The name of the corresponding Zone or ZoneList</param>
        /// <param name="heatingSpTempScheduleName">The name of the Schedule controling the heating setpoint temperature</param>
        /// <param name="coolingSpTempScheduleName">The name of the Schedule controling the cooling setpoint temperature</param>
        public Thermostat(string zoneName, string heatingSpTempScheduleName, string coolingSpTempScheduleName)
        {
            ZoneName = zoneName;
            HeatingSpTempScheduleName = heatingSpTempScheduleName;
            CoolingSpTempScheduleName = coolingSpTempScheduleName;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return ZoneName + " Thermostat (DualSetpoint)";
        }

        /// <summary>
        /// Adds possibility to write into a file
        /// </summary>
        /// <returns></returns>
        public override string Write()
        {
            var text = "ZoneControl:Thermostat,\n";
            text += "    " + ZoneName + " Thermostat,     !- Name\n";
            text += "    " + ZoneName + ",                !- Zone or ZoneList Name\n";
            text += "    ALWAYS 4,                !- Control Type Schedule Name\n";
            text += "    ThermostatSetpoint:DualSetpoint,  !- Control 1 Object Type\n";
            text += "    " + ZoneName + " Thermostat Dual SP Control;  !- Control 1 Name\n\n";
            
            text += "ThermostatSetpoint:DualSetpoint,\n";
            text += "    " + ZoneName + " Thermostat Dual SP Control,  !- Name\n";
            text += "    " + HeatingSpTempScheduleName + ",               !- Heating Setpoint Temperature Schedule Name\n";
            text += "    " + CoolingSpTempScheduleName + ";               !- Cooling Setpoint Temperature Schedule Name\n";
            return text;
        }
    }
}
