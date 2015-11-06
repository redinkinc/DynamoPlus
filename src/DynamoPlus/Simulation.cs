/*
 *  This file is part of DynamoPlus.
 *  
 *  Copyright (c) 2014-2015 Technische Universitaet Muenchen, 
 *  Chair of Computational Modeling and Simulation (https://www.cms.bgu.tum.de/)
 *  LEONHARD OBERMEYER CENTER (https://www.loc.tum.de)
 *  
 *  Developed by Fabian Ritter (Contact: mailto:mail@redinkinc.de) and Florian Englberger
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

using System.Diagnostics;
using System.IO;


namespace DynamoPlus

{
    /// <summary>
    /// Runs EPlus
    /// </summary>
    public static class Simulation
    {
        /// <summary>
        /// Runs the EnergyPlus Simulation.
        /// </summary>
        /// <param name="directory">EnergyPlus installation directory</param>
        /// <param name="idf">idf file</param>
        /// <param name="weatherData">weather data file</param>
        /// <returns>True when finished.</returns>
        public static string RunEPlus (string directory, string idf, string weatherData)
        {
            var file = Path.ChangeExtension(idf, null);
            var command = "RunEPlus " + file + " " + weatherData;

            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
                    {
                        WorkingDirectory = @directory + "\\",
                        CreateNoWindow = true,
                        //RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false
                    }
            };

            proc.Start();
            
            var e = proc.StandardError.ReadToEnd();
            if (e != "" || e != "RunEPlus executed succesfully")
            {
                //throw new ArgumentException(e);
            }

            //var result = proc.StandardOutput.ReadToEnd();

            proc.WaitForExit();
            proc.Close();

            var resultfile = file + "Table.csv";
            return resultfile;
        }

    }
}
