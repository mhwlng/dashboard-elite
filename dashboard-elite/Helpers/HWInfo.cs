using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace dashboard_elite.Helpers
{
    // copied from https://github.com/zipferot3000/HWiNFO-Shared-Memory-Dump/blob/master/Program.cs

    public class HWInfo
    {
        public enum SENSOR_TYPE
        {
            SENSOR_TYPE_NONE,
            SENSOR_TYPE_TEMP,
            SENSOR_TYPE_VOLT,
            SENSOR_TYPE_FAN,
            SENSOR_TYPE_CURRENT,
            SENSOR_TYPE_POWER,
            SENSOR_TYPE_CLOCK,
            SENSOR_TYPE_USAGE,
            SENSOR_TYPE_OTHER,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct _HWiNFO_SHARED_MEM
        {
            public uint Signature;
            public uint Version;
            public uint Revision;
            public long PollTime;
            public uint OffsetOfSensorSection;
            public uint SizeOfSensorElement;
            public uint NumSensorElements;
            public uint OffsetOfReadingSection;
            public uint SizeOfReadingElement;
            public uint NumReadingElements;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class _HWiNFO_SENSOR
        {
            public uint SensorId;
            public uint SensorInstance;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWINFO_SENSORS_STRING_LEN)]
            public string SensorNameOrig;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWINFO_SENSORS_STRING_LEN)]
            public string SensorNameUser;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class _HWiNFO_ELEMENT
        {
            public SENSOR_TYPE SensorType;
            public uint SensorIndex;
            public uint ElementId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWINFO_SENSORS_STRING_LEN)]
            public string LabelOrig;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWINFO_SENSORS_STRING_LEN)]
            public string LabelUser;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = HWINFO_UNIT_STRING_LEN)]
            public string Unit;
            public double Value;
            public double ValueMin;
            public double ValueMax;
            public double ValueAvg;
        }
        
        public class ElementObj
        {
            [JsonIgnore]
            public string ElementKey;

            public SENSOR_TYPE SensorType;

            public uint ElementId;
            public string LabelOrig;
            public string LabelUser;
            public string Unit;
            [JsonIgnore]
            public float NumericValue;
            public string Value;
            public string ValueMin;
            public string ValueMax;
            public string ValueAvg;
        }

        public class MQTTObj
        {
            public float Value;
            public string Unit;
        }


        public class SensorObj
        {
            public uint SensorId;
            public uint SensorInstance;

            public string SensorNameOrig;
            public string SensorNameUser;
            public Dictionary<string,ElementObj> Elements;
        }

        public static readonly object RefreshHWInfoLock = new object();

        private const string HWINFO_SHARED_MEM_FILE_NAME = "Global\\HWiNFO_SENS_SM2";
        private const int HWINFO_SENSORS_STRING_LEN = 128;
        private const int HWINFO_UNIT_STRING_LEN = 16;

        public Dictionary<int,SensorObj> FullSensorData = new Dictionary<int,SensorObj>();
        public Dictionary<int, SensorObj> SensorData = new Dictionary<int,SensorObj>();

        public Dictionary<string, ChartCircularBuffer> SensorTrends = new Dictionary<string, ChartCircularBuffer>();

        public IniParser.Model.IniData IncData = null;
      


        public void ReadMem(string incPath)
        {
            lock (RefreshHWInfoLock)
            {
                try
                {
                    var mmf = MemoryMappedFile.OpenExisting(HWINFO_SHARED_MEM_FILE_NAME, MemoryMappedFileRights.Read);
                    var accessor = mmf.CreateViewAccessor(0L, Marshal.SizeOf(typeof(_HWiNFO_SHARED_MEM)), MemoryMappedFileAccess.Read);
                    
                    accessor.Read(0L, out _HWiNFO_SHARED_MEM hWiNFOMemory);
                    
                    ReadSensors(mmf, hWiNFOMemory);

                    if (IncData == null)
                    {
                        incPath = Path.Combine(AppContext.BaseDirectory, incPath);

                        if (File.Exists(incPath))
                        {
                            var parser = new FileIniDataParser();

                            IncData = parser.ReadFile(incPath);
                        }
                    }

                    ParseIncFile();

                }
                catch 
                {
                    // don nothing
                }
            }
        }

        private void ReadSensors(MemoryMappedFile mmf, _HWiNFO_SHARED_MEM hWiNFOMemory)
        {
            for (var index = 0; index < hWiNFOMemory.NumSensorElements; ++index)
            {
                using (var viewStream = mmf.CreateViewStream(hWiNFOMemory.OffsetOfSensorSection + index * hWiNFOMemory.SizeOfSensorElement, hWiNFOMemory.SizeOfSensorElement, MemoryMappedFileAccess.Read))
                {
                    var buffer = new byte[(int)hWiNFOMemory.SizeOfSensorElement];
                    viewStream.Read(buffer, 0, (int)hWiNFOMemory.SizeOfSensorElement);
                    var gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    var structure = (_HWiNFO_SENSOR)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(_HWiNFO_SENSOR));
                    gcHandle.Free();
                    
                    if (!FullSensorData.ContainsKey(index))
                    {
                        var sensor = new SensorObj
                        {
                            SensorId = structure.SensorId,
                            SensorInstance = structure.SensorInstance,
                            SensorNameOrig = structure.SensorNameOrig,
                            SensorNameUser = structure.SensorNameUser,
                            Elements = new Dictionary<string, ElementObj>()
                        };

                        FullSensorData.Add(index,sensor);
                    }
                    
                }
            }
            
            ReadElements(mmf, hWiNFOMemory);
        }

        private void ReadElements(MemoryMappedFile mmf, _HWiNFO_SHARED_MEM hWiNFOMemory)
        {
            for (uint index = 0; index < hWiNFOMemory.NumReadingElements; ++index)
            {
                using (var viewStream = mmf.CreateViewStream(hWiNFOMemory.OffsetOfReadingSection + index * hWiNFOMemory.SizeOfReadingElement, hWiNFOMemory.SizeOfReadingElement, MemoryMappedFileAccess.Read))
                {
                    var buffer = new byte[(int)hWiNFOMemory.SizeOfReadingElement];
                    viewStream.Read(buffer, 0, (int)hWiNFOMemory.SizeOfReadingElement);
                    var gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    var structure = (_HWiNFO_ELEMENT)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(_HWiNFO_ELEMENT));
                    gcHandle.Free();

                    var sensor = FullSensorData[(int) structure.SensorIndex];

                    var elementKey = sensor.SensorId + "-" + sensor.SensorInstance + "-" + structure.ElementId;

                    var element = new ElementObj
                    {
                        ElementKey = elementKey,

                        SensorType = structure.SensorType,
                        ElementId = structure.ElementId,
                        LabelOrig = structure.LabelOrig,
                        LabelUser = structure.LabelUser,
                        Unit = structure.Unit,
                        NumericValue = (float)structure.Value,
                        Value = structure.SensorType.NumberFormat(structure.Unit, structure.Value),
                        ValueMin = structure.SensorType.NumberFormat(structure.Unit, structure.ValueMin),
                        ValueMax = structure.SensorType.NumberFormat(structure.Unit, structure.ValueMax),
                        ValueAvg = structure.SensorType.NumberFormat(structure.Unit,structure.ValueAvg)
                    };

                    sensor.Elements[elementKey] = element;
                }
            }
        }

        private void ParseIncFile()
        {
            if (IncData != null && FullSensorData.Any())
            {
                var serverName = Environment.MachineName;

                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };


                int index = -1;

                foreach (var section in IncData.Sections.Where(x => x.SectionName != "Variables"))
                {
                    index++;

                    var sectionName = Regex.Replace(section.SectionName, "HWINFO-CONFIG-", "", RegexOptions.IgnoreCase);

                    foreach (KeyData key in section.Keys)
                    {
                        var elementName = key.Value;

                        var sensorIdStr = IncData["Variables"][key.KeyName + "-SensorId"];
                        var sensorInstanceStr = IncData["Variables"][key.KeyName + "-SensorInstance"];
                        var elementIdStr = IncData["Variables"][key.KeyName + "-EntryId"];

                        if (sensorIdStr?.StartsWith("0x") == true && sensorInstanceStr?.StartsWith("0x") == true &&
                            elementIdStr?.StartsWith("0x") == true)
                        {
                            var sensorId = Convert.ToUInt32(sensorIdStr.Replace("0x", ""), 16);
                            var sensorInstance = Convert.ToUInt32(sensorInstanceStr.Replace("0x", ""), 16);
                            var elementId = Convert.ToUInt32(elementIdStr.Replace("0x", ""), 16);

                            var fullSensorDataSensor = FullSensorData.Values.FirstOrDefault(x =>
                                x.SensorId == sensorId && x.SensorInstance == sensorInstance);

                            var elementKey = sensorId + "-" + sensorInstance + "-" + elementId;

                            if (fullSensorDataSensor?.Elements.ContainsKey(elementKey) == true)
                            {
                                var fullSensorDataElement = fullSensorDataSensor.Elements[elementKey];

                                var element = new ElementObj
                                {
                                    ElementKey = elementKey,

                                    SensorType = fullSensorDataElement.SensorType,
                                    ElementId = fullSensorDataElement.ElementId,
                                    LabelOrig = elementName,
                                    LabelUser = elementName,
                                    Unit = fullSensorDataElement.Unit,
                                    NumericValue = fullSensorDataElement.NumericValue,
                                    Value = fullSensorDataElement.Value,
                                    ValueMin = fullSensorDataElement.ValueMin,
                                    ValueMax = fullSensorDataElement.ValueMax,
                                    ValueAvg = fullSensorDataElement.ValueAvg
                                };

                                if (!SensorData.ContainsKey(index))
                                {
                                    var sensor = new SensorObj
                                    {
                                        SensorId = 0,
                                        SensorInstance = 0,
                                        SensorNameOrig = sectionName,
                                        SensorNameUser = sectionName,
                                        Elements = new Dictionary<string, ElementObj>()
                                    };

                                    SensorData.Add(index, sensor);
                                }
                                
                                SensorData[index].Elements[elementKey] = element;

                                var t1 = serverName.Replace(' ', '_');
                                var t2 = SensorData[index].SensorNameUser.Replace(' ', '_');
                                var t3 = element.LabelUser.Replace(' ', '_');

                                /*!!!!!!!!!!!var task = Task.Run<bool>(async () => await MQTT.Publish($"HWINFO/{t1}/{t2}/{t3}", 
                                    JsonConvert.SerializeObject(new MQTTObj
                                        {
                                            Value = element.NumericValue,
                                            Unit = element.Unit
                                        }, 
                                        new JsonSerializerSettings
                                        {
                                            ContractResolver = contractResolver
                                        })));*/

                                if (!SensorTrends.ContainsKey(elementKey))
                                {
                                    SensorTrends.Add(elementKey, new ChartCircularBuffer(fullSensorDataElement.SensorType, fullSensorDataElement.Unit));
                                }

                                SensorTrends[elementKey].Put(fullSensorDataElement.NumericValue);

                            }
                        }

                    }

                }
            }

        }

        public void SaveDataToFile(string path)
        {
            path = Path.Combine(AppContext.BaseDirectory, path);

            using (var fs = File.Create(path))
            {
                var json = new UTF8Encoding(true).GetBytes(JsonConvert.SerializeObject(FullSensorData, Formatting.Indented));
                fs.Write(json, 0, json.Length);
            }
        }

    }
}
