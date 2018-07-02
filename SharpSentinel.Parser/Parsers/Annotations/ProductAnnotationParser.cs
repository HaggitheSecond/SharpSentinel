using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using JetBrains.Annotations;
using SharpSentinel.Parser.Data.Common;
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

                productAnnotation.RawXML = document.InnerXml;

                var productNode = document.SelectSingleNodeThrowIfNull("product");

                productAnnotation.AdsHeader = AdsHeaderParser.Parse(productNode.SelectSingleNodeThrowIfNull("adsHeader"));
                productAnnotation.QualityInformation = QualityInformationParser.Parse(productNode.SelectSingleNodeThrowIfNull("qualityInformation"));
            }

            return productAnnotation;
        }

        public static class QualityInformationParser
        {
            public static QualityInformation Parse(XmlNode qualityInformationNode)
            {
                var qualityInformation = new QualityInformation
                {
                    ProductQualityIndex = double.Parse(qualityInformationNode.SelectSingleNodeThrowIfNull("productQualityIndex").InnerText),
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
                    var downlinkQualityNode = qualityDataNode.SelectSingleNodeThrowIfNull("downlinkQuality");
                    var rawDataAnalysisQualityNode = qualityDataNode.SelectSingleNodeThrowIfNull("rawDataAnalysisQuality");
                    var dopplerCentroidQualityNode = qualityDataNode.SelectSingleNodeThrowIfNull("dopplerCentroidQuality");
                    var imageQualityNode = qualityDataNode.SelectSingleNodeThrowIfNull("imageQuality");

                    var data = new QualityData();
                    data.AzimuthTime = DateTimeOffset.Parse(qualityDataNode.SelectSingleNodeThrowIfNull("azimuthTime").InnerText);

                    data.DownlinkQuality = new DownlinkQuality();
                    data.DownlinkQuality.IInputDataMean = double.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("iInputDataMean").InnerText);
                    data.DownlinkQuality.QInputDataMean = double.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("qInputDataMean").InnerText);
                    data.DownlinkQuality.InputDataMeanOutsideNorminalRange = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("inputDataMeanOutsideNominalRangeFlag").InnerText);
                    data.DownlinkQuality.IInputDataStdDev = double.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("iInputDataStdDev").InnerText);
                    data.DownlinkQuality.QInputDataStdDev = double.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("qInputDataStdDev").InnerText);
                    data.DownlinkQuality.InputDataStDevOutsideNominalRange = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("inputDataStDevOutsideNominalRangeFlag").InnerText);
                    data.DownlinkQuality.DownlinkInputDataGaps = int.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("numDownlinkInputDataGaps").InnerText);
                    data.DownlinkQuality.DownlinkGapsInInputDataSignificant = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("downlinkGapsInInputDataSignificantFlag").InnerText);
                    data.DownlinkQuality.DownlinkInputMissingLines = int.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("numDownlinkInputMissingLines").InnerText);
                    data.DownlinkQuality.DownlinkMissingLinesSignificant = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("downlinkMissingLinesSignificantFlag").InnerText);
                    data.DownlinkQuality.InstrumentInputDataGaps = int.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("numInstrumentInputDataGaps").InnerText);
                    data.DownlinkQuality.InstrumentGapsInInputDataSignificant = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("instrumentGapsInInputDataSignificantFlag").InnerText);
                    data.DownlinkQuality.InstrumentInputMissingLines = int.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("numInstrumentInputMissingLines").InnerText);
                    data.DownlinkQuality.InstrumentMissingLinesSignificant = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("instrumentMissingLinesSignificantFlag").InnerText);
                    data.DownlinkQuality.SsbErrorInputDataGaps = int.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("numSsbErrorInputDataGaps").InnerText);
                    data.DownlinkQuality.SsbErrorGapsInInputDataSignificant = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("ssbErrorGapsInInputDataSignificantFlag").InnerText);
                    data.DownlinkQuality.SsbErrorInputMissingLines = int.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("numSsbErrorInputMissingLines").InnerText);
                    data.DownlinkQuality.SsbErrorMissingLinesSignificant = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("ssbErrorMissingLinesSignificantFlag").InnerText);
                    data.DownlinkQuality.ChirpSourceUsed = (ChirpSourceType) Enum.Parse(typeof(ChirpSourceType), downlinkQualityNode.SelectSingleNodeThrowIfNull("chirpSourceUsed").InnerText);
                    data.DownlinkQuality.PgSourceUsed = (PgSourceType) Enum.Parse(typeof(PgSourceType), downlinkQualityNode.SelectSingleNodeThrowIfNull("pgSourceUsed").InnerText);
                    data.DownlinkQuality.RrfSpectrumUsed = (RrfSpectrumType) Enum.Parse(typeof(RrfSpectrumType), downlinkQualityNode.SelectSingleNodeThrowIfNull("rrfSpectrumUsed").InnerText.RemoveWhitespaces());
                    data.DownlinkQuality.ReplicaReconstructionFailed = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("replicaReconstructionFailedFlag").InnerText);
                    data.DownlinkQuality.MeanPgProductAmplitude = double.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("meanPgProductAmplitude").InnerText);
                    data.DownlinkQuality.StdDevPgProductAmplitude = double.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("stdDevPgProductAmplitude").InnerText);
                    data.DownlinkQuality.MeanPgProductPhase = double.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("meanPgProductPhase").InnerText);
                    data.DownlinkQuality.StdDevPgProductPhase = double.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("stdDevPgProductPhase").InnerText);
                    data.DownlinkQuality.PgProductDerivationFailed = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("pgProductDerivationFailedFlag").InnerText);
                    data.DownlinkQuality.InvalidDownlinkParams = bool.Parse(downlinkQualityNode.SelectSingleNodeThrowIfNull("invalidDownlinkParamsFlag").InnerText);

                    data.RawDataAnalysisQuality = new RawDataAnalysisQuality();
                    data.RawDataAnalysisQuality.IBias = double.Parse(rawDataAnalysisQualityNode.SelectSingleNodeThrowIfNull("iBias").InnerText);
                    data.RawDataAnalysisQuality.IBiasSignificance = bool.Parse(rawDataAnalysisQualityNode.SelectSingleNodeThrowIfNull("iBiasSignificanceFlag").InnerText);
                    data.RawDataAnalysisQuality.QBias = double.Parse(rawDataAnalysisQualityNode.SelectSingleNodeThrowIfNull("qBias").InnerText);
                    data.RawDataAnalysisQuality.QBiasSignificance = bool.Parse(rawDataAnalysisQualityNode.SelectSingleNodeThrowIfNull("qBiasSignificanceFlag").InnerText);
                    data.RawDataAnalysisQuality.IQGainImbalance = double.Parse(rawDataAnalysisQualityNode.SelectSingleNodeThrowIfNull("iqGainImbalance").InnerText);
                    data.RawDataAnalysisQuality.IQGainSignificance = bool.Parse(rawDataAnalysisQualityNode.SelectSingleNodeThrowIfNull("iqGainSignificanceFlag").InnerText);
                    data.RawDataAnalysisQuality.IqQuadratureDeparture = double.Parse(rawDataAnalysisQualityNode.SelectSingleNodeThrowIfNull("iqQuadratureDeparture").InnerText);
                    data.RawDataAnalysisQuality.IqQuadratureDepartureSignificance = bool.Parse(rawDataAnalysisQualityNode.SelectSingleNodeThrowIfNull("iqQuadratureDepartureSignificanceFlag").InnerText);

                    data.DopplerCentroidQuality = new DopplerCentroidQuality();
                    data.DopplerCentroidQuality.DcMethod = (DcMethodType) Enum.Parse(typeof(DcMethodType), dopplerCentroidQualityNode.SelectSingleNodeThrowIfNull("dcMethod").InnerText.RemoveWhitespaces());
                    data.DopplerCentroidQuality.DopplerCentroidUncertain = bool.Parse(dopplerCentroidQualityNode.SelectSingleNodeThrowIfNull("dopplerCentroidUncertainFlag").InnerText);

                    data.ImageQuality = new ImageQuality();
                    data.ImageQuality.ImageStatistics = new ImageQualityStatistics();
                    data.ImageQuality.ImageStatistics.OutputDataMeanRe = double.Parse(imageQualityNode.SelectSingleNodeThrowIfNull("imageStatistics/outputDataMean/re").InnerText);
                    data.ImageQuality.ImageStatistics.OutputDataMeanIm = double.Parse(imageQualityNode.SelectSingleNodeThrowIfNull("imageStatistics/outputDataMean/im").InnerText);
                    data.ImageQuality.ImageStatistics.OutputDataStdDevRe = double.Parse(imageQualityNode.SelectSingleNodeThrowIfNull("imageStatistics/outputDataStdDev/re").InnerText);
                    data.ImageQuality.ImageStatistics.OutputDataStdDevIm = double.Parse(imageQualityNode.SelectSingleNodeThrowIfNull("imageStatistics/outputDataStdDev/im").InnerText);
                    data.ImageQuality.OutputDataMeanOutsideNominalRange = bool.Parse(imageQualityNode.SelectSingleNodeThrowIfNull("outputDataMeanOutsideNominalRangeFlag").InnerText);
                    data.ImageQuality.OutputDataStDevOutsideNominalRange = bool.Parse(imageQualityNode.SelectSingleNodeThrowIfNull("outputDataStDevOutsideNominalRangeFlag").InnerText);

                    return data;
                }
            }
        }
    }
}