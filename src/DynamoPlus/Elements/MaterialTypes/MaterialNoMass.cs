using System;
using System.Globalization;
using System.Linq;
using DSCore;
using DynamoPlus.idfFile;

namespace DynamoPlus
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