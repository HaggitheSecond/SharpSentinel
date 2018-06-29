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
    public static class ProductAnnotationParser
    {
        public static ProductAnnotation Parse([NotNull]FileInfo fileInfo, [CanBeNull]Checksum checkSum)
        {
            Guard.NotNullAndValidFileSystemInfo(fileInfo, nameof(fileInfo));

            var productAnnotation = new ProductAnnotation
            {
                File = fileInfo,
                Checksum = checkSum
            };

            using (var fileStream = new FileStream(fileInfo.FullName, FileMode.Open))
            {
                var document = new XmlDocument();
                document.Load(fileStream);

                var productNode = document.SelectSingleNode("product");

                productAnnotation.AdsHeader = AdsHeaderParser.Parse(productNode.SelectSingleNode("adsHeader"));
                productAnnotation.QualityInformation = QualityInformationParser.Parse(productNode.SelectSingleNode("qualityInformation"));
            }

            return productAnnotation;
        }

        public static class AdsHeaderParser
        {
            public static AdsHeader Parse(XmlNode adsHeaderNode)
            {
                Guard.NotNull(adsHeaderNode, nameof(adsHeaderNode));

                var header = new AdsHeader();
                header.MissionId = adsHeaderNode.SelectSingleNode("missionId")?.InnerText;
                header.ProductType = (ProductTypeType) Enum.Parse(typeof(ProductTypeType), adsHeaderNode.SelectSingleNode("productType").InnerText);
                header.Polarisation = (TransmitterReceiverPolarisationType) Enum.Parse(typeof(TransmitterReceiverPolarisationType),adsHeaderNode.SelectSingleNode("polarisation").InnerText);
                header.Mode = (InstrumentModeType) Enum.Parse(typeof(InstrumentModeType),adsHeaderNode.SelectSingleNode("mode").InnerText);
                header.Swath = (SwathType) Enum.Parse(typeof(SwathType), adsHeaderNode.SelectSingleNode("swath").InnerText);
                header.StartTime = DateTimeOffset.Parse(adsHeaderNode.SelectSingleNode("startTime").InnerText);
                header.StopTime = DateTimeOffset.Parse(adsHeaderNode.SelectSingleNode("stopTime").InnerText);
                header.AbsoluteOrbitNumber = int.Parse(adsHeaderNode.SelectSingleNode("absoluteOrbitNumber").InnerText);
                header.MissionDataTakeId = int.Parse(adsHeaderNode.SelectSingleNode("missionDataTakeId").InnerText);
                header.ImageNumber = int.Parse(adsHeaderNode.SelectSingleNode("imageNumber").InnerText);
                return header;
            }
        }

        public static class QualityInformationParser
        {
            public static QualityInformation Parse(XmlNode qualityInformationNode)
            {
                var qualityInformation = new QualityInformation
                {
                    ProductQualityIndex = double.Parse(qualityInformationNode.SelectSingleNode("productQualityIndex").InnerText),
                    QualityDatas = new List<QualityData>()
                };

                var qualityDataLists = qualityInformationNode.SelectNodes("qualityDataList/qualityData");

                foreach (var currentDataList in qualityDataLists.Cast<XmlNode>())
                {
                    qualityInformation.QualityDatas.Add(QualityDataParser.Parse(currentDataList));
                }

                return qualityInformation;
            }

            public static class QualityDataParser
            {
                public static QualityData Parse(XmlNode qualityDataNode)
                {
                    var downlinkQualityNode = qualityDataNode.SelectSingleNode("downlinkQuality");
                    var rawDataAnalysisQualityNode = qualityDataNode.SelectSingleNode("rawDataAnalysisQuality");
                    var dopplerCentroidQualityNode = qualityDataNode.SelectSingleNode("dopplerCentroidQuality");
                    var imageQualityNode = qualityDataNode.SelectSingleNode("imageQuality");

                    var data = new QualityData();
                    data.AzimuthTime = DateTimeOffset.Parse(qualityDataNode.SelectSingleNode("azimuthTime").InnerText);

                    data.DownlinkQuality = new DownlinkQuality();
                    data.DownlinkQuality.IInputDataMean = double.Parse(downlinkQualityNode.SelectSingleNode("iInputDataMean").InnerText);
                    data.DownlinkQuality.QInputDataMean = double.Parse(downlinkQualityNode.SelectSingleNode("qInputDataMean").InnerText);
                    data.DownlinkQuality.InputDataMeanOutsideNorminalRange = bool.Parse(downlinkQualityNode.SelectSingleNode("inputDataMeanOutsideNominalRangeFlag").InnerText);
                    data.DownlinkQuality.IInputDataStdDev = double.Parse(downlinkQualityNode.SelectSingleNode("iInputDataStdDev").InnerText);
                    data.DownlinkQuality.QInputDataStdDev = double.Parse(downlinkQualityNode.SelectSingleNode("qInputDataStdDev").InnerText);
                    data.DownlinkQuality.InputDataStDevOutsideNominalRange = bool.Parse(downlinkQualityNode.SelectSingleNode("inputDataStDevOutsideNominalRangeFlag").InnerText);
                    data.DownlinkQuality.DownlinkInputDataGaps = int.Parse(downlinkQualityNode.SelectSingleNode("numDownlinkInputDataGaps").InnerText);
                    data.DownlinkQuality.DownlinkGapsInInputDataSignificant = bool.Parse(downlinkQualityNode.SelectSingleNode("downlinkGapsInInputDataSignificantFlag").InnerText);
                    data.DownlinkQuality.DownlinkInputMissingLines = int.Parse(downlinkQualityNode.SelectSingleNode("numDownlinkInputMissingLines").InnerText);
                    data.DownlinkQuality.DownlinkMissingLinesSignificant = bool.Parse(downlinkQualityNode.SelectSingleNode("downlinkMissingLinesSignificantFlag").InnerText);
                    data.DownlinkQuality.InstrumentInputDataGaps = int.Parse(downlinkQualityNode.SelectSingleNode("numInstrumentInputDataGaps").InnerText);
                    data.DownlinkQuality.InstrumentGapsInInputDataSignificant = bool.Parse(downlinkQualityNode.SelectSingleNode("instrumentGapsInInputDataSignificantFlag").InnerText);
                    data.DownlinkQuality.InstrumentInputMissingLines = int.Parse(downlinkQualityNode.SelectSingleNode("numInstrumentInputMissingLines").InnerText);
                    data.DownlinkQuality.InstrumentMissingLinesSignificant = bool.Parse(downlinkQualityNode.SelectSingleNode("instrumentMissingLinesSignificantFlag").InnerText);
                    data.DownlinkQuality.SsbErrorInputDataGaps = int.Parse(downlinkQualityNode.SelectSingleNode("numSsbErrorInputDataGaps").InnerText);
                    data.DownlinkQuality.SsbErrorGapsInInputDataSignificant = bool.Parse(downlinkQualityNode.SelectSingleNode("ssbErrorGapsInInputDataSignificantFlag").InnerText);
                    data.DownlinkQuality.SsbErrorInputMissingLines = int.Parse(downlinkQualityNode.SelectSingleNode("numSsbErrorInputMissingLines").InnerText);
                    data.DownlinkQuality.SsbErrorMissingLinesSignificant = bool.Parse(downlinkQualityNode.SelectSingleNode("ssbErrorMissingLinesSignificantFlag").InnerText);
                    data.DownlinkQuality.ChirpSourceUsed = (ChirpSourceType) Enum.Parse(typeof(ChirpSourceType), downlinkQualityNode.SelectSingleNode("chirpSourceUsed").InnerText);
                    data.DownlinkQuality.PgSourceUsed = (PgSourceType) Enum.Parse(typeof(PgSourceType), downlinkQualityNode.SelectSingleNode("pgSourceUsed").InnerText);
                    data.DownlinkQuality.RrfSpectrumUsed = (RrfSpectrumType) Enum.Parse(typeof(RrfSpectrumType), downlinkQualityNode.SelectSingleNode("rrfSpectrumUsed").InnerText.RemoveWhitespaces());
                    data.DownlinkQuality.ReplicaReconstructionFailed = bool.Parse(downlinkQualityNode.SelectSingleNode("replicaReconstructionFailedFlag").InnerText);
                    data.DownlinkQuality.MeanPgProductAmplitude = double.Parse(downlinkQualityNode.SelectSingleNode("meanPgProductAmplitude").InnerText);
                    data.DownlinkQuality.StdDevPgProductAmplitude = double.Parse(downlinkQualityNode.SelectSingleNode("stdDevPgProductAmplitude").InnerText);
                    data.DownlinkQuality.MeanPgProductPhase = double.Parse(downlinkQualityNode.SelectSingleNode("meanPgProductPhase").InnerText);
                    data.DownlinkQuality.StdDevPgProductPhase = double.Parse(downlinkQualityNode.SelectSingleNode("stdDevPgProductPhase").InnerText);
                    data.DownlinkQuality.PgProductDerivationFailed = bool.Parse(downlinkQualityNode.SelectSingleNode("pgProductDerivationFailedFlag").InnerText);
                    data.DownlinkQuality.InvalidDownlinkParams = bool.Parse(downlinkQualityNode.SelectSingleNode("invalidDownlinkParamsFlag").InnerText);

                    data.RawDataAnalysisQuality = new RawDataAnalysisQuality();
                    data.RawDataAnalysisQuality.IBias = double.Parse(rawDataAnalysisQualityNode.SelectSingleNode("iBias").InnerText);
                    data.RawDataAnalysisQuality.IBiasSignificance = bool.Parse(rawDataAnalysisQualityNode.SelectSingleNode("iBiasSignificanceFlag").InnerText);
                    data.RawDataAnalysisQuality.QBias = double.Parse(rawDataAnalysisQualityNode.SelectSingleNode("qBias").InnerText);
                    data.RawDataAnalysisQuality.QBiasSignificance = bool.Parse(rawDataAnalysisQualityNode.SelectSingleNode("qBiasSignificanceFlag").InnerText);
                    data.RawDataAnalysisQuality.IQGainImbalance = double.Parse(rawDataAnalysisQualityNode.SelectSingleNode("iqGainImbalance").InnerText);
                    data.RawDataAnalysisQuality.IQGainSignificance = bool.Parse(rawDataAnalysisQualityNode.SelectSingleNode("iqGainSignificanceFlag").InnerText);
                    data.RawDataAnalysisQuality.IqQuadratureDeparture = double.Parse(rawDataAnalysisQualityNode.SelectSingleNode("iqQuadratureDeparture").InnerText);
                    data.RawDataAnalysisQuality.IqQuadratureDepartureSignificance = bool.Parse(rawDataAnalysisQualityNode.SelectSingleNode("iqQuadratureDepartureSignificanceFlag").InnerText);

                    data.DopplerCentroidQuality = new DopplerCentroidQuality();
                    data.DopplerCentroidQuality.DcMethod = (DcMethodType) Enum.Parse(typeof(DcMethodType), dopplerCentroidQualityNode.SelectSingleNode("dcMethod").InnerText.RemoveWhitespaces());
                    data.DopplerCentroidQuality.DopplerCentroidUncertain = bool.Parse(dopplerCentroidQualityNode.SelectSingleNode("dopplerCentroidUncertainFlag").InnerText);

                    data.ImageQuality = new ImageQuality();
                    data.ImageQuality.ImageStatistics = new ImageQualityStatistics();
                    data.ImageQuality.ImageStatistics.OutputDataMeanRe = double.Parse(imageQualityNode.SelectSingleNode("imageStatistics/outputDataMean/re").InnerText);
                    data.ImageQuality.ImageStatistics.OutputDataMeanIm = double.Parse(imageQualityNode.SelectSingleNode("imageStatistics/outputDataMean/im").InnerText);
                    data.ImageQuality.ImageStatistics.OutputDataStdDevRe = double.Parse(imageQualityNode.SelectSingleNode("imageStatistics/outputDataStdDev/re").InnerText);
                    data.ImageQuality.ImageStatistics.OutputDataStdDevIm = double.Parse(imageQualityNode.SelectSingleNode("imageStatistics/outputDataStdDev/im").InnerText);
                    data.ImageQuality.OutputDataMeanOutsideNominalRange = bool.Parse(imageQualityNode.SelectSingleNode("outputDataMeanOutsideNominalRangeFlag").InnerText);
                    data.ImageQuality.OutputDataStDevOutsideNominalRange = bool.Parse(imageQualityNode.SelectSingleNode("outputDataStDevOutsideNominalRangeFlag").InnerText);

                    return data;
                }
            }
        }
    }
}