using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;
using SharpSentinel.Parser.Data.ManifestObjects;

namespace SharpSentinel.Parser.Data.S1.Annotations
{
    public class GeneralAnnotation
    {
        /// <summary>
        /// General product information. This record describes some key characteristics of the product, the input data and the acquisition platform.
        /// </summary>
        public ProductInformation ProductInformation { get; set; }

        /// <summary>
        /// Downlink information. This record contains information about the data extracted/calculated from the input data, including values extracted from the ISP and data error counters.For individual scene and slice products
        /// there is one downlinkInformation record, except in the case of IW/EW GRD products, where there will be one record per swath. For assembled products the list contains all the downlinkInformation records for each
        /// slice included in the assembled product.For a minimum output slice length of 10s, a maximum segment length of 25 minutes and a maximum 5 swaths, the maximum number of records in the list is 750.
        /// </summary>
        public IList<DownlinkInformation> DownlinkInformations { get; set; }

        /// <summary>
        /// Orbit state vector record. This record contains a position vector and a velocity vector which together describe the orbit state of the platform at the annotated time.
        /// With a minimum orbit/attitude update rate of 1s and a maximum product length of 25 minutes, the maximum size of this list is 1500 elements.
        /// </summary>
        public IList<Orbit> Orbits { get; set; }

        /// <summary>
        /// Attitude data record. This record contains the attitude quaternions and an angular velocity vector which together describe the attitude state of the platform at the annotated time.
        /// With a minimum orbit/attitude update rate of 1s and a maximum product length of 25 minutes, the maximum size of this list is 1500 elements.
        /// </summary>
        public IList<Attitude> Attitudes { get; set; }
    }

    public class ProductInformation
    {
        /// <summary>
        /// Direction of the orbit (ascending, descending) for the oldest image data in the product (the start of the product).
        /// </summary>
        public PassType Pass { get; set; }

        /// <summary>
        /// Timeliness category under which the product was produced, i.e. time frame from the data acquisition (for the near real time categories) or from the satellite tasking to the product delivery to the end user.
        /// </summary>
        public ProductTimelinessCategoryType TimelinessCategory { get; set; }

        /// <summary>
        /// Platform heading relative to North [degrees]. 
        /// </summary>
        public double PlatformHeading { get; set; }

        /// <summary>
        /// Projection of the image, either slant range or ground range.
        /// </summary>
        public ProjectionType Projection { get; set; }

        /// <summary>
        /// Range sample rate [Hz]. 
        /// </summary>
        public double RangeSamplingRate { get; set; }

        /// <summary>
        /// Radar frequency [Hz].
        /// </summary>
        public double RadarFrequency { get; set; }

        /// <summary>
        /// Azimuth steering rate for IW and EW modes [degrees/s].
        /// </summary>
        public double AzimuthSteeringRate { get; set; }
    }

    public class DownlinkInformation
    {
        /// <summary>
        /// Swath from which this downlink information data was extracted
        /// </summary>
        public SwathType Swath { get; set; }

        /// <summary>
        /// Zero Doppler azimuth time at which this set of downlink information applies [UTC].
        /// </summary>
        public DateTimeOffset AzimuthTime { get; set; }

        /// <summary>
        /// Sensing time of first line of input data [UTC]. 
        /// </summary>
        public DateTimeOffset FirstLineSensingTime { get; set; }

        /// <summary>
        /// Sensing time of last line of input data [UTC]
        /// </summary>
        public DateTimeOffset LastLineSensingTime { get; set; }

        /// <summary>
        /// Pulse repetition frequency (PRF) of the input raw data [Hz]. This is the inverse of the PRI extracted from the downlink for this swath.
        /// </summary>
        public double PRF { get; set; }

        /// <summary>
        /// Error counters. This record contains the error counter for each field that is validated as the input source packets are analyzed.
        /// </summary>
        public BitErrorCount BitErrorCount { get; set; }

        /// <summary>
        /// Downlink values. This record contains values extracted directly from the Instrument Source Packets. 
        /// </summary>
        public DownlinkValues DownlinkValues { get; set; }
    }

    public class BitErrorCount
    {
        /// <summary>
        /// Number of errors detected in the sync marker field. 
        /// </summary>
        public int ErrorSyncMarker { get; set; }

        /// <summary>
        /// Number of errors detected in the data take identifier field
        /// </summary>
        public int ErrorDateTakeId { get; set; }

        /// <summary>
        /// Number of errors detected in the Event Control Code (ECC) number field. 
        /// </summary>
        public int ErrorEccNumber { get; set; }

        /// <summary>
        /// Number of errors detected in the test mode field. 
        /// </summary>
        public int ErrorTestMode { get; set; }

        /// <summary>
        /// Number of errors detected in the Rx channel identifier field. 
        /// </summary>
        public int ErrorRxChannelId { get; set; }

        /// <summary>
        /// Number of errors detected in the instrument configuration identifier field. 
        /// </summary>
        public int ErrorInstrumentConfigId { get; set; }

        /// <summary>
        /// Number of errors detected in the space packet count field. 
        /// </summary>
        public int ErrorPacketCount { get; set; }

        /// <summary>
        /// Number of errors detected in the Pulse Repetition Interval (PRI) count field. 
        /// </summary>
        public int ErrorPriCount { get; set; }

        /// <summary>
        /// Number of packets in which the SSB Error Flag is set to true.
        /// </summary>
        public int ErrorSsbErrorFlag { get; set; }

        /// <summary>
        /// Number of errors detected in the Block Adaptive Quantisation (BAQ) mode field. 
        /// </summary>
        public int ErrorBaqMode { get; set; }

        /// <summary>
        /// Number of errors detected in the BAQ block length field. 
        /// </summary>
        public int ErrorBaqBlockLength { get; set; }

        /// <summary>
        /// Number of errors detected in the range decimation field. 
        /// </summary>
        public int ErrorRangeDecimation { get; set; }

        /// <summary>
        /// Number of errors detected in the Rx gain field
        /// </summary>
        public int ErrorRxGain { get; set; }

        /// <summary>
        /// Number of errors detected in the Tx ramp rate field. 
        /// </summary>
        public int ErrorTxRampRate { get; set; }

        /// <summary>
        /// Number of errors detected in the Tx pulse start frequency field. 
        /// </summary>
        public int ErrorTxPulseStartFrequency { get; set; }

        /// <summary>
        /// Number of errors detected in the rank field. 
        /// </summary>
        public int ErrorRank { get; set; }

        /// <summary>
        /// Number of errors detected in the PRI code field 
        /// </summary>
        public int ErrorPri { get; set; }

        /// <summary>
        /// Number of errors detected in the sampling window start time (SWST) field. 
        /// </summary>
        public int ErrorSwst { get; set; }

        /// <summary>
        /// Number of errors detected in the sampling window length (SWL) field. 
        /// </summary>
        public int ErrorSwl { get; set; }

        /// <summary>
        /// Number of errors detected in the polarisation field. 
        /// </summary>
        public int ErrorPolarisation { get; set; }

        /// <summary>
        /// Number of errors detected in the temperature compensation field. 
        /// </summary>
        public int ErrorTempComp { get; set; }

        /// <summary>
        /// Number of errors detected in the elevation beam address field. 
        /// </summary>
        public int ErrorElevationBeamAddress { get; set; }

        /// <summary>
        /// Number of errors detected in the azimuth beam address field. 
        /// </summary>
        public int ErrorAzimuthBeamAddress { get; set; }

        /// <summary>
        /// Number of errors detected in the SAR Antenna Sub-system (SAS) test mode field. 
        /// </summary>
        public int ErrorSasTestMode { get; set; }

        /// <summary>
        /// Number of errors detected in the calibration operation type field. 
        /// </summary>
        public int ErrorCalType { get; set; }

        /// <summary>
        /// Number of errors detected in the calibration beam address field. 
        /// </summary>
        public int ErrorCalibrationBeamAddress { get; set; }

        /// <summary>
        /// Number of errors detected in the calibration mode field. 
        /// </summary>
        public int ErrorCalMode { get; set; }

        /// <summary>
        /// Number of errors detected in the Tx pulse number field
        /// </summary>
        public int ErrorTxPulseNumber { get; set; }

        /// <summary>
        /// Number of errors detected in the signal type field. 
        /// </summary>
        public int ErrorSignalType { get; set; }

        /// <summary>
        /// Number of errors detected in the swap flag field. 
        /// </summary>
        public int ErrorSwapFlag { get; set; }

        /// <summary>
        /// Number of errors detected in the swath number field. 
        /// </summary>
        public int ErrorSwathNumber { get; set; }

        /// <summary>
        /// Number of errors detected in the number of quads field.
        /// </summary>
        public int ErrorNumberOfQuads { get; set; }

        /// <summary>
        /// Total number of errors detected in ISP headers
        /// </summary>
        public int IspHeaderErrors { get; set; }
    }

    public class DownlinkValues
    {
        /// <summary>
        /// Pulse Repetition Interval [s]. 
        /// </summary>
        public double Pri { get; set; }

        /// <summary>
        /// The number of PRI between transmitted pulse and return echo. 
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Data take identifier. 
        /// </summary>
        public int DataTakeId { get; set; }

        /// <summary>
        /// The ECC number of the measurement mode. 
        /// </summary>
        public int EccNumber { get; set; }

        /// <summary>
        /// Receive channel identifier. 
        /// </summary>
        public int RxChannelId { get; set; }

        /// <summary>
        /// Instrument configuration identifier. 
        /// </summary>
        public int InstrumentConfigId { get; set; }

        /// <summary>
        /// Data format for instrument samples. There is one element corresponding to the data format for each packet type in the segment.
        /// </summary>
        public DataFormat DataFormat { get; set; }

        /// <summary>
        /// Decimation of the SAR data in the sampling window according to the needed mode bandwidth.
        /// </summary>
        public RangeDecimation RangeDecimation { get; set; }

        /// <summary>
        /// Applied value of the commandable Rx attenuation in the receiver channel of the SES. 
        /// </summary>
        public double RxGain { get; set; }

        /// <summary>
        /// Transmit pulse length [s]. 
        /// </summary>
        public double TxPulseLength { get; set; }

        /// <summary>
        /// Starting frequency of the transmit pulse [Hz].
        /// </summary>
        public double TxPulseStartFrequency { get; set; }

        /// <summary>
        /// The linear FM rate at which the frequency changes over the pulse duration [Hz/s]. 
        /// </summary>
        public double TxPulseRampRate { get; set; }

        /// <summary>
        /// SPPDU swath number identifier. This is of type SwathNumberType which is a range from 0..127
        /// </summary>
        public int SwathNumber { get; set; }

        /// <summary>
        /// List of sampling window lengths. 
        /// </summary>
        public IList<SamplingWindowLenght> SamplingWindowLengths { get; set; }

        /// <summary>
        /// List of sampling window start time changes. 
        /// </summary>
        public IList<SamplingWindowStartTime> SamplingWindowStartTimes { get; set; }

        /// <summary>
        /// List of pointing status changes
        /// </summary>
        public IList<PointingStatus> PointingStatuses { get; set; }
    }

    public class SamplingWindowLenght
    {
        /// <summary>
        /// Zero Doppler azimuth time of sampling window length change [UTC]. 
        /// </summary>
        public DateTimeOffset AzimuthTime { get; set; }

        /// <summary>
        /// Sampling Window Length [s]
        /// </summary>
        public double Value { get; set; }
    }

    public class SamplingWindowStartTime
    {
        /// <summary>
        /// Zero Doppler azimuth time of sampling window start change [UTC]. 
        /// </summary>
        public DateTimeOffset AzimuthTime { get; set; }

        /// <summary>
        /// Sampling window start time for first range sample [s]. 
        /// </summary>
        public double Value { get; set; }
    }

    public class PointingStatus
    {
        /// <summary>
        /// Zero Doppler azimuth time of the pointing status change [UTC]. 
        /// </summary>
        public DateTimeOffset AzimuthTime { get; set; }

        /// <summary>
        /// AOCS operational mode. 
        /// </summary>
        public AocsOpModeType AocsOpMode { get; set; }

        /// <summary>
        /// Roll error status. Set to false when the roll axis is fine pointed and set to true when the roll axis is degraded.
        /// </summary>
        public bool RollError { get; set; }

        /// <summary>
        /// Pitch error status. Set to false when the pitch axis is fine pointed and set to true when the pitch axis is degraded.
        /// </summary>
        public bool PitchError { get; set; }

        /// <summary>
        /// Yaw error status. Set to false when the yaw axis is fine pointed and set to true when the yaw axis is degraded.
        /// </summary>
        public bool YawError { get; set; }
    }

    public class DataFormat
    {
        /// <summary>
        /// BAQ block length for all packets. Not sure why this is of type ubyte.
        /// </summary>
        public int BaqBlockLength { get; set; }

        /// <summary>
        /// Data format of echo packets. 
        /// </summary>
        public DataFormatMode EchoFormat { get; set; }

        /// <summary>
        /// Data format of noise packets. 
        /// </summary>
        public DataFormatMode NoiseFormat { get; set; }

        /// <summary>
        /// Data format of calibration packets.
        /// </summary>
        public DataFormatMode CalibrationFormat { get; set; }

        /// <summary>
        /// The calculated mean FDBAQ bit rate code for echo packets over the entire segment. This field applies only when the echoFormat is FDBAQ.
        /// </summary>
        public double MeanBitRate { get; set; }
    }

    public class RangeDecimation
    {
        /// <summary>
        /// Filter bandwidth used to decimate the SAR signal data [Hz]. 
        /// </summary>
        public double DecimationFilterBandwidth { get; set; }

        /// <summary>
        /// Sampling frequency of the SAR signal data after decimation [Hz]. This frequency is equivalent to the to the sampling frequency before decimation multiplied by the decimation ratio.
        /// </summary>
        public double SamplingFrequencyAfterDecimation { get; set; }

        /// <summary>
        /// Length of the decimation filter [samples]
        /// </summary>
        public int FilterLength { get; set; }
    }

    public class Orbit
    {
        /// <summary>
        /// Timestamp at which orbit state vectors apply [UTC]. 
        /// </summary>
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// Reference frame of the orbit state data.
        /// </summary>
        public ReferenceFrameType Frame { get; set; }

        /// <summary>
        /// Position vector record. This record contains the platform position data with respect to the Earth-fixed reference frame.
        /// Note: The Earth fixed reference frame in use is the IERS Terrestrial Reference Frame(ITRF).The zero longitude or IERS Reference Meridian(IRM), as well as the IERS Reference Pole(IRP), 
        /// are maintained by the International Earth Rotation Service(IERS), based on a large number of observing stations, and define the IERS Terrestrial Reference Frame(ITRF).
        /// More details can be found in Earth Observation Mission CFI Software documentation or at https://en.wikipedia.org/wiki/ECEF.
        /// </summary>
        public Vector3D Position { get; set; }

        /// <summary>
        /// Velocity vector record. This record contains the platform velocity data with respect to the Earth-fixed reference frame.
        /// </summary>
        public Vector3D Velocity { get; set; } 
    }

    public class Attitude
    {
        /// <summary>
        /// Timestamp to which attitude data applies [UTC]. 
        /// </summary>
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// Reference frame of the attitude data. 
        /// </summary>
        public ReferenceFrameType Frame { get; set; }

        /// <summary>
        /// Q0 attitude quaternion as extracted from ancillary attitude data.
        /// </summary>
        public double Q0 { get; set; }

        /// <summary>
        /// Q1 attitude quaternion as extracted from ancillary attitude data.
        /// </summary>
        public double Q1 { get; set; }

        /// <summary>
        /// Q2 attitude quaternion as extracted from ancillary attitude data.
        /// </summary>
        public double Q2 { get; set; }

        /// <summary>
        /// Q3 attitude quaternion as extracted from ancillary attitude data.
        /// </summary>
        public double Q3 { get; set; }

        /// <summary>
        /// Angular velocity vector as extracted from ancillary attitude data [degrees/s]. 
        /// </summary>
        public Vector3D AngularVelocity { get; set; }

        /// <summary>
        /// Platform roll calculated from ancillary attitude data [degrees]. 
        /// </summary>
        public double Roll { get; set; }

        /// <summary>
        /// Platform pitch calculated from ancillary attitude data [degrees]
        /// </summary>
        public double Pitch { get; set; }

        /// <summary>
        /// Platform yaw calculated from ancillary attitude data [degrees].
        /// </summary>
        public double Yaw { get; set; }
    }

    /// <summary>
    /// Enumeration of coordinate system reference frames supported by the EO CFI.
    /// </summary>
    public enum ReferenceFrameType
    {
        Undefined,
        Galactic,
        BM1950,
        BM2000,
        HM2000,
        GM2000,
        MeanOfDate,
        TrueOfDate,
        PseudoEarthFixed,
        EarthFixed,
        Topocentric,
        SatelliteOrbital,
        SatelliteNominal,
        SatelliteAttitude,
        InstrumentAttitude
    }

    /// <summary>
    /// Enumeration of compression method names. This enumeration is a consolidated list from the
    /// Sentinel-1 SPPDU document and the ENVISAT Product Specification.
    /// </summary>
    public enum DataFormatMode
    {
        FDBAQ,
        BAQ3Bit,
        BAQ4Bit,
        BAQ5Bit,
        Decimation,
        Bypass,
        Full8Bit,
        SM,
        FBAQ2Bit,
        FBAQ3Bit,
        FBAQ4Bit,
        None
    }

    /// <summary>
    /// Enumeration of the image projection.
    /// </summary>
    public enum ProjectionType
    {
        SlantRange,
        GroundRange
    }

    /// <summary>
    /// Enumeration of the available AOCS operational mode from the pointing status in the downlink.
    /// </summary>
    public enum AocsOpModeType
    {
        NoMode,
        NormalPointingMode,
        OrbitControlMode
    }
}