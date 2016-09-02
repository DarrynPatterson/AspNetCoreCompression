using Microsoft.AspNetCore.Builder;

namespace AspNetCoreCompression
{
    public static class CompressionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCompression(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CompressionMiddleware>();
        }
    }
}