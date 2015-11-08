using System;
using System.Globalization;
using System.Linq;
using DynamoPlus.File;

namespace DynamoPlus
{
    /// <summary>
    /// Energy Plus WindowMaterial:SimpleGlazingSystem
    /// </summary>
    public class WindowMaterialSimpleGlazingSystem:AbsElement
    {
        private double UFactor { get; set; }
        private double SolarHeatGainCoefficient { get; set; }
        private double VisibleTransmittance { get; set; }

        private WindowMaterialSimpleGlazingSystem(string name, double uFactor=2.716, double solarHeatGainCoefficient = 0.763,
            double visibleTransmittance = 0.812)
        {
            Name = name;
            UFactor = uFactor;
            SolarHeatGainCoefficient = solarHeatGainCoefficient;
            VisibleTransmittance = visibleTransmittance;
        }

        /// <summary>
        /// Energy Plus WindowMaterial:SimpleGlazingSystem
        /// </summary>
        /// <param name="name"></param>
        /// <param name="uFactor"></param>
        /// <param name="solarHeatGainCoefficient"></param>
        /// <param name="visibleTransmittance"></param>
        /// <returns></returns>
        public WindowMaterialSimpleGlazingSystem WindowMaterialSimpleGlazingSystemByData(string name, double uFactor = 2.716,
            double solarHeatGainCoefficient=0.763,
            double visibleTransmittance = 0.812)
        {
            return new WindowMaterialSimpleGlazingSystem(name, uFactor,solarHeatGainCoefficient, visibleTransmittance);
        }

        internal static NoDuplicateList<AbsElement> ReadInFromTemplate(string templatePath)
        {
            var data = Template.GetDataFromFileByKeyword(templatePath, "WindowMaterial:SimpleGlazingSystem");

            var list = new NoDuplicateList<AbsElement>();
            foreach (var materialData in data.Where(materialData => materialData.Count() <= 5 && materialData.Count()>=4))
            {
                switch (materialData.Count())
                {
                    case 5:
                        list.Add(new WindowMaterialSimpleGlazingSystem(materialData[1],
                            Convert.ToDouble(materialData[2], CultureInfo.InvariantCulture),
                            Convert.ToDouble(materialData[3], CultureInfo.InvariantCulture), 
                            Convert.ToDouble(materialData[4], CultureInfo.InvariantCulture)));
                        break;
                    case 4:
                        list.Add(new WindowMaterialSimpleGlazingSystem(materialData[1],
                            Convert.ToDouble(materialData[2], CultureInfo.InvariantCulture), 
                            Convert.ToDouble(materialData[3], CultureInfo.InvariantCulture)));
                        break;
                }           
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
            return "WindowMaterial:SimpleGlazingSystem " + Name;
        }

        /// <summary>
        /// Adds possibility to write into a file
        /// </summary>
        /// <returns></returns>
        public override string Write()
        {
            string temp = "WindowMaterial:SimpleGlazingSystem,\n";
            temp += Name + ",\n";
            temp += UFactor.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += SolarHeatGainCoefficient.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += VisibleTransmittance.ToString(CultureInfo.InvariantCulture) + ";\n\n";          

            return temp;
        }


        
    }
}