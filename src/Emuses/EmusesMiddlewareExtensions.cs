using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace Emuses
{
    public static class EmusesMiddlewareExtensions
    {
        public static IApplicationBuilder UseEmuses(this IApplicationBuilder builder, int sessionTimeout, IStorage storage, string sessionExpiredPage, List<string> noSessionAccessPages)
        {
            return builder.UseMiddleware<EmusesMiddleware>(sessionTimeout, storage, sessionExpiredPage, noSessionAccessPages);
        }
    }
}
