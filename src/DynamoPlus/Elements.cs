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

using System.Collections.Generic;
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus
{
    /// <summary>
    /// All Elements of the Building
    /// </summary>
    public class Elements
    {
        /// <summary>
        /// The Building where the elements are associated to.
        /// </summary>
        public Building Building { get; set; }
        /// <summary>
        /// List of Zones
        /// </summary>
        public NoDuplicateList<Zone> Zones { get; set; }
        /// <summary>
        /// List of ZoneLists
        /// </summary>
        public NoDuplicateList<ZoneList> ZoneLists { get; set; } 
        /// <summary>
        /// List of BuildingSurfaces
        /// </summary>
        public NoDuplicateList<BuildingSurface> BuildingSurfaces { get; set; }
        /// <summary>
        /// List of Fenestration Surfaces
        /// </summary>
        public NoDuplicateList<FenestrationSurface> FenestrationSurfaces { get; set; }
        /// <summary>
        /// List of Shading Surfaces
        /// </summary>
        public NoDuplicateList<ShadingSurface> ShadingSurfaces { get; set; }
        /// <summary>
        /// List of Overhangs
        /// </summary>
        public NoDuplicateList<ShadingOverhang> ShadingOverhangs { get; set; }

        /// <summary>
        /// List of Materials that checks for duplicates in the add function
        /// </summary>
        public NoDuplicateList<Material> Materials { get; set; }
        
        /// <summary>
        /// List of Constructions
        /// </summary>
        public NoDuplicateList<Construction> Constructions { get; set; }

            /// <summary>
        /// 
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public Elements()
        {
            Zones = new NoDuplicateList<Zone>();
            ZoneLists = new NoDuplicateList<ZoneList>();
            BuildingSurfaces= new NoDuplicateList<BuildingSurface>();
            FenestrationSurfaces = new NoDuplicateList<FenestrationSurface>();
            ShadingSurfaces = new NoDuplicateList<ShadingSurface>();
            ShadingOverhangs = new NoDuplicateList<ShadingOverhang>();
            Materials = new NoDuplicateList<Material>();      
            Constructions = new NoDuplicateList<Construction>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="building"></param>
        /// <returns>The Elements object.</returns>
        public Elements AddBuilding(Building building)
        {
            Building = building;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zones"></param>
        /// <returns>The Elements object.</returns>
        public Elements AddZones(List<Zone> zones)
        {
            foreach (var zone in zones)
            {
                Zones.Add(zone);
            }
            return this;
        }

        /// <summary>
        /// Adds ZoneList to Elements
        /// </summary>
        /// <returns>The Elements object.</returns>
        public Elements AddZoneList(List<ZoneList> zoneLists)
        {
            foreach (var zoneList in zoneLists)
            {
                ZoneLists.Add(zoneList);
            }
            return this;
        }

        /// <summary>
        /// Adds the BuildingSurfaces to the Project Elements.
        /// </summary>
        /// <param name="buildingSurfaces">List of BuildingSurfaces to be added.</param>
        /// <returns>The Elements object.</returns>
        public Elements AddBuildingSurfaces(List<BuildingSurface> buildingSurfaces)
        {
            foreach (var surface in buildingSurfaces)
            {
                BuildingSurfaces.Add(surface);
            }

            return this;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shadingSurfaces"></param>
        /// <returns>The Elements object.</returns>
        public Elements AddShadingSurfaces(List<ShadingSurface> shadingSurfaces)
        {
            foreach (var shadingSurface in shadingSurfaces)
            {
                ShadingSurfaces.Add(shadingSurface);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fenestrationSurfaces"></param>
        /// <returns>The Elements object.</returns>
        public Elements AddFenestrationSurfaces(List<FenestrationSurface> fenestrationSurfaces)
        {
            foreach (var fenestrationSurface in fenestrationSurfaces)
            {
                FenestrationSurfaces.Add(fenestrationSurface);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shadingOverhangs"></param>
        /// <returns>The Elements object.</returns>
        public Elements AddShadingOverhangs(List<ShadingOverhang> shadingOverhangs)
        {
            foreach (var shadingOverhang in shadingOverhangs)
            {
                ShadingOverhangs.Add(shadingOverhang);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materials"></param>
        /// <returns>The Elements object.</returns>
        public Elements AddMaterials(List<AbsElement> materials)
        {            
            {
                foreach (var material in materials)
                {
                    Materials.Add(material);
                }
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="constructions"></param>
        /// <returns>The Elements object.</returns>
        public Elements AddConstructions(List<Construction> constructions)
        {
            foreach (var construction  in constructions)
            {
                Constructions.Add(construction);
            }
            return this;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            var temp = "Building: ";
            temp += (Building != null) ? Building + "\n" : "not defiend\n";
            temp += "Zones:" + Zones.Count + "\n";
            temp += "Surfaces:" + BuildingSurfaces.Count + "\n";
            temp += "Windows:" + FenestrationSurfaces.Count + "\n";
            temp += "Overhangs:" + ShadingOverhangs.Count + "\n";
            temp += "Shading Elements:" + ShadingSurfaces.Count + "\n";
            temp += "Materials:" + Materials.Count + "\n";
            temp += "Constructions:" + Constructions.Count;
            return temp;
        }

    }
}
