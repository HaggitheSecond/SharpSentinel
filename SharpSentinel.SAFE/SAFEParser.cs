using System;
using System.Threading.Tasks;
using SharpSentinel.Parser.Data;
using SharpSentinel.Parser.Data.Internal;
using SharpSentinel.Parser.Helpers;
using SharpSentinel.Parser.Parsers;

namespace SharpSentinel.Parser
{
    /// <summary>
    /// The entry point to parse sentinel 1 and 2 datasets.
    /// </summary>
    public static class SAFEParser
    {
        //TODO: implement everything here

        /// <summary>
        /// Detects platform and loads data from a SAFE directory or archive asynchronously 
        /// </summary>
        /// <param name="path">Path to the directory or the archive</param>
        /// <returns>The parsed data</returns>
        public static async Task<BaseData> OpenDataSetAsync(string path)
        {
            var platform = SAFEHelper.DetectPlatform(path);

            BaseData data;

            switch (platform)
            {
                case PlatformType.Sentinel1:
                    data = await OpenSentinel1DataSetAsync(path);
                    break;
                case PlatformType.Sentinel2:
                    data = await OpenSentinel1DataSetAsync(path);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return data;
        }

        /// <summary>
        /// Loads data from a SAFE Sentinel 1 directory or archive asynchronously
        /// </summary>
        /// <param name="path">Path to the directory or the archive</param>
        /// <returns>The parsed SAR-C data</returns>
        public static async Task<S1Data> OpenSentinel1DataSetAsync(string path)
        {
            DirectoryHelper.EnsureS1SAFEDirectory(path);

            var manifest = ManifestParser.Parse(path);


            return null;
        }

        /// <summary>
        /// Loads data from a SAFE Sentinel 2 directory or archive asynchronously
        /// </summary>
        /// <param name="path">Path to the directory or the archive</param>
        /// <returns>The parsed MSI data</returns>
        public static async Task<S2Data> OpenSentine2DataSetAsync(string path)
        {
            return null;
        }
    }
}