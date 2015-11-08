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
using System.Globalization;
using System.Linq;
using DynamoPlus.File;

namespace DynamoPlus.Materials
{
    /// <summary>
    /// energy Plus Material:AirGap
    /// </summary>
    public class MaterialAirGap:AbsElement
    {
        private double ThermalResistance { get; set; }

        private MaterialAirGap(string name, double thermalResistance)
        {
            Name = name;
            ThermalResistance = thermalResistance;
        }

        /// <summary>
        /// Creates an EnergyPlus MaterialAirGap
        /// </summary>
        /// <param name="name"></param>
        /// <param name="thermalResistance"></param>
        /// <returns></returns>
        public MaterialAirGap MaterialAirGapByData(string name, double thermalResistance)
        {
            return new MaterialAirGap(name, thermalResistance);
        }

        internal static NoDuplicateList<AbsElement> ReadInFromTemplate(string templatePath)
        {
            var data = Template.GetDataFromFileByKeyword(templatePath, "Material:AirGap");

            var list = new NoDuplicateList<AbsElement>();
            foreach (var materialData in data.Where(materialData=>materialData.Count()==3))
            {
                list.Add(new MaterialAirGap(materialData[1], Convert.ToDouble(materialData[2], CultureInfo.InvariantCulture)));
            }
            return list;
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
            string temp = "Material:AirGap,\n";
            temp += Name + ",\n";
            temp += ThermalResistance.ToString(CultureInfo.InvariantCulture) + ";\n\n";          

            return temp;
        }
    }
}