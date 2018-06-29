using System;
using System.Xml;
using SharpSentinel.Parser.Data.ManifestObjects;
using SharpSentinel.Parser.Data.S1.Annotations;
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
            header.MissionId = adsHeaderNode.SelectSingleNode("missionId")?.InnerText;
            header.ProductType = (ProductTypeType)Enum.Parse(typeof(ProductTypeType), adsHeaderNode.SelectSingleNode("productType").InnerText);
            header.Polarisation = (TransmitterReceiverPolarisationType)Enum.Parse(typeof(TransmitterReceiverPolarisationType), adsHeaderNode.SelectSingleNode("polarisation").InnerText);
            header.Mode = (InstrumentModeType)Enum.Parse(typeof(InstrumentModeType), adsHeaderNode.SelectSingleNode("mode").InnerText);
            header.Swath = (SwathType)Enum.Parse(typeof(SwathType), adsHeaderNode.SelectSingleNode("swath").InnerText);
            header.StartTime = DateTimeOffset.Parse(adsHeaderNode.SelectSingleNode("startTime").InnerText);
            header.StopTime = DateTimeOffset.Parse(adsHeaderNode.SelectSingleNode("stopTime").InnerText);
            header.AbsoluteOrbitNumber = int.Parse(adsHeaderNode.SelectSingleNode("absoluteOrbitNumber").InnerText);
            header.MissionDataTakeId = int.Parse(adsHeaderNode.SelectSingleNode("missionDataTakeId").InnerText);
            header.ImageNumber = int.Parse(adsHeaderNode.SelectSingleNode("imageNumber").InnerText);
            return header;
        }
    }
}