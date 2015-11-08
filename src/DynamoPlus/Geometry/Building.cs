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

using System;
using System.Globalization;
using Autodesk.DesignScript.Runtime;
using Dynamo.Models;

namespace DynamoPlus.Geometry
{
    
    /// <summary>
    /// The EnergyPlus Building
    /// </summary>
    
    [NodeCategory("DynamoPlus.Elements")]
    public class Building:AbsElement
    {
        private double NorthAxis { get; set; }
        private string Terrain { get; set; }
        private double LoadsConvergenceToleranceValue { get; set; }
        private double DeltaC { get; set; }
        private string SolarDistribution { get; set; }
        private int MaxNWarmupDays { get; set; }
        private int MinNWarmupDays { get; set; }

        private Building  (string name, double northAxis, string terrain, double loadsConvergenceToleranceValue,
            double deltaC, string solarDistribution, int maxNWarmupDays,int minNWarmupDays)
        {
            Name = name;
            NorthAxis = northAxis;
            Terrain = terrain;
            LoadsConvergenceToleranceValue = loadsConvergenceToleranceValue;
            DeltaC = deltaC;
            SolarDistribution = solarDistribution;
            MaxNWarmupDays = maxNWarmupDays;
            MinNWarmupDays = minNWarmupDays;
        }

        /// <summary>
        /// Creates a Building with default values:
        /// North Axis {deg}: 0 
        /// Terrain: Urban
        /// Loads Convergence Tolerance Value: 0.04
        /// Temperature Convergence Tolerance Value {deltaC}: 0.4
        /// Solar Distribution: Full Exterior
        /// Maximum Number of Warmup Days: 25
        /// Minimum Number of Warmup Days: 0
        /// </summary>
        /// <param name="name">The Name of the EnergyPlus.Building</param>
        /// <returns></returns>
        public static Building ByName(string name)
        {
            var building = new Building(name, -0, "Urban", 0.04, 0.4, "FullExterior", 25, 0);
            return building;
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
        /// Writes all Information of the Building object into a string. 
        /// </summary>
        /// <returns>All Building Information as string.</returns>
        [IsVisibleInDynamoLibrary(false)]
        public override string Write()
        {
            var temp = "Building,\n";
            temp += Name + ",           !- Name\n";
            if (Math.Abs(NorthAxis) < 0.001)
            {
                temp += "-0,  !- North Axis {deg}\n";
            }
            else
            {
                temp += NorthAxis.ToString(CultureInfo.InvariantCulture) + ",  !- North Axis {deg}\n";
            }
            temp += Terrain + ",    !- Terrain\n";
            temp += LoadsConvergenceToleranceValue.ToString(CultureInfo.InvariantCulture) + ", !- Loads Convergence Tolerance Value\n";
            temp += DeltaC.ToString(CultureInfo.InvariantCulture) + ",  !- Temperature Convergence Tolerance Value {deltaC}\n";
            temp += SolarDistribution + ",  !- Solar Distribution\n";
            if (MaxNWarmupDays > 0) temp += MaxNWarmupDays; //only set when bigger than 0, else emtpy. Otherwise will result in EPlus error.
            temp += ",  !- Maximum Number of Warmup Days\n";
            if (MinNWarmupDays > 0) temp += MinNWarmupDays; //only set when bigger than 0, else emtpy. Otherwise will result in EPlus error.
            temp += ";  !- Minimum Number of Warmup Days";
            return temp;
        }

    }
    
}
    
