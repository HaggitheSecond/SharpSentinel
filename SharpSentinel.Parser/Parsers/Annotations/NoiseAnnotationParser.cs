using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.Common;
using SharpSentinel.Parser.Data.ManifestObjects;
using SharpSentinel.Parser.Data.S1;
using SharpSentinel.Parser.Data.S1.Annotations;
using SharpSentinel.Parser.Extensions;
using SharpSentinel.Parser.Helpers;
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace SharpSentinel.Parser.Parsers.Annotations
{
    public static class NoiseAnnotationParser
    {
        public static NoiseAnnotation Parse([NotNull]FileInfo fileInfo, [CanBeNull]Checksum checkSum)
        {
            Guard.NotNullAndValidFileSystemInfo(fileInfo, nameof(fileInfo));

            var noiseAnnotation = new NoiseAnnotation
            {
                File = fileInfo,
                Checksum = checkSum,
                NoiseRangeVectors = new List<NoiseRangeVector>(),
                NoiseAzimuthVectors = new List<NoiseAzimuthVector>()
            };

            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(fileStream);

                noiseAnnotation.RawXml = document.InnerXml;

                var noiseNode = document.SelectSingleNodeThrowIfNull("noise");

                noiseAnnotation.AdsHeader = AdsHeaderParser.Parse(noiseNode.SelectSingleNodeThrowIfNull("adsHeader"));
                noiseAnnotation.NoiseRangeVectors = NoiseRangeVectorParser.Parse(noiseNode.SelectSingleNodeThrowIfNull("noiseRangeVectorList"));
                noiseAnnotation.NoiseAzimuthVectors = NoiseAzimuthVectorParser.Parse(noiseNode.SelectSingleNodeThrowIfNull("noiseAzimuthVectorList"));
            }

            return noiseAnnotation;
        }

        public static class NoiseRangeVectorParser
        {
            public static IList<NoiseRangeVector> Parse(XmlNode noiseRangeVectorListNode)
            {
                Guard.NotNull(noiseRangeVectorListNode, nameof(noiseRangeVectorListNode));

                var items = new List<NoiseRangeVector>();

                var noiseRangeVectorNodes = noiseRangeVectorListNode.SelectNodes("noiseRangeVector");

                foreach (var currentNoiseRangeVectorNode in noiseRangeVectorNodes.Cast<XmlNode>())
                {
                    var noiseRangeVector = new NoiseRangeVector
                    {
                        Values = new List<NoiseRangeVectorValue>()
                    };

                    noiseRangeVector.AzimuthTime = DateTimeOffset.Parse(currentNoiseRangeVectorNode.SelectSingleNodeThrowIfNull("azimuthTime").InnerText);
                    noiseRangeVector.Line = int.Parse(currentNoiseRangeVectorNode.SelectSingleNodeThrowIfNull("line").InnerText);

                    var allPixels = currentNoiseRangeVectorNode
                        .SelectSingleNodeThrowIfNull("pixel")
                        .InnerText
                        .Split(' ')
                        .Where(f => string.IsNullOrWhiteSpace(f) == false)
                        .Select(int.Parse)
                        .ToList();

                    var allNoiseRangeLut = currentNoiseRangeVectorNode
                        .SelectSingleNodeThrowIfNull("noiseRangeLut")
                        .InnerText
                        .Split(' ')
                        .Where(f => string.IsNullOrWhiteSpace(f) == false)
                        .Select(double.Parse)
                        .ToList();

                    if (allNoiseRangeLut.Count != allPixels.Count)
                        throw new XmlException("Missing pixels or noiseRangeLuts");

                    for (var i = 0; i < allPixels.Count; i++)
                    {
                        noiseRangeVector.Values.Add(new NoiseRangeVectorValue
                        {
                            Pixel = allPixels[i],
                            NoiseRangeLut = allNoiseRangeLut[i]
                        });
                    }

                    items.Add(noiseRangeVector);
                }

                return items;
            }
        }

        public static class NoiseAzimuthVectorParser
        {
            public static IList<NoiseAzimuthVector> Parse(XmlNode noiseAzimuthVectorListNode)
            {
                var items = new List<NoiseAzimuthVector>();

                var noiseAzimuthVectorNodes = noiseAzimuthVectorListNode.SelectNodes("noiseAzimuthVector");

                foreach (var currentNoiseAzimuthVectorNode in noiseAzimuthVectorNodes.Cast<XmlNode>())
                {
                    var noiseAzimuthVector = new NoiseAzimuthVector
                    {
                        Values = new List<NoiseAzimuthVectorValue>()
                    };

                    noiseAzimuthVector.Swath = (SwathType)Enum.Parse(typeof(SwathType), currentNoiseAzimuthVectorNode.SelectSingleNodeThrowIfNull("swath").InnerText);

                    noiseAzimuthVector.FirstAzimuthLine = int.Parse(currentNoiseAzimuthVectorNode.SelectSingleNodeThrowIfNull("firstAzimuthLine")?.InnerText);
                    noiseAzimuthVector.FirstRangeSample = int.Parse(currentNoiseAzimuthVectorNode.SelectSingleNodeThrowIfNull("firstRangeSample")?.InnerText);

                    noiseAzimuthVector.LastAzimuthLine = int.Parse(currentNoiseAzimuthVectorNode.SelectSingleNodeThrowIfNull("lastAzimuthLine")?.InnerText);
                    noiseAzimuthVector.LastRangeSample = int.Parse(currentNoiseAzimuthVectorNode.SelectSingleNodeThrowIfNull("lastRangeSample")?.InnerText);

                    var allLines = currentNoiseAzimuthVectorNode
                        .SelectSingleNodeThrowIfNull("line")
                        .InnerText
                        .Split(' ')
                        .Where(f => string.IsNullOrWhiteSpace(f) == false)
                        .Select(int.Parse)
                        .ToList();

                    var allNoiseAzimuthRangeLut = currentNoiseAzimuthVectorNode
                        .SelectSingleNodeThrowIfNull("noiseAzimuthLut")
                        .InnerText
                        .Split(' ')
                        .Where(f => string.IsNullOrWhiteSpace(f) == false)
                        .Select(double.Parse)
                        .ToList();

                    if (allNoiseAzimuthRangeLut.Count != allLines.Count)
                        throw new XmlException("Missing line or noiseAzimuthLut");

                    for (var i = 0; i < allLines.Count; i++)
                    {
                        noiseAzimuthVector.Values.Add(new NoiseAzimuthVectorValue
                        {
                            Line = allLines[i],
                            NoiseAzimuthLut = allNoiseAzimuthRangeLut[i]
                        });
                    }

                    items.Add(noiseAzimuthVector);
                }

                return items;
            }
        }
    }
}