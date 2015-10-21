using System.Collections.Generic;
using System.IO;
using System.Linq;

/*namespace IDF4DynamoNodes
{
      
    public class Modules
    {
        public IDFmodules Module { get; set; }
        public Modules(int n, double depth, int storys)
        {
            Module = new IDFmodules(n, depth, storys);

            double width = 5.5;

            for (int i = 0; i < n; i++)
            {
                Module.AddSingleOffice(width);
            }
        }

        public List<Autodesk.DesignScript.Geometry.PolyCurve> CreateGeometry(IDFparser parser)//IDFmodules module)
        {
            

            if (parser.Zones.Count == 0)
            {
                parser.CreateGeometry(Module);
            }

            var rectangles = new List<Autodesk.DesignScript.Geometry.PolyCurve>();

            foreach (Surface surface in parser.Surfaces)
	        {
                var point_list = surface.PointList.Select(p => Autodesk.DesignScript.Geometry.Point.ByCoordinates(p.X, p.Y, p.Z)).ToList();
                rectangles.Add(Autodesk.DesignScript.Geometry.PolyCurve.ByPoints(point_list, true));
            }

            foreach (FenestrationSurface window in parser.Windows)
            {
                var point_list = window.PointList.Select(p => Autodesk.DesignScript.Geometry.Point.ByCoordinates(p.X, p.Y, p.Z)).ToList();
                rectangles.Add(Autodesk.DesignScript.Geometry.PolyCurve.ByPoints(point_list, true));
            }

            return rectangles;
        
        }


        public IDFparser Parse(string template)
        {

            var parser = new IDFparser();

            parser.Parse(Module);

            return parser;
        }

        public void WriteIDF(IDFparser parser, string filepath)
        {
            parser.OutputFilepath = filepath;

            parser.Write2File();
        }

        static public Dictionary<string, double[]> ReadResults(string path = @"C:\Users\Florian\Documents\Arbeit\idf4dynamo\CreateIDF4Dynamo")
        {
            var dict = new Dictionary<string, double[]>();
            var files = Directory.GetFiles(path, "*.csv");

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);

                var energyUse = "cooling";
                var heatingEnergy = 0.0;
                var coolingEnergy = 0.0;

                foreach (var line in lines)
                {

                    if (line.Contains("ZoneCoolingSummaryMonthly")) energyUse = "cooling";
                    else if (line.Contains("ZoneHeatingSummaryMonthly")) energyUse = "heating";

                    if (line.Contains("Annual Sum or Average"))
                    {
                        string[] value = line.Split(',');
                        var amount = double.Parse(value[2]);

                        switch (energyUse)
                        {
                            case "cooling":
                                coolingEnergy += amount;
                                break;
                            case "heating":
                                heatingEnergy += amount;
                                break;
                            default:
                                break;
                        }

                    }

                }
                
                double[] energyDemand = {coolingEnergy / 100, heatingEnergy / 100};
                dict.Add(file, energyDemand);
            }

            return dict;

        }

    }
}

*/