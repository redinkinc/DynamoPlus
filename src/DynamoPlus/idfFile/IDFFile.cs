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

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DynamoPlus.idfFile
{
    /// <summary>
    /// This class handles the idf File and stores all relevant information
    /// </summary>
    public static class IdfFile
    {
        //internal string TemplateFilepath { get; set; }
        //private string OutputFilepath { get; set; }

        
        /// <summary>
        /// Reads the Results from the Simulation
        /// </summary>
        /// <param name="templateFilePath"></param>
        /// /// <returns></returns>
        public static Dictionary<string, double[]> ReadResults(string templateFilePath)
        {
            var dict = new Dictionary<string, double[]>();
            var files = Directory.GetFiles(templateFilePath, "*.csv");

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);

                var energyUse = "cooling";
                var heatingEnergy = 0.0;
                var coolingEnergy = 0.0;

                foreach (var line in lines)
                {

                    if (line.Contains("ZoneCoolingSummaryMonthly")) energyUse = "cooling";
                    else if (line.Contains("ZoneHeatingSummaryMonthly")) energyUse = "heating";

                    if (line.Contains("Annual Sum or Average"))
                    {
                        string[] value = line.Split(',');
                        var amount = double.Parse(value[2]);

                        switch (energyUse)
                        {
                            case "cooling":
                                coolingEnergy += amount;
                                break;
                            case "heating":
                                heatingEnergy += amount;
                                break;
                        }

                    }

                }

                double[] energyDemand = { coolingEnergy / 100, heatingEnergy / 100 };
                dict.Add(file, energyDemand);
            }

            return dict;

        }

        /// <summary>
        /// Write Simulation file (.idf)
        /// </summary>
        /// <param name="directory">file directory</param>
        /// <param name="name">file name</param>
        /// <param name="elements">all elements to add to file</param>
        /// <param name="additionalInformation">additional string to write to file</param>
        /// <returns>file path</returns>
        public static string Write(string directory, string name, Elements elements, string additionalInformation)
        {
            var filePath = directory + @"\" + name + ".idf";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var filetext = StringCollector.GetIdfString(elements);

            using (var outfile = new StreamWriter(filePath))
            {
                foreach (var text in filetext)
                {
                    outfile.Write(text);
                }

                outfile.Write(additionalInformation);

            }

            return filePath;
        }
    }
}
