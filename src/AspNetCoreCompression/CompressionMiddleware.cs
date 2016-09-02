using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreCompression
{
    public class CompressionMiddleware
    {
        private const string AcceptEncodingName = "Accept-Encoding";
        private const string ContentEncodingName = "Content-Encoding";
        private const string ContentLengthName = "Content-Length";
        private const string GzipEncodingType = "gzip";

        private readonly RequestDelegate _next;

        public CompressionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var acceptEncoding = context.Request.Headers[AcceptEncodingName];

            if (!IsGzipAllowed(acceptEncoding))
            {
                await _next(context);
                return;
            }

            using (var buffer = new MemoryStream())
            {
                var body = context.Response.Body;
                context.Response.Body = buffer;

                try
                {
                    await _next(context);

                    using (var compressed = new MemoryStream())
                    {
                        using (var gzip = new GZipStream(compressed, CompressionLevel.Optimal, leaveOpen: true))
                        {
                            buffer.Seek(0, SeekOrigin.Begin);
                            await buffer.CopyToAsync(gzip);
                        }

                        context.Response.Headers.Add(ContentEncodingName, new[] { GzipEncodingType });

                        if (context.Response.Headers[ContentLengthName].Count > 0)
                        {
                            context.Response.Headers[ContentLengthName] = compressed.Length.ToString();
                        }

                        compressed.Seek(0, SeekOrigin.Begin);
                        await compressed.CopyToAsync(body);
                    }
                }
                finally
                {
                    context.Response.Body = body;
                }
            }
        }

        private static bool IsGzipAllowed(string acceptEncodingHeader)
        {
            if (String.IsNullOrWhiteSpace(acceptEncodingHeader))
            {
                return false;
            }

            var acceptTypes = acceptEncodingHeader.Split(',').Select(x => x.ToLower());

            if (!acceptTypes.Contains(GzipEncodingType))
            {
                return false;
            }

            return true;
        }
    }
}