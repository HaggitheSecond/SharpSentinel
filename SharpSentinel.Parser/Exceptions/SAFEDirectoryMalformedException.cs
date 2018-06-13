using System;
using System.Runtime.Serialization;

namespace SharpSentinel.Parser.Exceptions
{
    public class SAFEDirectoryMalformedException : Exception
    {
        public SAFEDirectoryMalformedException()
        {
        }

        public SAFEDirectoryMalformedException(string message) : base(message)
        {
        }

        public SAFEDirectoryMalformedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SAFEDirectoryMalformedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}