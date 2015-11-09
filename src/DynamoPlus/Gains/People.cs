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

namespace DynamoPlus.Gains
{
    /// <summary>
    /// The energyPlus People element
    /// </summary>
    public class People : AbsElement
    {
        /// <summary>
        /// The Name of the Corresponding Zone or ZoneList
        /// </summary>
        public string ZoneName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NumOfPeopleScheduleName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ActivityLevelScheduleName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zoneName"></param>
        /// <param name="numOfPeopleScheduleName"></param>
        /// <param name="activityLevelSchuleName"></param>
        public People(string zoneName, string numOfPeopleScheduleName, string activityLevelSchuleName)
        {
            Name = zoneName + " PeopleInst";
            ZoneName = zoneName;
            NumOfPeopleScheduleName = numOfPeopleScheduleName;
            ActivityLevelScheduleName = activityLevelSchuleName;
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
        /// Adds possibility to write into a file
        /// </summary>
        /// <returns></returns>
        public override string Write()
        {
            var text = "People,\n";
            text += "    " + Name + ",  !-Name\n";
            text += "    " + ZoneName + ",  !-Zone or ZoneList Name\n";
            text += "    " + NumOfPeopleScheduleName + ",  !-Number of People Schedule Name\n";
            text += "    Area/Person,             !-Number of People Calculation Method\n";
            text += "    0,                       !-Number of People\n";
            text += "    10,                      !-People per Zone Floor Area {person/m2}\n";
            text += "    ,                        !-Zone Floor Area per Person {m2/person}\n";
            text += "    0.3,                     !-Fraction Radiant\n";
            text += "    ,                        !-Sensible Heat Fraction\n";
            text += "    " + ActivityLevelScheduleName + "; !-Activity Level Schedule Name\n";
            return text;
        }

        
    }
}
