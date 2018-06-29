using System;
using System.Collections.Generic;

namespace SharpSentinel.Parser.Data.S1.Annotations
{
    public class QualityInformation
    {
        /// <summary>
        /// Overall product quality index. This annotation is calculated based on specific quality parameters and gives an overall quality value to the product.This parameter is reserved for future use and its value is set to 0.0. 
        /// </summary>
        public double ProductQualityIndex { get; set; }

        /// <summary>
        /// Quality data list. This element contains a list of qualityData records which contain the quality values and flags calculated and set during image processing.For individual scene and slice products there is one qualityData
        /// record in the list. For assembled products the list contains one qualityData record for each slice included in the assembled product.
        /// </summary>
        public List<QualityData> QualityDatas { get; set; }
    }

    public class QualityData
    {
        /// <summary>
        /// Zero Doppler azimuth time at which this set of quality annotations applies [UTC]. 
        /// </summary>
        public DateTimeOffset AzimuthTime { get; set; }

        /// <summary>
        /// Downlink quality. This record contains the quality indicators - values and flags - related to the downlink information.
        /// </summary>
        public DownlinkQuality DownlinkQuality { get; set; }

        /// <summary>
        /// Raw data analysis quality. This record contains the quality indicators - values and flags - related to the raw data analysis information.
        /// </summary>
        public RawDataAnalysisQuality RawDataAnalysisQuality { get; set; }

        /// <summary>
        /// Doppler centroid quality. This record contains the quality indicators - values and flags - related to the Doppler centroid estimation.
        /// </summary>
        public DopplerCentroidQuality DopplerCentroidQuality { get; set; }

        /// <summary>
        /// Image quality. This record contains the quality indicators - values and flags - related to properties of the output image.
        /// </summary>
        public ImageQuality ImageQuality { get; set; }
    }

    public class DownlinkQuality
    {
        /// <summary>
        /// Calculated mean of the input data for the I channel. 
        /// </summary>
        public double IInputDataMean { get; set; }

        /// <summary>
        /// Calculated mean of the input data for the Q channel.
        /// </summary>
        public double QInputDataMean { get; set; }

        /// <summary>
        /// Input data mean outside nominal range flag. False if the mean of I and Q input values are both within specified range from expected mean.For expected mean of x, 
        /// the measured mean must fall between x-threshold to x+threshold. True otherwise. 
        /// </summary>
        public bool InputDataMeanOutsideNorminalRange { get; set; }

        /// <summary>
        /// Calculated standard deviation of the input data for the I channel.
        /// </summary>
        public double IInputDataStdDev { get; set; }

        /// <summary>
        /// Calculated standard deviation of the input data for the Q channel.
        /// </summary>
        public double QInputDataStdDev { get; set; }

        /// <summary>
        /// Input data standard deviation outside nominal range flag. False if the standard deviation values of I and Q input values are both within 
        /// specified range of expected standard deviation. For expected std. dev.x, the measured std.dev.must fall between x-threshold to x+threshold.True otherwise.
        /// </summary>
        public bool InputDataStDevOutsideNominalRange { get; set; }

        #region Downlink

        /// <summary>
        /// Number of downlink gaps detected in the input data. 
        /// </summary>
        public int DownlinkInputDataGaps { get; set; }

        /// <summary>
        /// Significant downlink gaps in the input data flag. A downlink input data gap is defined as a
        /// contiguous block of N downlink missing lines(the value of N is predefined for each product). False
        /// if the number of downlink input gaps is less than or equal to the threshold value, true if number of
        /// downlink input data gaps is greater than the threshold value.
        /// </summary>
        public bool DownlinkGapsInInputDataSignificant { get; set; }

        /// <summary>
        /// Number of downlink missing lines detected in the input data, excluding data gaps. A downlink missing line is defined as any echo line
        /// physically absent from the input data file due to a downlink error.
        /// </summary>
        public int DownlinkInputMissingLines { get; set; }

        /// <summary>
        /// Downlink missing lines significant flag. False if the percentage of downlink missing lines is less than or equal to the threshold value, 
        /// true if the percentage of downlink missing lines is greater than  the threshold value.The number of downlink missing lines is numDownlinkInputMissingLines.
        /// </summary>
        public bool DownlinkMissingLinesSignificant { get; set; }

        #endregion

        #region Instrument

        /// <summary>
        /// Number of instrument gaps detected in the input data. 
        /// </summary>
        public int InstrumentInputDataGaps { get; set; }

        /// <summary>
        /// Significant instrument gaps in the input data flag. An instrument input data gap is defined as a contiguous block of N instrument missing lines(the value of N is predefined for each product).
        /// False if the number of instrument input gaps is less than or equal to the threshold value, true if number of instrument input data gaps is greater than the threshold value.
        /// </summary>
        public bool InstrumentGapsInInputDataSignificant { get; set; }

        /// <summary>
        /// Number of instrument missing lines detected in the input data, excluding data gaps. An instrument missing line is defined 
        /// as any echo line physically absent from the input data file due to a failure by  the instrument to produce the expected echo line.
        /// </summary>
        public int InstrumentInputMissingLines { get; set; }

        /// <summary>
        /// Instrument missing lines significant flag. False if the percentage of instrument missing lines is less than or equal to the threshold value, true if the percentage of instrument missing lines is greater than
        /// the threshold value.The number of instrument missing lines is numInstrumentInputMissingLines
        /// </summary>
        public bool InstrumentMissingLinesSignificant { get; set; }

        #endregion

        #region Ssb

        /// <summary>
        /// Number of gaps detected in the input data due to the SSB Error flag being set. 
        /// </summary>
        public int SsbErrorInputDataGaps { get; set; }

        /// <summary>
        /// Significant SSB Error gaps in the input data flag. An SSB Error input data gap is defined as a contiguous block of N lines in which the SSB Error Flag is set to true (the value of N is predefined
        /// for each product). False if the number of SSB Error input gaps is less than or equal to the threshold value, true if number of SSB Error input data gaps is greater than the threshold value.
        /// </summary>
        public bool SsbErrorGapsInInputDataSignificant { get; set; }

        /// <summary>
        /// Number of SSB Error missing lines detected in the input data, excluding data gaps. An SSB Error missing line is defined
        ///  as any echo line in which the SSB Error Flag is the ISP secondary header is  set to true.
        /// </summary>
        public int SsbErrorInputMissingLines { get; set; }

        /// <summary>
        /// SSB Error missing lines significant flag. False if the percentage of SSB Error missing lines is less than or equal to the threshold value,
        ///  true if the percentage of SSB Error missing lines is greater than  the threshold value.The number of SSB Error missing lines is numSsbErrorInputMissingLines.
        /// </summary>
        public bool SsbErrorMissingLinesSignificant { get; set; }

        #endregion

        /// <summary>
        /// Chirp source used during processing (Nominal or Extracted). This value is a copy of the value from the processingOptions record.
        /// </summary>
        public ChirpSourceType ChirpSourceUsed { get; set; }

        /// <summary>
        /// PG source used during processing (Model or Extracted). This value is a copy of the value from the referenceReplica record.
        /// </summary>
        public PgSourceType PgSourceUsed { get; set; }

        /// <summary>
        /// Type of range replica function used (Unextended, Extended Flat, Extended Tapered). This value is a copy of the value from the processingOptions record.
        /// </summary>
        public RrfSpectrumType RrfSpectrumUsed { get; set; }

        /// <summary>
        /// Chirp replica reconstruction failed or is of low quality flag. False if able to reconstruct at least one valid extracted replica during processing.True if unable to reconstruct any valid extracted replicas
        /// during processing.A replica is valid if it was successfully reconstructed and all quality measures were acceptable.If this flag is true then the processor uses the nominal range pulse for processing
        /// and a nominal elevation beam scaling factor
        /// </summary>
        public bool ReplicaReconstructionFailed { get; set; }

        /// <summary>
        /// Mean of all PG product amplitude values from the replicas extracted from the calibration pulses.
        /// </summary>
        public double MeanPgProductAmplitude { get; set; }

        /// <summary>
        /// Standard deviation of all PG product amplitude values from the replicas extracted from the calibration pulses.
        /// </summary>
        public double StdDevPgProductAmplitude { get; set; }

        /// <summary>
        /// Mean value of all PG product phase values from the replicas extracted from the calibration pulses [radians].
        /// </summary>
        public double MeanPgProductPhase { get; set; }

        /// <summary>
        /// Standard deviation of all PG product phase values from the replicas extracted from the calibration pulses[radians].
        /// </summary>
        public double StdDevPgProductPhase { get; set; }

        /// <summary>
        /// PG product derivation failed flag. False if the percentage of invalid relative and absolute PG products is below the configured threshold; or, true otherwise.If this flag is set to true then the
        /// values from the PG product model will be used in place of the derived PG product values.
        /// </summary>
        public bool PgProductDerivationFailed { get; set; }

        /// <summary>
        /// Invalid downlink parameters flag. False if all parameters read from the downlinked data were valid, true if any downlink parameter
        /// is out of range and therefore a default value has been used during processing.
        /// </summary>
        public bool InvalidDownlinkParams { get; set; }
    }

    public class RawDataAnalysisQuality
    {
        /// <summary>
        /// Calculated I bias. This value is a copy of the value from the rawDataAnalysis record
        /// </summary>
        public double IBias { get; set; }

        /// <summary>
        /// I bias significance, true if I bias falls within acceptable range, false otherwise. 
        /// </summary>
        public bool IBiasSignificance { get; set; }

        /// <summary>
        /// Calculated Q bias. This value is a copy of the value from the rawDataAnalysis record.
        /// </summary>
        public double QBias { get; set; }

        /// <summary>
        /// Q bias significance, true if Q bias falls within acceptable range, false otherwise. 
        /// </summary>
        public bool QBiasSignificance { get; set; }

        /// <summary>
        /// Calculated I/Q gain imbalance. This value is a copy of the value from the rawDataAnalysis record. 
        /// </summary>
        public double IQGainImbalance { get; set; }

        /// <summary>
        /// I/Q Gain Significance, true if I/Q gain imbalance falls within acceptable range, false otherwise. 
        /// </summary>
        public bool IQGainSignificance { get; set; }

        /// <summary>
        /// Calculated I/Q quadrature departure.
        /// </summary>
        public double IqQuadratureDeparture { get; set; }

        /// <summary>
        /// I/Q Quadrature Departure Significance, true if quadrature departure falls within acceptable range, false otherwise.
        /// </summary>
        public bool IqQuadratureDepartureSignificance { get; set; }
    }

    public class DopplerCentroidQuality
    {
        /// <summary>
        /// Doppler centroid estimation method used during processing. Both the Doppler centroid (DC) calculated from orbit geometry and the DC estimated from the raw data are annotated within the Doppler data set;
        /// however, this parameter describes the actual DC method used during image processing.This value is a copy of the value from the processingOptions record.
        /// </summary>
        public DcMethodType DcMethod { get; set; }

        /// <summary>
        /// Doppler centroid uncertain flag. False if the root mean squared (RMS) error for the DCE method used for image processing is less than the specified threshold, true if the RMS error is greater than or equal to the
        /// specified threshold.Note: if more than one Doppler centroid estimation is performed, the flag is set to true if any RMS error is greater than or equal to the threshold). 
        /// </summary>
        public bool DopplerCentroidUncertain { get; set; }
    }

    public class ImageQuality
    {
        /// <summary>
        /// Mean and standard deviation statistics for the image. This record is a copy of the record from the imageInformation record.
        /// </summary>
        public ImageQualityStatistics ImageStatistics { get; set; }

        /// <summary>
        /// Output data mean outside nominal range flag. False if the mean of I and Q output values for SLC image or mean of detected pixels for a detected product, are both within specified range from
        /// expected mean.For expected mean of x, the measured mean must fall between x-threshold to x+threshold.True otherwise. 
        /// </summary>
        public bool OutputDataMeanOutsideNominalRange { get; set; }

        /// <summary>
        /// Output data standard deviation outside nominal range flag. False if the std. dev. of I and Q output values for SLC image or std.dev.of detected pixels for a detected product, are both within
        /// specified range from expected std. dev.For expected std.dev.of x, the measured std.dev.must fall between x-threshold to x+threshold.True otherwise. 
        /// </summary>
        public bool OutputDataStDevOutsideNominalRange { get; set; }
    }

    public class ImageQualityStatistics
    {
        public double OutputDataMeanRe { get; set; }

        public double OutputDataMeanIm { get; set; }

        public double OutputDataStdDevRe { get; set; }

        public double OutputDataStdDevIm { get; set; }
    }

    /// <summary>
    /// Enumeration of the available chirp schemes. 
    /// </summary>
    public enum ChirpSourceType
    {
        Extracted,
        Nominal
    }

    /// <summary>
    /// Enumeration of the available PG schemes.
    /// </summary>
    public enum PgSourceType
    {
        Extracted,
        Model
    }

    /// <summary>
    /// The type of range matched filter to use during processing. 
    /// "Unextended": range reference function is unextended in frequency domain;
    /// "Extended Flat": range reference function is extended and flat in frequency domain; and,
    /// "Extended Tapered": range reference function is extended and tapered in frequency domain.
    /// </summary>
    public enum RrfSpectrumType
    {
        Unextended,
        ExtendedFlat,
        ExtendedTapered
    }

    /// <summary>
    /// Enumeration of Doppler centroid calculation/estimation methods.
    /// </summary>
    public enum DcMethodType
    {
        DataAnalysis,
        OrbitAndAttitude,
        Predefined
    }

    /// <summary>
    /// Enumeration of input data types for Doppler centroid estimation.
    /// </summary>
    public enum DxInputDataType
    {
        Raw,
        RangeCompressed
    }
}