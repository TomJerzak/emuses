using System.Collections.Generic;

namespace Emuses.Dashboard.Template
{
    public class IndexSettings
    {
        public string DocTitle { get; set; } = "emuses - dashboard";

        public IDictionary<string, string> ToTemplateParameters()
        {
            return new Dictionary<string, string>
            {
                {"%(DocumentTitle)", DocTitle}
            };
        }
    }
}
