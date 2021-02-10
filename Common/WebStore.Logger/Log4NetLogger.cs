using Microsoft.Extensions.Logging;
using System.Xml;

namespace WebStore.Logger
{
    class Log4NetLogger : ILogger
    {
        public Log4NetLogger(string Category, XmlElement Configuration)
        {

        }
    }
}
