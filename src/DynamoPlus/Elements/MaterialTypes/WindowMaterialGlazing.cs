using System;
using System.Globalization;
using System.Linq;
using DynamoPlus.idfFile;

namespace DynamoPlus
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowMaterialGlazing:AbsElement
    {
        private string OpticalDataType { get; set; }
        private string SpectralDataSetName { get; set; }
        private double Thickness { get; set; }
        private double SolarTransmittance { get; set; }
        private double SolarReflectanceFront { get; set; }
        private double SolarReflectanceBack { get; set; }
        private double VisibleTransmittance { get; set; }
        private double VisibleReflectanceFront { get; set; }
        private double VisibleReflectanceBack { get; set; }
        private double IRTransmittance { get; set; }
        private double IRHemisphericalEmissivityFront { get; set; }
        private double IRHemisphericalEmissivityBack { get; set; }
        private double Conductivity { get; set; }

        private WindowMaterialGlazing(string name, string opticalDataType, string spectralDataSetName, double thickness, double solarTransmittance,
            double solarReflectanceFront, double solarReflectanceBack,
            double visibleTransmittance, double visibleReflectanceFront, double visibleReflectanceBack,
            double irTransmittance, double irHemisphericalEmissivityFront, double irHemisphericalEmissivityBack,
            double conductivity)
        {
            Name = name;
            OpticalDataType = opticalDataType;
            SpectralDataSetName = spectralDataSetName;
            Thickness = thickness;
            SolarTransmittance = solarTransmittance;
            SolarReflectanceFront = solarReflectanceFront;
            SolarReflectanceBack = solarReflectanceBack;
            VisibleTransmittance = visibleTransmittance;
            VisibleReflectanceFront = visibleReflectanceFront;
            VisibleReflectanceBack = visibleReflectanceBack;
            IRTransmittance = irTransmittance;
            IRHemisphericalEmissivityFront = irHemisphericalEmissivityFront;
            IRHemisphericalEmissivityBack = irHemisphericalEmissivityBack;
            Conductivity = conductivity;
        }

        /// <summary>
        /// Energy Plus WindowMaterial:Glazing
        /// </summary>
        /// <param name="name"></param>
        /// <param name="opticalDataType"></param>
        /// <param name="spectralDataSetName"></param>
        /// <param name="thickness"></param>
        /// <param name="solarTransmittance"></param>
        /// <param name="solarReflectanceFront"></param>
        /// <param name="solarReflectanceBack"></param>
        /// <param name="visibleTransmittance"></param>
        /// <param name="visibleReflectanceFront"></param>
        /// <param name="visibleReflectanceBack"></param>
        /// <param name="irTransmittance"></param>
        /// <param name="irHemisphericalEmissivityFront"></param>
        /// <param name="irHemisphericalEmissivityBack"></param>
        /// <param name="conductivity"></param>
        /// <returns></returns>
        public WindowMaterialGlazing WindowMaterialGlazingByData(string name, string opticalDataType, string spectralDataSetName, double thickness,
            double solarTransmittance, double solarReflectanceFront, double solarReflectanceBack,
            double visibleTransmittance, double visibleReflectanceFront, double visibleReflectanceBack,
            double irTransmittance, double irHemisphericalEmissivityFront, double irHemisphericalEmissivityBack,
            double conductivity)
        {
            return new WindowMaterialGlazing(name, opticalDataType, spectralDataSetName, thickness,
                solarTransmittance, solarReflectanceFront, solarReflectanceBack,
                visibleTransmittance, visibleReflectanceFront, VisibleReflectanceBack,
                irTransmittance, irHemisphericalEmissivityFront, irHemisphericalEmissivityBack,
                conductivity);
        }

        internal static NoDuplicateList<AbsElement> ReadInFromTemplate(string templatePath)
        {
            var data = Template.GetDataFromFileByKeyword(templatePath, "WindowMaterial:Glazing");

            var list = new NoDuplicateList<AbsElement>();
            foreach (var materialData in data.Where(materialData=>materialData.Count()==15)) 
            {
                list.Add(new WindowMaterialGlazing(materialData[1], materialData[2], materialData[3],
                                Convert.ToDouble(materialData[4], CultureInfo.InvariantCulture), Convert.ToDouble(materialData[5], CultureInfo.InvariantCulture),
                                Convert.ToDouble(materialData[6], CultureInfo.InvariantCulture), Convert.ToDouble(materialData[7], CultureInfo.InvariantCulture), 
                                Convert.ToDouble(materialData[8], CultureInfo.InvariantCulture), Convert.ToDouble(materialData[9], CultureInfo.InvariantCulture), 
                                Convert.ToDouble(materialData[10], CultureInfo.InvariantCulture), Convert.ToDouble(materialData[11], CultureInfo.InvariantCulture),
                                Convert.ToDouble(materialData[12], CultureInfo.InvariantCulture), Convert.ToDouble(materialData[13], CultureInfo.InvariantCulture),
                                Convert.ToDouble(materialData[14], CultureInfo.InvariantCulture)));
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
            return "WindowMaterial:Glazing, " + Name;
        }

        /// <summary>
        /// Adds possibility to write into a file
        /// </summary>
        /// <returns></returns>
        public override string Write()
        {
            string temp = "WindowMaterial:Glazing,\n";
            temp += Name + ",\n";
            temp += OpticalDataType + ",\n";
            temp += SpectralDataSetName + ",\n";
            temp += Thickness.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += SolarTransmittance.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += SolarReflectanceFront.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += SolarReflectanceBack.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += VisibleTransmittance.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += VisibleReflectanceBack.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += VisibleReflectanceFront.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += IRTransmittance.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += IRHemisphericalEmissivityFront.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += IRHemisphericalEmissivityBack.ToString(CultureInfo.InvariantCulture) + ",\n";
            temp += Conductivity.ToString(CultureInfo.InvariantCulture) + ";\n\n";

            return temp;
        }
    }
}