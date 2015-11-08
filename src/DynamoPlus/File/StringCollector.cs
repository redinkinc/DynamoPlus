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
                GetGroundTemperaturesString()
            };

            string[] hvacOutput =
            {    
                GetLightsString(),
                GetElectricEquipmentString(),
                GetZoneControlThermostatString(),
                GetThermostatSetpointDualSetpointString(),
                GetOutputVariableDictionaryString(),
                GetOutputString()
            };

            var elementsStrings = GetElementsStrings(elements);
            
            return settingStrings.Concat(elementsStrings.Concat(hvacOutput)).ToArray();            
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
                fenestrationSurfaces = elements.FenestrationSurfaces.Aggregate(fenestrationSurfaces, (current, fenestrationSurface) => current + fenestrationSurface.Write());
            }

            if (elements.ShadingSurfaces.Count == 0)
            {
                shadingSurfaces += "!No ShadingSurfaces defined\n";
            }
            else
            {
                shadingSurfaces = elements.ShadingSurfaces.Aggregate(shadingSurfaces, (current, shadingSurface) => current + shadingSurface.Write());
            }

            if (elements.ShadingOverhangs.Count == 0)
            {
                shadingOverhangs += "!No ShadingOverhang defined\n";
            }
            else
            {
                shadingOverhangs = elements.ShadingOverhangs.Aggregate(shadingOverhangs, (current, shadingOverhang) => current + shadingOverhang.Write());
            }

            if (elements.Materials.Count == 0)
            {
                materials += "!No Materials defined\n";
            }
            else
            {
                materials = elements.Materials.Aggregate(materials, (current, material) => current + material.Write());
            }

            if (elements.Constructions.Count == 0)
            {
                constructions += "!No Constructions defined\n";
            }
            else
            {
                constructions = elements.Constructions.Aggregate(constructions, (current, construction) => current + construction.Write());
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
                GetHeadLine("HVAYEQUIPMENTCONNECTIONS")+hvacEquipmentConnections,
                GetHeadLine("HVACEQUIPMENTLIST")+hvacEquipementLists,
                GetHeadLine("HVACIDEALLOADS")+hvacIdealLoads
            };
            return lines;
        }

        
        static string GetHeadLine(string classname)
        {
            return "\n!-   ===========  ALL OBJECTS IN CLASS: " + classname + "===========\n\n";
        }

        static private string GetVersionString()
        {
            var versionText = GetHeadLine("VERSION");
            versionText += "Version,\n";
            versionText += "    8.3.0;                     !- Version Identifier\n";
            return versionText;
        }

        static private string GetSimulationControlString()
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

        static private string GetTimestepString()
        {
            var text = GetHeadLine("TIMESTEP");
            text += "Timestep, 6;                       !- Number of Timesteps per Hour\n";
            return text;
        }

        static private string GetProgramControlString()
        {
            var text = GetHeadLine("PROGRAMCONTROL");
            text += "ProgramControl, 1;                       !- Number of Threads Allowed\n";
            return text;
        }

        static private string GetRunPeriodString()
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

        static private string GetGroundTemperaturesString()
        {
            var text = GetHeadLine("GROUNDTEMPERATURES");
            text += "Site:GroundTemperature:BuildingSurface,18.3,18.2,18.3,18.4,20.1,22.0,22.3,22.5,22.5,20.7,18.9,18.5;\n";
            return text;
        }

        static private string GetGlobalGeometryRulesString()
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

        static private string GetOutputVariableDictionaryString()
        {
            var text = GetHeadLine("OUTPUT:VARIABLEDICTIONARY");
            text += "Output:VariableDictionary,\n";
            text += "    Idf,                     !- Key Field\n";
            text += "    Unsorted;                !- Sort Option\n";
            return text;
        }

        private static string GetLightsString()
        {
            var text = GetHeadLine("LIGHTS");
            text += "Lights,\n";
            text += "    dinv18599 - 10 Grossraumbuero LightsInst,  !-Name\n";
            text += "    DIN V 18599 - 10 A.3 Grossraumbuero,  !-Zone or ZoneList Name\n";
            text += "    Beleuchtung - dinv18599 - 10a3,!-Schedule Name\n";
            text += "    Watts / Area,            !-Design Level Calculation Method\n";
            text += "    ,                        !-Lighting Level {W}\n";
            text += "    9.68752354606417,        !-Watts per Zone Floor Area {W/m2}\n";
            text += "    ,                        !-Watts per Person {W/person}\n";
            text += "    ,                        !-Return Air Fraction\n";
            text += "    ,                        !-Fraction Radiant\n";
            text += "    ,                        !-Fraction Visible\n";
            text += "    ,                        !-Fraction Replaceable\n";
            text += "    Lights; !-End - Use Subcategory\n";
            return text;
        }

        private static string GetElectricEquipmentString()
        {
            var text = GetHeadLine("ELECTRICEQUIPMENT");
            text += "ElectricEquipment,\n";
            text += "    dinv18599 -10 Grossraumbuero ElecInst,  !- Name\n";
            text += "    DIN V 18599-10 A.3 Grossraumbuero,  !- Zone or ZoneList Name\n";
            text += "    BetriebszeitHZG -dinv18599-10a3,!- Schedule Name\n";
            text += "    Watts /Area,              !- Design Level Calculation Method\n";
            text += "    ,                        !- Design Level { W }\n";
            text += "    5.8125141276385,         !- Watts per Zone Floor Area {W/m2}\n";
            text += "    ,                        !- Watts per Person {W/person}\n";
            text += "    ,                        !- Fraction Latent\n";
            text += "    ,                        !- Fraction Radiant\n";
            text += "    ,                        !- Fraction Lost\n";
            text += "    ElectricEquipment;       !- End-Use Subcategory\n";
            return text;
        }

        private static string GetZoneControlThermostatString()
        {
            var text = GetHeadLine("ZONECONTROL:THERMOSTAT");
            text += "ZoneControl:Thermostat,\n";
            text += "    ZONE ONE Thermostat,     !- Name\n";
            text += "    DIN V 18599-10 A.3 Grossraumbuero,                !- Zone or ZoneList Name\n";
            text += "    ALWAYS 4,                !- Control Type Schedule Name\n";
            text += "    ThermostatSetpoint:DualSetpoint,  !- Control 1 Object Type\n";
            text += "    Office Thermostat Dual SP Control;  !- Control 1 Name\n";
            return text;
        }

        private static string GetThermostatSetpointDualSetpointString()
        {
            var text = GetHeadLine("THERMOSTATSETPOINT:DUALSETPOINT");
            text += "ThermostatSetpoint:DualSetpoint,\n";
            text += "    Office Thermostat Dual SP Control,  !- Name\n";
            text += "    ALWAYS 20,               !- Heating Setpoint Temperature Schedule Name\n";
            text += "    ALWAYS 24;               !- Cooling Setpoint Temperature Schedule Name\n";
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