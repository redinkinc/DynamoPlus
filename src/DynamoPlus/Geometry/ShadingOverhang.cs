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
using System.Globalization;
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Runtime;

namespace DynamoPlus.Geometry
{
    /// <summary>
    /// Used to define Shading Overhangs above a FenestrationSurface
    /// </summary>

    public class ShadingOverhang : AbsElement
    {
        FenestrationSurface FenestrationSurface { get; set; }
        double Height { get; set; }
        double Extension { get; set; }
        double Depth { get; set; }
        double TiltAngle { get; set; }
      
        /// <summary>
        /// Adds a symmetrical Shading Overhang above a FenestrationSurface with an Angle of 90 degree.
        /// </summary>
        /// <param name="fenestrationSurface"></param>
        /// <param name="heightAbove"></param>
        /// <param name="extension">Extension szmmetrical to both sides</param>
        /// <param name="depth"></param>
        public ShadingOverhang(FenestrationSurface fenestrationSurface, double heightAbove, double extension, double depth)
        {
            FenestrationSurface = fenestrationSurface;
            Height = heightAbove;
            Extension = extension;
            Depth = depth;
            TiltAngle = 90;
            Name = fenestrationSurface.Name + " ShadingOverhang";
            fenestrationSurface.ShadingOverhang = this;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Name + ";";
        }

        /// <summary>
        ///  Writes all Information of the Building object into a string. 
        /// </summary>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(false)]
        public override string Write()
        {
            var temp = "\nShading:Overhang,\n";
            temp += Name + ",  !Name\n";
            temp += FenestrationSurface.Name + ",  !fenestrationSurfaceName\n";
            temp += Height.ToString(CultureInfo.InvariantCulture) + ",        !- height above Window or Door\n";
            temp += TiltAngle.ToString(CultureInfo.InvariantCulture) + ",        !- Tilt Angle\n";
            temp += Extension.ToString(CultureInfo.InvariantCulture) + ",        !- Left Extension from Window/Door Width\n";
            temp += Extension.ToString(CultureInfo.InvariantCulture) + ",        !- Right Extension from Window/Door Width\n";
            temp += Depth.ToString(CultureInfo.InvariantCulture) + ",        !- Depth\n";

            return temp;
        }


        /// <summary>
        /// Draws the shading overhang by a list of Overhangs
        /// </summary>
        /// <param name="shadingOverhangsList"></param>
        /// <returns></returns>
        private static List<PolyCurve> Draw(List<ShadingOverhang> shadingOverhangsList)
        {
            var rectangles = new List<PolyCurve>();

            foreach (ShadingOverhang shadingOverhang in shadingOverhangsList)
            {
                //expects fenestrationSurface to be defined like:   3   2 
                //                                                  0   1
                //find a coordinateSystem where the fenestration Surface lies in 
                var vec1 = Vector.ByTwoPoints(shadingOverhang.FenestrationSurface.Surface.Vertices[0].PointGeometry, shadingOverhang.FenestrationSurface.Surface.Vertices[1].PointGeometry);
                var vec2 = Vector.ByTwoPoints(shadingOverhang.FenestrationSurface.Surface.Vertices[0].PointGeometry, shadingOverhang.FenestrationSurface.Surface.Vertices[3].PointGeometry);
                var length = vec1.Length;
                //calculate new points for the ShadingOverhang
                var points = new List<Point>();
                var coordSystem = CoordinateSystem.ByOriginVectors(shadingOverhang.FenestrationSurface.Surface.Vertices[3].PointGeometry, vec1, vec2);
                points.Add(Point.ByCartesianCoordinates(coordSystem, -shadingOverhang.Extension, shadingOverhang.Height));
                points.Add(Point.ByCartesianCoordinates(coordSystem, -shadingOverhang.Extension, shadingOverhang.Height,shadingOverhang.Depth));
                points.Add(Point.ByCartesianCoordinates(coordSystem, length + shadingOverhang.Extension, shadingOverhang.Height, shadingOverhang.Depth));
                points.Add(Point.ByCartesianCoordinates(coordSystem, length + shadingOverhang.Extension, shadingOverhang.Height));
                PolyCurve rectangle = PolyCurve.ByPoints(points,true);
                rectangles.Add(rectangle);
                
            }//foreach

            return rectangles;
        }//Draw ShadingOverhang
    } //class
}//namespace

