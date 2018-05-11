using System;
using System.Text;

namespace Emuses.Services
{
    public class Log
    {
        public string ComponentName { get; set; }

        public string Text { get; set; }

        public DateTime Created { get; set; }

        public string Head { get; set; }

        public string Value { get; set; }

        public Log(string componentName, string text)
        {
            ComponentName = componentName;
            Text = text;
            Created = DateTime.Now;
            Head = CreateHead();
            Value = CreateValue();
        }

        private string CreateHead()
        {
            var logHead = new StringBuilder("=== " + ComponentName + " ");

            var result = 70 - logHead.Length;
            for (var i = 0; i < result; i++)
                logHead.Append("=");

            return logHead.ToString();
        }

        private string CreateValue()
        {
            return DateTime.Now + ": " + Text;
        }
    }
}
