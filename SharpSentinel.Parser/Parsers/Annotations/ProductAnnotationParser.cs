using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;
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

                productAnnotation.RawXml = document.InnerXml;

                var productNode = document.SelectSingleNodeThrowIfNull("product");

                productAnnotation.AdsHeader = AdsHeaderParser.Parse(productNode.SelectSingleNodeThrowIfNull("adsHeader"));
                productAnnotation.QualityInformation = QualityInformationParser.Parse(productNode.SelectSingleNodeThrowIfNull("qualityInformation"));
                productAnnotation.GeneralAnnotation = GeneralAnnotationParser.Parse(productNode.SelectSingleNodeThrowIfNull("generalAnnotation"));
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
                    data.DownlinkQuality.ChirpSourceUsed = (ChirpSourceType)Enum.Parse(typeof(ChirpSourceType), downlinkQualityNode.SelectSingleNodeThrowIfNull("chirpSourceUsed").InnerText);
                    data.DownlinkQuality.PgSourceUsed = (PgSourceType)Enum.Parse(typeof(PgSourceType), downlinkQualityNode.SelectSingleNodeThrowIfNull("pgSourceUsed").InnerText);
                    data.DownlinkQuality.RrfSpectrumUsed = (RrfSpectrumType)Enum.Parse(typeof(RrfSpectrumType), downlinkQualityNode.SelectSingleNodeThrowIfNull("rrfSpectrumUsed").InnerText.RemoveWhitespaces());
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
                    data.DopplerCentroidQuality.DcMethod = (DcMethodType)Enum.Parse(typeof(DcMethodType), dopplerCentroidQualityNode.SelectSingleNodeThrowIfNull("dcMethod").InnerText.RemoveWhitespaces());
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

        public static class GeneralAnnotationParser
        {
            public static GeneralAnnotation Parse(XmlNode generalAnnotationNode)
            {
                var generalAnnotaiton = new GeneralAnnotation();

                generalAnnotaiton.ProductInformation = ParseProductInormation(generalAnnotationNode.SelectSingleNodeThrowIfNull("productInformation"));
                generalAnnotaiton.DownlinkInformations = ParseDownlinkInformationList(generalAnnotationNode.SelectSingleNodeThrowIfNull("downlinkInformationList"));
                generalAnnotaiton.Orbits = ParseOrbits(generalAnnotationNode.SelectSingleNode("orbitList"));
                generalAnnotaiton.Attitudes = ParseAttitudes(generalAnnotationNode.SelectSingleNode("attitudeList"));

                return generalAnnotaiton;
            }

            private static List<Attitude> ParseAttitudes(XmlNode attitudeListNode)
            {
                var nodes = attitudeListNode.SelectNodes("attitude").Cast<XmlNode>();
                return nodes.Select(ParseAttitude).ToList();
            }

            private static Attitude ParseAttitude(XmlNode attitudeNode)
            {
                var attitude = new Attitude();

                attitude.Time = DateTimeOffset.Parse(attitudeNode.SelectSingleNodeThrowIfNull("time").InnerText);
                attitude.Frame = (ReferenceFrameType)Enum.Parse(typeof(ReferenceFrameType), attitudeNode.SelectSingleNodeThrowIfNull("frame").InnerText.RemoveWhitespaces());

                attitude.Q0 = double.Parse(attitudeNode.SelectSingleNodeThrowIfNull("q0").InnerText);
                attitude.Q1 = double.Parse(attitudeNode.SelectSingleNodeThrowIfNull("q1").InnerText);
                attitude.Q2 = double.Parse(attitudeNode.SelectSingleNodeThrowIfNull("q2").InnerText);
                attitude.Q3 = double.Parse(attitudeNode.SelectSingleNodeThrowIfNull("q3").InnerText);

                attitude.AngularVelocity = new Vector3D(
                    double.Parse(attitudeNode.SelectSingleNodeThrowIfNull("wx").InnerText),
                    double.Parse(attitudeNode.SelectSingleNodeThrowIfNull("wy").InnerText),
                    double.Parse(attitudeNode.SelectSingleNodeThrowIfNull("wz").InnerText));

                attitude.Roll = double.Parse(attitudeNode.SelectSingleNodeThrowIfNull("roll").InnerText);
                attitude.Pitch = double.Parse(attitudeNode.SelectSingleNodeThrowIfNull("pitch").InnerText);
                attitude.Yaw = double.Parse(attitudeNode.SelectSingleNodeThrowIfNull("yaw").InnerText);

                return attitude;
            }

            private static List<Orbit> ParseOrbits(XmlNode orbitsListNode)
            {
                var nodes = orbitsListNode.SelectNodes("orbit").Cast<XmlNode>();
                return nodes.Select(ParseOrbit).ToList();
            }

            private static Orbit ParseOrbit(XmlNode orbitNode)
            {
                var orbit = new Orbit();

                orbit.Time = DateTimeOffset.Parse(orbitNode.SelectSingleNodeThrowIfNull("time").InnerText);
                orbit.Frame = (ReferenceFrameType) Enum.Parse(typeof(ReferenceFrameType), orbitNode.SelectSingleNodeThrowIfNull("frame").InnerText.RemoveWhitespaces());
                
                var positionNode = orbitNode.SelectSingleNodeThrowIfNull("position");
                orbit.Position = new Vector3D(
                    double.Parse(positionNode.SelectSingleNodeThrowIfNull("x").InnerText),
                    double.Parse(positionNode.SelectSingleNodeThrowIfNull("y").InnerText),
                    double.Parse(positionNode.SelectSingleNodeThrowIfNull("z").InnerText));

                var velocityNode = orbitNode.SelectSingleNodeThrowIfNull("velocity");
                orbit.Velocity = new Vector3D(
                    double.Parse(velocityNode.SelectSingleNodeThrowIfNull("x").InnerText),
                    double.Parse(velocityNode.SelectSingleNodeThrowIfNull("y").InnerText),
                    double.Parse(velocityNode.SelectSingleNodeThrowIfNull("z").InnerText));

                return orbit;
            }

            private static List<DownlinkInformation> ParseDownlinkInformationList(XmlNode downlinkInformationListNode)
            {
                var nodes = downlinkInformationListNode.SelectNodes("downlinkInformation").Cast<XmlNode>();

                return nodes.Select(ParseDownlinkInformation).ToList();
            }

            private static DownlinkInformation ParseDownlinkInformation(XmlNode downlinkInformationNode)
            {
                var downlinkInformation = new DownlinkInformation();

                downlinkInformation.Swath = (SwathType)Enum.Parse(typeof(SwathType), downlinkInformationNode.SelectSingleNodeThrowIfNull("swath").InnerText);
                downlinkInformation.AzimuthTime = DateTimeOffset.Parse(downlinkInformationNode.SelectSingleNodeThrowIfNull("azimuthTime").InnerText);
                downlinkInformation.FirstLineSensingTime = DateTimeOffset.Parse(downlinkInformationNode.SelectSingleNodeThrowIfNull("firstLineSensingTime").InnerText);
                downlinkInformation.LastLineSensingTime = DateTimeOffset.Parse(downlinkInformationNode.SelectSingleNodeThrowIfNull("lastLineSensingTime").InnerText);
                downlinkInformation.PRF = double.Parse(downlinkInformationNode.SelectSingleNodeThrowIfNull("prf").InnerText);
                downlinkInformation.BitErrorCount = ParseBitErrorCount(downlinkInformationNode.SelectSingleNodeThrowIfNull("bitErrorCount"));
                downlinkInformation.DownlinkValues = ParseDownlinkValues(downlinkInformationNode.SelectSingleNodeThrowIfNull("downlinkValues"));

                return downlinkInformation;
            }

            private static DownlinkValues ParseDownlinkValues(XmlNode downlinkValuesNode)
            {
                var downlinkValues = new DownlinkValues();

                downlinkValues.Pri = double.Parse(downlinkValuesNode.SelectSingleNode("pri").InnerText);
                downlinkValues.Rank = int.Parse(downlinkValuesNode.SelectSingleNodeThrowIfNull("rank").InnerText);
                downlinkValues.DataTakeId = int.Parse(downlinkValuesNode.SelectSingleNodeThrowIfNull("dataTakeId").InnerText);
                downlinkValues.EccNumber = int.Parse(downlinkValuesNode.SelectSingleNodeThrowIfNull("eccNumber").InnerText);
                downlinkValues.RxChannelId = int.Parse(downlinkValuesNode.SelectSingleNodeThrowIfNull("rxChannelId").InnerText);
                downlinkValues.InstrumentConfigId = int.Parse(downlinkValuesNode.SelectSingleNodeThrowIfNull("instrumentConfigId").InnerText);

                var dataFormatNode = downlinkValuesNode.SelectSingleNodeThrowIfNull("dataFormat");
                downlinkValues.DataFormat = new DataFormat();

                downlinkValues.DataFormat.BaqBlockLength = int.Parse(dataFormatNode.SelectSingleNodeThrowIfNull("baqBlockLength").InnerText);
                downlinkValues.DataFormat.EchoFormat = (DataFormatMode) Enum.Parse(typeof(DataFormatMode), dataFormatNode.SelectSingleNodeThrowIfNull("echoFormat").InnerText.RemoveWhitespaces());
                downlinkValues.DataFormat.NoiseFormat = (DataFormatMode)Enum.Parse(typeof(DataFormatMode), dataFormatNode.SelectSingleNodeThrowIfNull("noiseFormat").InnerText.RemoveWhitespaces());
                downlinkValues.DataFormat.CalibrationFormat = (DataFormatMode)Enum.Parse(typeof(DataFormatMode), dataFormatNode.SelectSingleNodeThrowIfNull("calibrationFormat").InnerText.RemoveWhitespaces());
                downlinkValues.DataFormat.MeanBitRate = double.Parse(dataFormatNode.SelectSingleNodeThrowIfNull("meanBitRate").InnerText);

                var rangeDecimationNode = downlinkValuesNode.SelectSingleNodeThrowIfNull("rangeDecimation");
                downlinkValues.RangeDecimation = new RangeDecimation();

                downlinkValues.RangeDecimation.DecimationFilterBandwidth = double.Parse(rangeDecimationNode.SelectSingleNodeThrowIfNull("decimationFilterBandwidth").InnerText);
                downlinkValues.RangeDecimation.SamplingFrequencyAfterDecimation = double.Parse(rangeDecimationNode.SelectSingleNodeThrowIfNull("samplingFrequencyAfterDecimation").InnerText);
                downlinkValues.RangeDecimation.FilterLength = int.Parse(rangeDecimationNode.SelectSingleNodeThrowIfNull("filterLength").InnerText);

                downlinkValues.RxGain = double.Parse(downlinkValuesNode.SelectSingleNodeThrowIfNull("rxGain").InnerText);
                downlinkValues.TxPulseLength = double.Parse(downlinkValuesNode.SelectSingleNodeThrowIfNull("txPulseLength").InnerText);
                downlinkValues.TxPulseStartFrequency = double.Parse(downlinkValuesNode.SelectSingleNodeThrowIfNull("txPulseStartFrequency").InnerText);
                downlinkValues.TxPulseRampRate = double.Parse(downlinkValuesNode.SelectSingleNodeThrowIfNull("txPulseRampRate").InnerText);
                downlinkValues.SwathNumber = int.Parse(downlinkValuesNode.SelectSingleNodeThrowIfNull("swathNumber").InnerText);

                var swlNodes = downlinkValuesNode.SelectNodes("swlList/swlList");
                downlinkValues.SamplingWindowLengths = new List<SamplingWindowLenght>();

                foreach (var currentSwlNode in swlNodes.Cast<XmlNode>())
                {
                    var samplingWindowLength = new SamplingWindowLenght();

                    samplingWindowLength.Value = double.Parse(currentSwlNode.SelectSingleNodeThrowIfNull("value").InnerText);
                    samplingWindowLength.AzimuthTime = DateTimeOffset.Parse(currentSwlNode.SelectSingleNodeThrowIfNull("azimuthTime").InnerText);

                    downlinkValues.SamplingWindowLengths.Add(samplingWindowLength);
                }

                var swstNodes = downlinkValuesNode.SelectNodes("swstList/swst");
                downlinkValues.SamplingWindowStartTimes = new List<SamplingWindowStartTime>();

                foreach (var currentSwstNode in swstNodes.Cast<XmlNode>())
                {
                    var samplingWindowStartTime = new SamplingWindowStartTime();

                    samplingWindowStartTime.Value = double.Parse(currentSwstNode.SelectSingleNodeThrowIfNull("value").InnerText);
                    samplingWindowStartTime.AzimuthTime = DateTimeOffset.Parse(currentSwstNode.SelectSingleNodeThrowIfNull("azimuthTime").InnerText);

                    downlinkValues.SamplingWindowStartTimes.Add(samplingWindowStartTime);
                }

                var pointingStatusNodes = downlinkValuesNode.SelectNodes("pointingStatusList/pointingStatus");
                downlinkValues.PointingStatuses = new List<PointingStatus>();

                foreach (var currentPointingStatusNode in pointingStatusNodes.Cast<XmlNode>())
                {
                    var pointingStatus = new PointingStatus();

                    pointingStatus.AzimuthTime = DateTimeOffset.Parse(currentPointingStatusNode.SelectSingleNodeThrowIfNull("azimuthTime").InnerText);
                    pointingStatus.AocsOpMode = (AocsOpModeType) Enum.Parse(typeof(AocsOpModeType), currentPointingStatusNode.SelectSingleNodeThrowIfNull("aocsOpMode").InnerText.RemoveWhitespaces());
                    pointingStatus.RollError = bool.Parse(currentPointingStatusNode.SelectSingleNodeThrowIfNull("rollErrorFlag").InnerText);
                    pointingStatus.PitchError = bool.Parse(currentPointingStatusNode.SelectSingleNodeThrowIfNull("pitchErrorFlag").InnerText);
                    pointingStatus.YawError = bool.Parse(currentPointingStatusNode.SelectSingleNodeThrowIfNull("yawErrorFlag").InnerText);

                    downlinkValues.PointingStatuses.Add(pointingStatus);
                }

                return downlinkValues;
            }

            private static BitErrorCount ParseBitErrorCount(XmlNode bitErrorCountNode)
            {
                var bitError = new BitErrorCount();

                bitError.ErrorSyncMarker = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrSyncMarker").InnerText);
                bitError.ErrorDateTakeId = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrDataTakeId").InnerText);
                bitError.ErrorEccNumber = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrEccNumber").InnerText);
                bitError.ErrorTestMode = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrTestMode").InnerText);
                bitError.ErrorRxChannelId = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrRxChannelId").InnerText);
                bitError.ErrorInstrumentConfigId = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrInstrumentConfigId").InnerText);
                bitError.ErrorPacketCount = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrPacketCount").InnerText);
                bitError.ErrorPriCount = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrPriCount").InnerText);
                bitError.ErrorSsbErrorFlag = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrSsbErrorFlag").InnerText);
                bitError.ErrorBaqMode = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrBaqMode").InnerText);
                bitError.ErrorBaqBlockLength = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrBaqBlockLength").InnerText);
                bitError.ErrorRangeDecimation = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrRangeDecimation").InnerText);
                bitError.ErrorRxGain = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrRxGain").InnerText);
                bitError.ErrorTxRampRate = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrTxRampRate").InnerText);
                bitError.ErrorTxPulseStartFrequency = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrTxPulseStartFrequency").InnerText);
                bitError.ErrorRank = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrRank").InnerText);
                bitError.ErrorPri = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrPri").InnerText);
                bitError.ErrorSwst = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrSwst").InnerText);
                bitError.ErrorSwl = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrSwl").InnerText);
                bitError.ErrorPolarisation = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrPolarisation").InnerText);
                bitError.ErrorTempComp = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrTempComp").InnerText);
                bitError.ErrorElevationBeamAddress = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrElevationBeamAddress").InnerText);
                bitError.ErrorAzimuthBeamAddress = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrAzimuthBeamAddress").InnerText);
                bitError.ErrorSasTestMode = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrSasTestMode").InnerText);
                bitError.ErrorCalType = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrCalType").InnerText);
                bitError.ErrorCalibrationBeamAddress = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrCalibrationBeamAddress").InnerText);
                bitError.ErrorCalMode = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrCalMode").InnerText);
                bitError.ErrorTxPulseNumber = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrTxPulseNumber").InnerText);
                bitError.ErrorSignalType = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrSignalType").InnerText);
                bitError.ErrorSwapFlag = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrSwapFlag").InnerText);
                bitError.ErrorSwathNumber = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrSwathNumber").InnerText);
                bitError.ErrorNumberOfQuads = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numErrNumberOfQuads").InnerText);
                bitError.IspHeaderErrors = int.Parse(bitErrorCountNode.SelectSingleNodeThrowIfNull("numIspHeaderErrors").InnerText);

                return bitError;
            }

            private static ProductInformation ParseProductInormation(XmlNode productInformationNode)
            {
                var productInformation = new ProductInformation();

                productInformation.Pass = (PassType)Enum.Parse(typeof(PassType), productInformationNode.SelectSingleNodeThrowIfNull("pass").InnerText.ToUpper());
                productInformation.TimelinessCategory = (ProductTimelinessCategoryType)Enum.Parse(typeof(ProductTimelinessCategoryType), productInformationNode.SelectSingleNodeThrowIfNull("timelinessCategory").InnerText.Replace("-", ""));
                productInformation.PlatformHeading = double.Parse(productInformationNode.SelectSingleNodeThrowIfNull("platformHeading").InnerText);
                productInformation.Projection = (ProjectionType)Enum.Parse(typeof(ProjectionType), productInformationNode.SelectSingleNodeThrowIfNull("projection").InnerText.RemoveWhitespaces());
                productInformation.RangeSamplingRate = double.Parse(productInformationNode.SelectSingleNodeThrowIfNull("rangeSamplingRate").InnerText);
                productInformation.RadarFrequency = double.Parse(productInformationNode.SelectSingleNodeThrowIfNull("radarFrequency").InnerText);
                productInformation.AzimuthSteeringRate = double.Parse(productInformationNode.SelectSingleNodeThrowIfNull("azimuthSteeringRate").InnerText);

                return productInformation;
            }
        }
    }
}