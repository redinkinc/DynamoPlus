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
using Autodesk.DesignScript.Runtime;
using DynamoPlus.File;

namespace DynamoPlus.Geometry
{
    /// <summary>
    /// The EnergyPlus Construction Element.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    //IEquatable is used to override the equal function
    public class Construction : AbsElement
    {
        private List<string> Layer{get;set;}
        private int ConstructionId { get; set; }
        private static int _constructionCounter = 0;

        /// <summary>
        /// The EnergyPlus Construction
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="layer">List of Materialnames from outside to inside</param>

        private Construction(string name, List<string> layer)
        {
            Name = name;
            Layer = layer;
            ConstructionId = _constructionCounter;
            _constructionCounter++;
        }

        /// <summary>
        /// Creates a energy plus Construction
        /// </summary>
        /// <param name="name"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public Construction ConstructionByData(string name, List<string> layer)
        {
            return new Construction(name, layer);
        }

        /// <summary>
        /// Reads in Constructions from the template and returns a list of Constructions
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static NoDuplicateList<AbsElement> ReadInFromTemplate (string template)
        {
            var data = Template.GetDataFromFileByKeyword(template, "Construction");

            var list = new NoDuplicateList<AbsElement>();
            foreach (var constructionData in data)
            {
                var layer=new List<string>();
                for (var i = 2; i < constructionData.Count; i++)
                {
                    layer.Add(constructionData[i]);
                }
                list.Add(new Construction(constructionData[1],layer));
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
            return "Construction, " + Name + ";";
        }

        /// <summary>
        /// Writes the properties of the Surface into one string
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override string Write()
        {
            var temp = "Construction,\n";
            temp += Name + ", ";
            for (int i = 0; i < Layer.Count;i++ )
            {
                temp += Layer[i];
                if (i != Layer.Count - 1)
                {
                    temp += ",\n";
                }
                else
                {
                    temp += ";\n";
                }
            }
            temp += "\n";
            return temp;
        }
       
    }
}
