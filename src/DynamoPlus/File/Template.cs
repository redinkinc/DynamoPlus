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
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus.File
{
    /// <summary>
    /// reads the template
    /// </summary>
    public static class Template
    {

        /// <summary>
        /// returns elements defined in the template file.
        /// </summary>
        /// <param name="template">tamplate file (path)</param>
        /// <returns></returns>
        [MultiReturn( new[] {"Schedules:Compact", "Schedules:Year", "construction", "materials", "materialsNoMass", "windowMaterials"})]
        [IsVisibleInDynamoLibrary(false)] //Hidden for current build. maybe used for setting defaults later.
        public static Dictionary<string, List<string>> ReadNames(string template)
        {
            return new Dictionary<string, List<string>>()
            {
                { "Schedule:Compact", new List<string>(GetNamesFromFileByKeyword(template, "Schedule:Compact")) },
                // schedulesCompact.Insert(0, "default");
                { "schedulesYear", new List<string>(GetNamesFromFileByKeyword(template, "Schedule:Year")) },
                // schedulesYear.Insert(0, "default");
                { "constructions", new List<string>(GetNamesFromFileByKeyword(template, "Construction")) },
                // constructions.Insert(0, "default");
                { "materials", new List<string>(GetNamesFromFileByKeyword(template, "Material")) },
                // materials.Insert(0, "default");
                { "materialsNoMass", new List<string>(GetNamesFromFileByKeyword(template, "Material:NoMass")) },
                // materialsNoMass.Insert(0, "default");
                { "windowMaterials", new List<string>(GetNamesFromFileByKeyword(template, "WindowMaterial")) }
                // windowMaterials.Insert(0, "default");
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateFile"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "Schedules:Compact", "Schedules:Year", "Schedule:Constant", "ScheduleTypeLimits" })]
        public static Dictionary<string, List<string>> GetScheduleNames(string templateFile)
        {
            return new Dictionary<string, List<string>>()
            {
                { "schedules:Year", new List<string>(GetNamesFromFileByKeyword(templateFile, "Schedule:Year")) },
                { "Schedule:Compact", new List<string>(GetNamesFromFileByKeyword(templateFile, "Schedule:Compact")) },
                { "Schedule:Week:Compact", new List<string>(GetNamesFromFileByKeyword(templateFile, "Schedule:Week:Compact")) },
                { "ScheduleTypeLimits", new List<string>(GetNamesFromFileByKeyword(templateFile, "ScheduleTypeLimits")) },
                { "Schedule:Constant", new List<string>(GetNamesFromFileByKeyword(templateFile, "Schedule:Constant")) }
            };
        }

        /// <summary>
        /// Reads in all pre-written Constructions, Materials and WindowMaterials from the template
        /// </summary>
        /// <param name="templateFile">The Filepath to the template file</param>
        /// <returns>Construction, Materials and Windowmaterials as DynamoPlus Elements.</returns>
        [MultiReturn("constructions", "materials", "windowMaterials")]
        public static Dictionary<string,NoDuplicateList<AbsElement>> GetConstrutionsAndMaterials(string templateFile)       
        {
            var constructions =new NoDuplicateList<AbsElement>();
            constructions.AddNoDuplicateList(Construction.ReadInFromTemplate(templateFile));

            var materials = MaterialReader.ReadMaterialsFromTemplate(templateFile);
            var windowMaterials = MaterialReader.ReadWindowMaterialsFromTemplate(templateFile);
              
            return new Dictionary<string, NoDuplicateList<AbsElement>>()
            {
                { "constructions", constructions},
                { "materials", materials },       
                { "windowMaterials", windowMaterials }
                
            };

        }

        /// <summary>
        /// Possibility to get the informations from the template File to a specific keyword 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public static IEnumerable<List<string>> GetDataFromFileByKeyword (string path, string keyword)
        {
            var lines = global::System.IO.File.ReadAllLines(path);
            var collector = new List<List<string>>();

            for (var i = 0; i < lines.Length; i++)
            {
                //Only reads on if lines contains keyword and does not contain a "!" and a ";"
                // Idea is to seperate if the keyword means the class name or is part of an attribute
                //problem is that if in the last attribute contains the keyword it is handled like a new class object
                //but at the point of converting the data into a class these will not be handled
                if (!lines[i].Contains(keyword) || (lines[i].Contains("!")&& !lines[i].Contains(";"))) continue;//???
                var data = new List<string>();
                var j = i;
                //Read until line contains a ";"
                do
                {
                    string line;
                    var commentStart = lines[j].IndexOf("!");
                    if (commentStart != -1)
                    {
                        line = lines[j].Remove(commentStart).Trim();
                    }
                    else
                    {
                        line = lines[j].Trim();
                    }

                    string[] value;

                    if (line.LastIndexOf(",", StringComparison.Ordinal) == line.Length - 1 ||
                        line.LastIndexOf(";", StringComparison.Ordinal) == line.Length - 1)
                    {
                        value = line.Remove(line.Length - 1).Split(',',';');
                    }
                    else
                    {
                        value = line.Split(',', ';');
                    }

                    foreach (var val in value)
                    {
                        //Add Data without whitespaces
                        data.Add(val.Trim());
                    }
                    j++;
                }
                while (!lines[j - 1].Contains(";"));

                if (data[1]=="")data.RemoveAt(1);
                collector.Add(data);
                i = j;
            }
            return collector;
        }
        


        private static IEnumerable<string> GetNamesFromFileByKeyword(string path,string keyword)
        {
            var collector = GetDataFromFileByKeyword(path, keyword);
            return collector.Select(list => list[1]).ToList();
        }
    }
}
