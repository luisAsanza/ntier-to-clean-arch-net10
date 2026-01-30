using System.Security.Cryptography;
using System.Text;

namespace CRUDExample.Middleware
{
    /// <summary>
    /// Provides middleware extension methods to configure Content Security Policy (CSP) headers
    /// and generate nonces for script/style whitelisting in views.
    /// </summary>
    public static class CspMiddlewareExtensions
    {
        private const string CspNonceItemKey = "CspNonce";
        private const string CspHeaderName = "Content-Security-Policy";
        private const string CspReportOnlyHeaderName = "Content-Security-Policy-Report-Only";

        public static IApplicationBuilder UseCsp(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                // 1 Generate a nonce
                string nonce = GenerateNonce();

                // 2 Store nonce so views/layouts can use it
                context.Items[CspNonceItemKey] = nonce;

                // 3 Add the nonce to the Content-Security-Policy header
                StringBuilder csp = BuildCspHeader(nonce);

                context.Response.Headers[CspHeaderName] = csp.ToString();

                await next();
            });
        }

        public static IApplicationBuilder UseCspReportOnly(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                // 1 Generate a nonce
                string nonce = GenerateNonce();

                // 2 Store nonce so views/layouts can use it
                context.Items[CspNonceItemKey] = nonce;

                // 3 Add the nonce to the Content-Security-Policy header
                StringBuilder csp = BuildCspHeader(nonce);

                context.Response.Headers[CspReportOnlyHeaderName] = csp.ToString();

                await next();
            });
        }

        /// <summary>
        /// Builds the CSP header value using the provided nonce for inline scripts and styles.
        /// </summary>
        /// <param name="nonce">The per-request nonce to include in the header.</param>
        /// <returns>A <see cref="StringBuilder"/> containing the CSP header value.</returns>
        private static StringBuilder BuildCspHeader(string nonce)
        {
            var csp = new StringBuilder();
            csp.Append("default-src 'self'; ");
            csp.Append($"script-src 'self' 'nonce-{nonce}' https:; ");
            csp.Append($"style-src 'self' 'nonce-{nonce}' https:; ");
            csp.Append("img-src 'self' data: blob: https:; ");
            csp.Append("font-src 'self' https:; ");
            csp.Append("connect-src 'self' https:; ");
            csp.Append("object-src 'none'; ");
            csp.Append("base-uri 'self'; ");
            csp.Append("frame-ancestors 'none'; ");
            csp.Append("form-action 'self'; ");
            csp.Append("manifest-src 'self'; ");
            csp.Append("media-src 'self'; ");
            csp.Append("worker-src 'self' blob:; ");
            return csp;
        }

        /// <summary>
        /// Generates a cryptographically secure, random nonce encoded as a Base64 string.
        /// </summary>
        /// <returns>A Base64-encoded string representing a 16-byte cryptographically secure random nonce.</returns>
        private static string GenerateNonce()
        {            
            var nonceBytes = RandomNumberGenerator.GetBytes(16);
            var nonce = Convert.ToBase64String(nonceBytes);
            return nonce;
        }
    }
}
