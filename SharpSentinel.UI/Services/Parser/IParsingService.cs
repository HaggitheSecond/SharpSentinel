using System.IO;
using System.Threading.Tasks;
using SharpSentinel.Parser;
using SharpSentinel.Parser.Data;
using SharpSentinel.Parser.Helpers;

namespace SharpSentinel.UI.Services.Parser
{
    public interface IParsingService : IService
    {
        Task<BaseData> LoadDataSet(string directoryPath);
    }

    public class ParsingService : IParsingService
    {
        public async Task<BaseData> LoadDataSet(string directoryPath)
        {
            Guard.NotNullOrWhitespace(directoryPath, nameof(directoryPath));

            return await SAFEParser.OpenDataSetAsync(directoryPath);
        }
    }
}