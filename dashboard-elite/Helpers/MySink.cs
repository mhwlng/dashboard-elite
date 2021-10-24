using System.Globalization;
using System.IO;
using System.Text;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace dashboard_elite.Helpers
{
    public class MySink : ILogEventSink
    {
        private readonly ITextFormatter _formatter;

        public MySink(ITextFormatter formatter)
        {
            _formatter = formatter;
        }


        public void Emit(LogEvent logEvent)
        {
            var buffer = new StringWriter(new StringBuilder(1024));
            _formatter.Format(logEvent, buffer);
            var message = buffer.ToString();

            // do stuff
        }
    }

    public static class MySinkExtensions
    {
        public static LoggerConfiguration MySink(
            this LoggerSinkConfiguration loggerConfiguration,
            string outputTemplate
        )
        {
            Serilog.Formatting.Display.MessageTemplateTextFormatter tf = new Serilog.Formatting.Display.MessageTemplateTextFormatter(outputTemplate, CultureInfo.InvariantCulture);

            return loggerConfiguration.Sink(new MySink(tf));
        }
    }
}
