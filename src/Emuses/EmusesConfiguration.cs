using System.Collections.Generic;

namespace Emuses
{
    public class EmusesConfiguration
    {
        public string OpenSessionPage { get; set; }

        public string SessionExpiredPage { get; set; }

        public List<string> NoSessionAccessPages { get; set; }

        public IStorage Storage { get; set; }
    }
}