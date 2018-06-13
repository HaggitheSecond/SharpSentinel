using System.IO;

namespace SharpSentinel.Parser.Data.Internal.FileTypes
{
    public abstract class DataFile
    {
        public string FilePath { get; }

        internal DataFile(string filePath)
        {
            if(File.Exists(filePath) == false)
                throw new FileNotFoundException($"File {filePath} could not be found");

            this.FilePath = filePath;
        }
    }
}