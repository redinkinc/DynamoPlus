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

namespace DynamoPlus.Loads
{
    /// <summary>
    /// The EnergyPlus Lights element.
    /// </summary>
    public class Lights : AbsElement
    {
        /// <summary>
        /// The Name of the corresponding Zone or ZoneList
        /// </summary>
        public Zone ZoneName { get; set; }
        /// <summary>
        /// The Name of the Lights control schedule.
        /// </summary>
        public string ScheduleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="scheduleName"></param>
        public Lights(Zone zoneName, string scheduleName)
        {
            ZoneName = zoneName;
            ScheduleName = scheduleName;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return ZoneName + "LightsInst";
        }

        /// <summary>
        /// Adds possibility to write into a file
        /// </summary>
        /// <returns></returns>
        public override string Write()
        {
            var text = "Lights,\n";
            text += "    " + ZoneName + " LightsInst,  !-Name\n";
            text += "    " + ZoneName + ",  !-Zone or ZoneList Name\n";
            text += "    " + ScheduleName + ",!-Schedule Name\n";
            text += "    Watts / Area,            !-Design Level Calculation Method\n";
            text += "    ,                        !-Lighting Level {W}\n";
            text += "    9.68752354606417,        !-Watts per Zone Floor Area {W/m2}\n";
            text += "    ,                        !-Watts per Person {W/person}\n";
            text += "    ,                        !-Return Air Fraction\n";
            text += "    ,                        !-Fraction Radiant\n";
            text += "    ,                        !-Fraction Visible\n";
            text += "    ,                        !-Fraction Replaceable\n";
            text += "    Lights; !-End - Use Subcategory\n";
            return text;
        }
    }
}
