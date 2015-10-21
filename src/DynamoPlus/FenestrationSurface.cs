using System;
using System.Collections.Generic;
using System.Globalization;
using Autodesk.DesignScript.Geometry;

namespace DynamoPlus
{
    //At the Moment to build Standard Windows, but easily extendable to other windows or doors, etc.
    public class FenestrationSurface
    {
        public string Name { get; set; }
        public List<Point> PointList { get; set; }
        public string Type { get; set; }
        public string ConstructionName { get; set; }
        public string SurfaceName { get; set; }
        public ShadingOverhang shadingOverhang { get; set; }
        public Surface _surface { get; set; }

        public FenestrationSurface(Surface surface, List<Point> points)
        {
            Name = surface.Name + " - Window " + surface.FenestrationSurfacesNumber.ToString();

            Type = "Window";
            ConstructionName = "000 Standard Window";
            SurfaceName = surface.Name;

            PointList = points;
            //CheckAngle(surface);
            _surface = surface;
            surface.FenestrationSurfacesNumber++;
        }

        //Get 'Fenestration Surface by Selected Surface
        public static List<FenestrationSurface> FenestrationSurfaceBySurfacelist (List<Face> facelist, Surface surface)
        {
            var fenestrationsurfaceList = new List<FenestrationSurface>();
            

            foreach (var face in facelist)
            {
                var points = new List<Point>();

                foreach (var RevitVertex in face.Vertices)
                {
                    //Translate Vertex to Point. Can't find a way to avoid this. Deviding through 1000 because of differents units(Revit->EPlus)
                    //Don't know if this is right
                    var point = Point.ByCoordinates(RevitVertex.PointGeometry.X/1000,
                        RevitVertex.PointGeometry.Y/1000, RevitVertex.PointGeometry.Z/1000);
                    points.Add(point);
                }
                fenestrationsurfaceList.Add(new FenestrationSurface(surface, points));
            }
          
           return fenestrationsurfaceList;
        }

        //Checks if the angle of the window and the surface is the same and rotates the window if not
        //internal void CheckAngle(Surface surface)
        //{
        //    var windowNormal = GetNormal();
        //    var surfaceNormal = surface.GetNormal();

        //    var angle = windowNormal.GetAngle(surfaceNormal);

        //    angle = angle % (2 * Math.PI);

        //    if (angle != 0)
        //    {
        //        Rotate();
        //    }
        //}
        //Finds the normal vector to compare it with the surface. Could later also be used to check if the direction is right 
        //internal CreateIDF4Dynamo.Vector GetNormal()
        //{
        //    var p1 = PointList[0];
        //    var p2 = PointList[1];
        //    var p3 = PointList[2];

        //    var v1 = new Vector(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        //    var v2 = new Vector(p2.X - p3.X, p2.Y - p3.Y, p2.Z - p3.Z);

        //    var normal = v1.CrossProduct(v2);

        //    return normal;
        //}

        //internal void Rotate()
        //{
        //    var temp = PointList[1];
        //    PointList[1] = PointList[3];
        //    PointList[3] = temp;
        //}

        // Writes the data into one string
        public override string ToString()
        {
            var temp = "\nFenestrationSurface:Detailed,\n";
            temp += Name + ",  !Name\n";
            temp += Type + ",  !Surface Type\n";
            temp += ConstructionName + ",  !Construction Name\n";
            temp += SurfaceName + ",  !Building Surface Name\n";
            temp += ",        !- Outside Boundary Condition Object\n";
            temp += ",        !- View Factor to Ground\n";
            temp += ",        !- Shading Control Name\n";
            temp += ",        !- Frame and Divider Name\n";
            temp += ",        !- Multiplier\n";
            temp += ",        !- Number of Vertices\n";
            for (var i = 0; i < PointList.Count; i++)
            {
                temp += PointList[i].X.ToString().Replace(",", ".") + ", " + PointList[i].Y.ToString().Replace(",", ".") + ", " + PointList[i].Z.ToString().Replace(",", ".");
                if (i != PointList.Count - 1)
                {
                    temp += ", ";
                }
                else
                {
                    temp += ";\n";
                }
            }

            return temp;
        }
    }
}
