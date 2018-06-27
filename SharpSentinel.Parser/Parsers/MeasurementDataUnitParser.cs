using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SharpSentinel.Parser.Extensions;
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace SharpSentinel.Parser.Parsers
{
    public static class MeasurementDataUnitParser
    {
        public static IList<MeasurementDataUnit> Parse(XmlNode informationPackageMap, XmlNode dataObjectSection, XmlNamespaceManager manager, DirectoryInfo baseDirectory)
        {
            var measurementDataUnits = new List<MeasurementDataUnit>();
            var measurementDataUnitNodes = informationPackageMap.SelectNodes("xfdu:contentUnit/xfdu:contentUnit[@unitType='Measurement Data Unit']", manager);
            
            foreach (var currentMeasurementDataUnitNode in measurementDataUnitNodes.Cast<XmlNode>())
            {
                var measurementDataUnit = new MeasurementDataUnit();

                var dataObjectId = currentMeasurementDataUnitNode
                    .SelectSingleNode("dataObjectPointer", manager)
                    .Attributes
                    .GetNamedItem("dataObjectID")
                    .Value;

                var repId = currentMeasurementDataUnitNode
                    .Attributes
                    .GetNamedItem("repID")
                    .Value;

                if (repId == "s1Level1MeasurementSchema")
                {
                    measurementDataUnit.MeasurementDataUnitType = MeasurementDataUnitType.Measurement;

                    var dmdIds = currentMeasurementDataUnitNode
                        .Attributes
                        .GetNamedItem("dmdID")
                        .Value
                        .Split(' ')
                        .Where(f => string.IsNullOrWhiteSpace(f) == false);


                }
                else if (repId == "s1Level1QuickLookSchema")
                {
                    measurementDataUnit.MeasurementDataUnitType = MeasurementDataUnitType.QuickLook;
                }
                else
                {
                    throw new XmlException("Found unknown measurement data unit");
                }

                var dataObjectNode = dataObjectSection.SelectedDataObjectById(dataObjectId);

                var fileLocationRaw = dataObjectNode
                    .SelectSingleNode("byteStream/fileLocation")
                    .Attributes
                    .GetNamedItem("href")
                    .Value;

                measurementDataUnit.File = new FileInfo(Path.Combine(baseDirectory.FullName, fileLocationRaw.Remove(0, 2).Replace(@"/", @"\")));

                measurementDataUnit.ChecksumName = dataObjectNode
                    .SelectSingleNode("byteStream/checksum")
                    .Attributes
                    .GetNamedItem("checksumName")
                    .Value;

                measurementDataUnit.Checksum = dataObjectNode.SelectSingleNode("byteStream/checksum").InnerText;

                measurementDataUnits.Add(measurementDataUnit);
            }

            return measurementDataUnits;
        }
    }
}