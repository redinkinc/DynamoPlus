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

namespace DynamoPlus.File
{
    /// <summary>
    /// This class collects all different Materials so that it's easier to add new Material-Types
    /// </summary>
    internal static class MaterialReader
    {
        internal static NoDuplicateList<AbsElement> ReadMaterialsFromTemplate(string templatePath)
        {
            var materialList = new NoDuplicateList<AbsElement>();
            materialList.AddNoDuplicateList(Material.ReadInFromTemplate(templatePath));
            materialList.AddNoDuplicateList(MaterialAirGap.ReadInFromTemplate(templatePath));
            materialList.AddNoDuplicateList(MaterialNoMass.ReadInFromTemplate(templatePath));

            return materialList;
        }

        internal static NoDuplicateList<AbsElement> ReadWindowMaterialsFromTemplate(string templatePath)
        {
            var windowMaterialList= new NoDuplicateList<AbsElement>();
            windowMaterialList.AddNoDuplicateList(WindowMaterialGas.ReadInFromTemplate(templatePath));
            windowMaterialList.AddNoDuplicateList(WindowMaterialGlazing.ReadInFromTemplate(templatePath));
            windowMaterialList.AddNoDuplicateList(WindowMaterialSimpleGlazingSystem.ReadInFromTemplate(templatePath));
            return windowMaterialList;
        }
    }
}