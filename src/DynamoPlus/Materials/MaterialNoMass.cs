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
    /// Energy Plus Material:NoMass
    /// </summary>
    public class MaterialNoMass:AbsElement
    {
        private string Roughness { get; set; }
        private double Resistence { get; set; }
        private double ThermalAbsorptance { get; set; }
        private double SolarAbsorptance { get; set; }
        private double VisibleAbsorptance { get; set; }

        private MaterialNoMass(string name, string roughness = "Rough", double resistance=2.290965, double thermalAbsorptance=0.9,
            double solarAbsorptance=0.75, double visibleAbsorptance=0.75)
        {
            //Name in base
            Name = name;
            Roughness = roughness;
            Resistence = resistance;
            ThermalAbsorptance = thermalAbsorptance;
            SolarAbsorptance = solarAbsorptance;
            VisibleAbsorptance = visibleAbsorptance;

        }
        
        /// <summary>
        /// Create an Energy Plus Material:NoMass by user defined Data
        /// </summary>
        /// <param name="name"></param>
        /// <param name="roughness"></param>
        /// <param name="resistance"></param>
        /// <param name="thermalAbsorptance"></param>
        /// <param name="solarAbsorptance"></param>
        /// <param name="visibleAbsorptance"></param>
        /// <returns></returns>
        public MaterialNoMass MaterialNoMassByData(string name, string roughness = "Rough", double resistance = 2.290965,
            double thermalAbsorptance = 0.9,
            double solarAbsorptance = 0.75, double visibleAbsorptance = 0.75)
        {
            return new MaterialNoMass(name,roughness,thermalAbsorptance,solarAbsorptance,visibleAbsorptance);
        }

        internal static NoDuplicateList<AbsElement> ReadInFromTemplate(string templatePath)
        {
            var data = Template.GetDataFromFileByKeyword(templatePath, "Material:NoMass");

            var list=new NoDuplicateList<AbsElement>();
            foreach (var materialData in data.Where(materialData=>materialData.Count()==7))
            {
                list.Add(new MaterialNoMass(materialData[1], (materialData[2]), Convert.ToDouble(materialData[3], CultureInfo.InvariantCulture),
                                Convert.ToDouble(materialData[4], CultureInfo.InvariantCulture), Convert.ToDouble(materialData[5], CultureInfo.InvariantCulture),
                                Convert.ToDouble(materialData[6], CultureInfo.InvariantCulture)));
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
            return "Material:NoMass, " + Name;
        }


        /// <summary>
        /// Adds possibility to write into a file
        /// </summary>
        /// <returns></returns>
        public override string Write()
        {
            string temp = "Material:NoMass,\n";
            temp += Name + ",\n";
            temp += Roughness + ",\n";
            temp += ThermalAbsorptance.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += SolarAbsorptance.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += VisibleAbsorptance.ToString(CultureInfo.InvariantCulture) + ";\n\n";

            return temp;
        }
    }
}