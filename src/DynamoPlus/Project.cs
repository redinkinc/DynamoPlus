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


using Autodesk.DesignScript.Runtime;

namespace DynamoPlus
{
    /// <summary>
    /// The DynamoPlus Project.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class Project
    {
        private string _name;

        private string Directory { get; set; }

        /// <summary>
        /// The EnergyPlus Elements
        /// </summary>
        public Elements Elements { get; set; }

        /// <summary>
        /// Creates an DynamoPlusProject
        /// </summary>
        private Project(string name, string path)
        {
            _name = name;
            Directory = path;
            //Elements = new Elements();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Project Create(string name, string path)
        {
            return new Project(name, path);
        }

        ///// <summary>
        ///// Creates a new DynamoPlus Project
        ///// </summary>
        ///// <param name="name">Name of the project</param>
        ///// <param name="path">Path for the project directory</param>
        ///// <returns>A new DynamoPlus Project</returns>
        //[MultiReturn(new []{"project", "elements"})]
        //public static Dictionary<string, object> Create(string name, string path)
        //{
        //    var project = new Project(name, path);
        //    var elements = project.Elements;
        //    return new Dictionary<string, object>
        //    {
        //        {"project", project},
        //        {"elements", elements}
        //    };
        //}

        ///// <summary>
        ///// Exports all Project Elements into an EnergyPlus Simulation File.
        ///// </summary>
        ///// <returns>True when successful!</returns>
        //public bool ExportToIdf()
        //{
        //    var idfFile = new Idf(Directory);
        //    idfFile.WriteNewFile(Directory, _name, Elements);
        //    return true;
        //}
    }
}
