using System;
using System.Runtime.Serialization;
using System.Xml;

namespace SharpSentinel.Parser.Exceptions.XML
{
    public class XmlNodeNotFoundException : XmlException
    {
        public XmlNodeNotFoundException()
        {
        }

        public XmlNodeNotFoundException(string message) : base(message)
        {
        }

        public XmlNodeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public XmlNodeNotFoundException(string message, Exception innerException, int lineNumber, int linePosition) : base(message, innerException, lineNumber, linePosition)
        {
        }

        protected XmlNodeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}