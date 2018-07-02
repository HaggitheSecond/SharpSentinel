using System;
using System.Xml;
using SharpSentinel.Parser.Data.ManifestObjects;
using SharpSentinel.Parser.Data.S1.Annotations;
using SharpSentinel.Parser.Extensions;
using SharpSentinel.Parser.Helpers;
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace SharpSentinel.Parser.Parsers.Annotations
{
    public static class AdsHeaderParser
    {
        public static AdsHeader Parse(XmlNode adsHeaderNode)
        {
            Guard.NotNull(adsHeaderNode, nameof(adsHeaderNode));

            var header = new AdsHeader();
            header.MissionId = adsHeaderNode.SelectSingleNodeThrowIfNull("missionId")?.InnerText;
            header.ProductType = (ProductTypeType)Enum.Parse(typeof(ProductTypeType), adsHeaderNode.SelectSingleNodeThrowIfNull("productType").InnerText);
            header.Polarisation = (TransmitterReceiverPolarisationType)Enum.Parse(typeof(TransmitterReceiverPolarisationType), adsHeaderNode.SelectSingleNodeThrowIfNull("polarisation").InnerText);
            header.Mode = (InstrumentModeType)Enum.Parse(typeof(InstrumentModeType), adsHeaderNode.SelectSingleNodeThrowIfNull("mode").InnerText);
            header.Swath = (SwathType)Enum.Parse(typeof(SwathType), adsHeaderNode.SelectSingleNodeThrowIfNull("swath").InnerText);
            header.StartTime = DateTimeOffset.Parse(adsHeaderNode.SelectSingleNodeThrowIfNull("startTime").InnerText);
            header.StopTime = DateTimeOffset.Parse(adsHeaderNode.SelectSingleNodeThrowIfNull("stopTime").InnerText);
            header.AbsoluteOrbitNumber = int.Parse(adsHeaderNode.SelectSingleNodeThrowIfNull("absoluteOrbitNumber").InnerText);
            header.MissionDataTakeId = int.Parse(adsHeaderNode.SelectSingleNodeThrowIfNull("missionDataTakeId").InnerText);
            header.ImageNumber = int.Parse(adsHeaderNode.SelectSingleNodeThrowIfNull("imageNumber").InnerText);
            return header;
        }
    }
}