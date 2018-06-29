using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Data.S1.Annotations;
using SharpSentinel.Parser.Helpers;
// ReSharper disable PossibleNullReferenceException

namespace SharpSentinel.Parser.Parsers.Annotations
{
    public static class CalibrationAnnotationParser
    {
        public static CalibriationAnnotation Parse([NotNull]FileInfo fileInfo, [CanBeNull] Checksum checkSum)
        {
            Guard.NotNullAndValidFileSystemInfo(fileInfo, nameof(fileInfo));

            var calibrationAnnotation = new CalibriationAnnotation
            {
                File = fileInfo,
                Checksum = checkSum,
                CalibrationVectors = new List<CalibrationVector>()
            };

            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(fileStream);

                var calibrationNode = document.SelectSingleNode("calibration");

                calibrationAnnotation.RawXML = document.InnerXml;

                calibrationAnnotation.AdsHeader = AdsHeaderParser.Parse(calibrationNode.SelectSingleNode("adsHeader"));
                calibrationAnnotation.AbsoluteCalibrationConstant = double.Parse(calibrationNode.SelectSingleNode("calibrationInformation/absoluteCalibrationConstant").InnerText);
                calibrationAnnotation.CalibrationVectors = CalibrationVectorParser.Parse(calibrationNode.SelectSingleNode("calibrationVectorList"));
            }

            return calibrationAnnotation;
        }

        public static class CalibrationVectorParser
        {
            public static IList<CalibrationVector> Parse(XmlNode calibrationVectorListNode)
            {
                Guard.NotNull(calibrationVectorListNode, nameof(calibrationVectorListNode));

                var items = new List<CalibrationVector>();

                var calibrationVectorNodes = calibrationVectorListNode.SelectNodes("calibrationVector");

                foreach (var currentCalibrationVectorNode in calibrationVectorNodes.Cast<XmlNode>())
                {
                    var calibrationVector = new CalibrationVector
                    {
                        Values = new List<CalibratinoVectorValue>()
                    };

                    calibrationVector.AzimuthTime = DateTimeOffset.Parse(currentCalibrationVectorNode.SelectSingleNode("azimuthTime").InnerText);
                    calibrationVector.Line = int.Parse(currentCalibrationVectorNode.SelectSingleNode("line").InnerText);

                    var allPixels = currentCalibrationVectorNode
                        .SelectSingleNode("pixel")
                        .InnerText
                        .Split(' ')
                        .Where(f => string.IsNullOrWhiteSpace(f) == false)
                        .Select(int.Parse)
                        .ToList();

                    var allSigmaNoughts = currentCalibrationVectorNode
                        .SelectSingleNode("sigmaNought")
                        .InnerText
                        .Split(' ')
                        .Where(f => string.IsNullOrWhiteSpace(f) == false)
                        .Select(double.Parse)
                        .ToList();
                    
                    var allBetaNoughts = currentCalibrationVectorNode
                        .SelectSingleNode("betaNought")
                        .InnerText
                        .Split(' ')
                        .Where(f => string.IsNullOrWhiteSpace(f) == false)
                        .Select(double.Parse)
                        .ToList();

                    var allGammas = currentCalibrationVectorNode
                        .SelectSingleNode("gamma")
                        .InnerText
                        .Split(' ')
                        .Where(f => string.IsNullOrWhiteSpace(f) == false)
                        .Select(double.Parse)
                        .ToList();

                    var allDns = currentCalibrationVectorNode
                        .SelectSingleNode("dn")
                        .InnerText
                        .Split(' ')
                        .Where(f => string.IsNullOrWhiteSpace(f) == false)
                        .Select(double.Parse)
                        .ToList();

                    for (var i = 0; i < allPixels.Count; i++)
                    {
                        var vectorValue = new CalibratinoVectorValue
                        {
                            Pixel = allPixels[i],
                            SigmaNought = allSigmaNoughts[i],
                            BetaNought = allBetaNoughts[i],
                            Gamma = allGammas[i],
                            Dn = allDns[i],
                        };

                        calibrationVector.Values.Add(vectorValue);
                    }

                    items.Add(calibrationVector);
                }

                return items;
            }
        }
    }
}