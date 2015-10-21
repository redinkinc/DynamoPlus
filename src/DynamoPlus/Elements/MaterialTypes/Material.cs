/*
 *  This file is part of DynamoPlus.
 *  
 *  Copyright (c) 2014-2015 Technische Universitaet Muenchen, 
 *  Chair of Computational Modeling and Simulation (https://www.cms.bgu.tum.de/)
 *  LEONHARD OBERMEYER CENTER (www.loc.tum.de)
 *  
 *  Developed by Fabian Ritter, Florian Englberger
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
using System.Threading;
using Autodesk.DesignScript.Runtime;
using DynamoPlus.idfFile;

namespace DynamoPlus
{
    /// <summary>
    /// The EnergyPlus Material Element.
    /// </summary>
    [IsVisibleInDynamoLibrary(true)]
    //By Inheriting from NoDuplicate it's inheriting from Iequatable and automatically overrides the equals function
    //With that it can be putted in in the NoDuplicate list which checks in the add function that the object doesn't already exists
    public class Material:AbsElement
    {
        //name is used from base class

        //is it possible to forbit to hide NoDuplicate.Name???????
        //internal string Name { get; set; }
        private string Roughness { get; set; }
        private double Thickness { get; set; }
        private double Conductivity { get; set; }
        private double Density { get; set; }
        private double SpecificHeat { get; set; }
        private double ThermalAbsorptance { get; set; }
        private double SolarAbsorptance { get; set; }
        private double VisibleAbsorptance { get; set; }
        private static int _materialCounter;

        private Material(string name, string roughness, double thickness, double conductivity = 0.045,
                        double density = 265, double specificHeat = 836.8, double thermalAbsorptance = 0.9,
                        double solarAbsorptance = 0.7, double visibleAbsorptance = 0.7)
        {
            Name = name; // name of the material, stored in base class
            Roughness = roughness;
            Thickness = thickness; // in meter
            Conductivity = conductivity;
            Density = density;
            SpecificHeat = specificHeat;
            ThermalAbsorptance = thermalAbsorptance;
            SolarAbsorptance = solarAbsorptance;
            VisibleAbsorptance = visibleAbsorptance;
            Id = _materialCounter;
            _materialCounter++;
        }
       
        ///<summary>
        ///Material as defined in Energy Plus
        ///</summary>
        ///<param name="name">The name of the material</param>
        ///<param name="roughness">The roughness of the material (VeryRough, Rough, MediumRough, MediumSmooth, Smooth, VerySmooth</param>
        ///<param name="thickness">The thickness in meter</param>
        ///<param name="conductivity">The conductivity in W/mK</param>
        ///<param name="density">The density in kg/m³</param>
        ///<param name="specificHeat">The specific heat in J/kgK</param>
        ///<param name="thermalAbsorptance">The thermal absorptance between 0.0 and 1.0</param>
        ///<param name="solarAbsorptance">The solar absorptance between 0.0 and 1.0</param>
        ///<param name="visibleAbsorptance">The visible absorptance between 0.0 and 1.0</param>
        ///<returns>An EnergyPlus Material</returns>
        public static Material CreateMaterialByData(string name, string roughness, double thickness, double conductivity=0.045, double density=265, 
            double specificHeat=836.8, double thermalAbsorptance=0.9, double solarAbsorptance=0.7, double visibleAbsorptance=0.7)
        {
            return new Material(name, roughness, thickness, conductivity, density, specificHeat, thermalAbsorptance,
                solarAbsorptance, visibleAbsorptance);
        }

        /// <summary>
        /// Reads in the Data from the Template and Returns a List of Materials
        /// </summary>
        /// <param name="templatePath"></param>
        /// <returns></returns>
        internal static NoDuplicateList<AbsElement> ReadInFromTemplate(string templatePath)
        {
            var data = Template.GetDataFromFileByKeyword(templatePath, "Material");

            var list = new NoDuplicateList<AbsElement>();
            foreach (var materialData in data.Where(materialData => materialData.Count == 10))
            {
                list.Add(new Material(materialData[1], materialData[2], Convert.ToDouble(materialData[3], CultureInfo.InvariantCulture),
                    Convert.ToDouble(materialData[4], CultureInfo.InvariantCulture), Convert.ToDouble(materialData[5], CultureInfo.InvariantCulture),
                    Convert.ToDouble(materialData[6], CultureInfo.InvariantCulture), Convert.ToDouble(materialData[7], CultureInfo.InvariantCulture),
                    Convert.ToDouble(materialData[8], CultureInfo.InvariantCulture), Convert.ToDouble(materialData[9], CultureInfo.InvariantCulture)));
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
            return "Material, " + Name;
        }

        /// <summary>
        /// Writes the Material into a string for the idf File.
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override string Write()
        {
            var temp = "Material,\n";
            temp += Name + ",\n";
            temp += Roughness + ",\n";
            temp += Thickness.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += Conductivity.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += Density.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += SpecificHeat.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += ThermalAbsorptance.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += SolarAbsorptance.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += VisibleAbsorptance.ToString(CultureInfo.InvariantCulture) + ";\n\n";

            return temp;
        }

    }
}
