using System;
using System.Globalization;
using System.Linq;
using DynamoPlus.idfFile;

namespace DynamoPlus
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowMaterialGas:AbsElement
    {
        private string GasType { get; set; }
        private double Thickness { get; set; }

        private WindowMaterialGas(string name, string gasType, double thickness)
        {
            Name = name;
            GasType = gasType;
            Thickness = thickness;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gasType"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public WindowMaterialGas WindowMaterialGasByData(string name, string gasType, double thickness)
        {
            return new WindowMaterialGas(name,gasType,thickness);
        }

        internal static NoDuplicateList<AbsElement> ReadInFromTemplate(string templatePath)
        {
            var data = Template.GetDataFromFileByKeyword(templatePath, "WindowMaterial:Gas");

            var list = new NoDuplicateList<AbsElement>();
            foreach (var materialData in data.Where(materialData => materialData.Count() == 4))
            {
                list.Add(new WindowMaterialGas(materialData[1], materialData[2], Convert.ToDouble(materialData[3], CultureInfo.InvariantCulture)));
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
            return "WindowMaterial:Gas " + Name;
        }


        /// <summary>
        /// Adds possibility to write into a file
        /// </summary>
        /// <returns></returns>
        public override string Write()
        {
            var temp = "WindowMaterial:Gas, \n";
            temp += Name + ",\n";
            temp += GasType + ",\n";
            temp += Thickness.ToString(CultureInfo.InvariantCulture) + ";\n\n";
            return temp;
        }
    }
}