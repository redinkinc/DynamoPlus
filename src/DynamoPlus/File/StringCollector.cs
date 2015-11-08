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

using System.Linq;
using DynamoPlus.Geometry;
using DynamoPlus.System;

namespace DynamoPlus.File
{
    static class StringCollector
    {
        static internal string[] GetIdfString(Elements elements)
        {
            string[] settingStrings =
            {
                GetVersionString(),
                GetSimulationControlString(),
                GetProgramControlString(),
                GetTimestepString(),
                GetRunPeriodString(),
                GetGlobalGeometryRulesString(),
                GetGroundTemperaturesString(),
                GetOutputVariableDictionaryString(),
                GetOutputString()
            };

            var elementsStrings = GetElementsStrings(elements);
            
            return settingStrings.Concat(elementsStrings).ToArray();            
        }

        static private string[] GetElementsStrings(Elements elements)
        {
            var zoneList = "";
            var zones = "";
            var buildingSurfaces = "";
            var fenestrationSurfaces = "";
            var shadingSurfaces = "";
            var shadingOverhangs = "";
            var materials = "";
            var constructions = "";

            var hvacEquipmentConnections = "";
            var hvacEquipementLists = "";
            var hvacIdealLoads = "";

            var lights = "";
            var peoples = "";
            var electricEquipments = "";
            var thermostats = "";

            //Allways checks if the lists are full and writes a message in the idf-File if not
            var building = elements.Building == null ? "!No Building defined\n" : elements.Building.Write();

            if (elements.Zones.Count == 0)
            {
                //throw new ArgumentException("No Zones found");
                zones += "!No Zones defined\n";
            }

            foreach (var zone in elements.Zones)
            {
                //Downcast from NoDuplicate to Zone
                var realZone = (Zone) zone;
                zones += realZone.Write();

                var zc = new ZoneConnections(realZone);
                var eq = new EquipmentList(realZone);
                var ilas = new IdealLoadsAirSystem(realZone);

                hvacEquipmentConnections += zc.Write();
                hvacEquipementLists += eq.Write();
                hvacIdealLoads += ilas.Write();
            }

            zoneList = elements.ZoneLists.Aggregate(zoneList, (current, zList) => current + zList.Write());

            //handles Surfaces and BuildingSurfaces right now. Should be changed to BuildingSurfaces on the long run...
            if (elements.BuildingSurfaces.Count == 0)
            {
                buildingSurfaces += "!No Surfaces defined\n";     
            }
            else
            {
                buildingSurfaces += elements.BuildingSurfaces.Aggregate(buildingSurfaces,
                    (current, surface) => current + surface.Write());
            }

            if (elements.FenestrationSurfaces.Count == 0)
            {
                fenestrationSurfaces += "!No FenestrationSurfaces defined\n";
            }
            else
            {
                fenestrationSurfaces = elements.FenestrationSurfaces.Aggregate(fenestrationSurfaces, 
                    (current, fenestrationSurface) => current + fenestrationSurface.Write());
            }

            if (elements.ShadingSurfaces.Count == 0)
            {
                shadingSurfaces += "!No ShadingSurfaces defined\n";
            }
            else
            {
                shadingSurfaces = elements.ShadingSurfaces.Aggregate(shadingSurfaces, 
                    (current, shadingSurface) => current + shadingSurface.Write());
            }

            if (elements.ShadingOverhangs.Count == 0)
            {
                shadingOverhangs += "!No ShadingOverhang defined\n";
            }
            else
            {
                shadingOverhangs = elements.ShadingOverhangs.Aggregate(shadingOverhangs, 
                    (current, shadingOverhang) => current + shadingOverhang.Write());
            }

            if (elements.Materials.Count == 0)
            {
                materials += "!No Materials defined\n";
            }
            else
            {
                materials = elements.Materials.Aggregate(materials, 
                    (current, material) => current + material.Write());
            }

            if (elements.Constructions.Count == 0)
            {
                constructions += "!No Constructions defined\n";
            }
            else
            {
                constructions = elements.Constructions.Aggregate(constructions, 
                    (current, construction) => current + construction.Write());
            }

            if (elements.Lights.Count == 0)
            {
                lights += "!No Lights defined\n";
            }
            else
            {
                lights = elements.Lights.Aggregate(lights, (current, light) => current + light.Write());
            }

            if (elements.Peoples.Count == 0)
            {
                peoples += "!No People defined\n";
            }
            else
            {
                peoples = elements.Peoples.Aggregate(peoples, (current, people) => current + people.Write());
            }

            if (elements.ElectricEquipments.Count == 0)
            {
                electricEquipments += "!No ElectricEquipment defined\n";
            }
            else
            {
                electricEquipments = elements.ElectricEquipments.Aggregate(electricEquipments, (current, electricEquipment) => current + electricEquipment.Write());
            }

            if (elements.Thermostats.Count == 0)
            {
                thermostats += "!No Thermostats defined\n";
            }
            else
            {
                thermostats = elements.Thermostats.Aggregate(thermostats, (current, thermostat) => current + thermostat.Write());
            }

            string[] lines =
            {
                GetHeadLine("BUILDING")+building,
                GetHeadLine("ZONELIST")+zoneList,
                GetHeadLine("ZONE")+zones,
                GetHeadLine("SURFACE")+buildingSurfaces,
                GetHeadLine("FENESTRATIONSURFACE")+fenestrationSurfaces,
                GetHeadLine("SHADINGSURFACE")+shadingSurfaces,
                GetHeadLine("SHADINHOVERHANG")+shadingOverhangs,
                GetHeadLine("MATERIAL")+materials,
                GetHeadLine("CONSTRUCTION")+constructions,
                GetHeadLine("HVAC:EQUIPMENTCONNECTIONS")+hvacEquipmentConnections,
                GetHeadLine("HVAC:EQUIPMENTLIST")+hvacEquipementLists,
                GetHeadLine("HVAC:IDEALLOADS")+hvacIdealLoads,
                GetHeadLine("LIGHTS")+lights,
                GetHeadLine("PEOPLES")+peoples,
                GetHeadLine("ELECTRICEQUIPMENT")+electricEquipments,
                GetHeadLine("ZONECONTROL:THERMOSTATS")+thermostats
            };
            return lines;
        }

        
        static string GetHeadLine(string classname)
        {
            return "\n!-   ===========  ALL OBJECTS IN CLASS: " + classname + "===========\n\n";
        }

        private static string GetVersionString()
        {
            var versionText = GetHeadLine("VERSION");
            versionText += "Version,\n";
            versionText += "    8.3.0;                     !- Version Identifier\n";
            return versionText;
        }

        private static string GetSimulationControlString()
        {
            var text = GetHeadLine("SIMULATIONCONTROL");
            text += "SimulationControl,\n";
            text += "    No,                      !- Do Zone Sizing Calculation\n";
            text += "    No,                      !- Do System Sizing Calculation\n";
            text += "    No,                      !- Do Plant Sizing Calculation\n";
            text += "    No,                      !- Run Simulation for Sizing Periods\n";
            text += "    Yes;                      !- Run Simulation for Weather File Run Periods\n";
            return text;
        }

        private static string GetTimestepString()
        {
            var text = GetHeadLine("TIMESTEP");
            text += "Timestep, 6;                       !- Number of Timesteps per Hour\n";
            return text;
        }

        private static string GetProgramControlString()
        {
            var text = GetHeadLine("PROGRAMCONTROL");
            text += "ProgramControl, 1;                       !- Number of Threads Allowed\n";
            return text;
        }

        private static string GetRunPeriodString()
        {
            var text = GetHeadLine("RUNPERIOD");
            text += "RunPeriod,\n";
            text += "    Run Period 1,            !- Name\n";
            text += "    1,                       !- Begin Month\n";
            text += "    1,                       !- Begin Day of Month\n";
            text += "    12,                      !- End Month\n";
            text += "    31,                      !- End Day of Month\n";
            text += "    UseWeatherFile,          !- Day of Week for Start Day\n";
            text += "    No,                      !- Use Weather File Holidays and Special Days\n";
            text += "    Yes,                     !- Use Weather File Daylight Saving Period\n";
            text += "    No,                      !- Apply Weekend Holiday Rule\n";
            text += "    Yes,                     !- Use Weather File Rain Indicators\n";
            text += "    Yes,                     !- Use Weather File Snow Indicators\n";
            text += "    1;                       !- Number of Times Runperiod to be Repeated\n";
            return text;
        }

        private static string GetGroundTemperaturesString()
        {
            var text = GetHeadLine("GROUNDTEMPERATURES");
            text += "Site:GroundTemperature:BuildingSurface,18.3,18.2,18.3,18.4,20.1,22.0,22.3,22.5,22.5,20.7,18.9,18.5;\n";
            return text;
        }

        private static string GetGlobalGeometryRulesString()
        {
            var text = GetHeadLine("GLOBALGEOMETRYRULES");
            text += "GlobalGeometryRules,\n";
            text += "    UpperLeftCorner,         !- Starting Vertex Position\n";
            text += "    Counterclockwise,        !- Vertex Entry Direction\n";
            text += "    World,                   !- Coordinate System\n";
            text += "    Relative,                !- Daylighting Reference Point Coordinate System\n";
            text += "    World;                   !- Rectangular Surface Coordinate System\n";
            return text;
        }

        private static string GetOutputVariableDictionaryString()
        {
            var text = GetHeadLine("OUTPUT:VARIABLEDICTIONARY");
            text += "Output:VariableDictionary,\n";
            text += "    Idf,                     !- Key Field\n";
            text += "    Unsorted;                !- Sort Option\n";
            return text;
        }

        static private string GetOutputString()
        {
            var text = GetHeadLine("OUTPUT");
            text += "Output:Table:SummaryReports,\n";
            text += "    ZoneHeatingSummaryMonthly,  !- Report 3 Name\n";
            text += "    ZoneCoolingSummaryMonthly;  !- Report 4 Name\n\n";
            text += "    OutputControl:Table:Style,\n";
            text += "    Comma,                    !- Column Separator\n";
            text += "	 JtokWh;                  !- Unit Conversion\n\n";
            text += "    Output:Variable,*,Ideal Loads Air Heating Rate,RunPeriod;\n";
            text += "    Output:Variable,*,Ideal Loads Air Total Cooling Rate,RunPeriod;\n";
            text += "    ! HVAC,Sum,Zone Ideal Loads Supply Air Total Heating Energy [J]\n";
            text += "    ! HVAC,Sum,Zone Ideal Loads Supply Air Total Cooling Energy [J]\n\n";
            text += "    Output:Diagnostics,\n";
            text += "    ! DisplayExtraWarnings;    !- Key 1\n";
            text += "	 DisplayAllWarnings;    !- Key 1\n";
            return text;
        }

        internal static string[] GetSchedules(string directory)
        {
            var path = directory + @"\Schedules.template";
            return global::System.IO.File.ReadAllLines(path);

        }
    }
}