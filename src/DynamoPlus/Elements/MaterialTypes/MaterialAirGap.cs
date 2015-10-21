using System;
using System.Globalization;
using System.Linq;
using DynamoPlus.idfFile;

namespace DynamoPlus
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

       

        public override string ToString()
        {
            return Name;
        }

       


        public override string Write()
        {
            string temp = "Material:AirGap,\n";
            temp += Name + ",\n";
            temp += ThermalResistance.ToString(CultureInfo.InvariantCulture) + ";\n\n";          

            return temp;
        }
    }
}