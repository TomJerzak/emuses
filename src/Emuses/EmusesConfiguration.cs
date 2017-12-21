using System.Collections.Generic;

namespace Emuses
{
    public class EmusesConfiguration
    {
        public string OpenSessionPage { get; set; }

        public string SessionExpiredPage { get; set; }

        public List<string> NoSessionAccessPages { get; set; }

        public ISessionStorage Storage { get; set; }
    }
}