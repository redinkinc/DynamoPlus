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
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus
{
    /// <summary>
    /// A List that doesn't allow to insert a element twice
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [IsVisibleInDynamoLibrary(false)]
    public class NoDuplicateList<T> : List<AbsElement>
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherList"></param>
        /// <returns></returns>
        public NoDuplicateList<T> AddNoDuplicateList(NoDuplicateList<T> otherList)
        {
            foreach (var element in otherList)
            {
                Add(element);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        public new void Add(AbsElement value)
        {                           
            if (Contains(value))
            {
                Remove(value);
                //throw new ArgumentException("Object already exists", value.ToString());
            } 
            base.Add(value);
        }
    }
}


