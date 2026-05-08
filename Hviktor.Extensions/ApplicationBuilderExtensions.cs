using System.Text;
using Hviktor.Extensions.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Hviktor.Extensions;

/// <summary>
/// Extension methods for IApplicationBuilder to integrate Hviktor functionalities.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Middleware to inject Hviktor scripts and styles into HTML responses.
    /// </summary>
    /// <param name="app">The application builder to configure.</param>
    /// <param name="componentTypes">Component types to render and inject into the body.</param>
    /// <returns><b><see cref="IApplicationBuilder"/></b></returns>
    public static IApplicationBuilder UseHviktor(this IApplicationBuilder app, params Type[] componentTypes)
    {
        app.UseRequestLocalization();
        app.Use(async (context, next) =>
        {
            // Skip non-HTML requests early (static files, API calls, etc.)
            var path = context.Request.Path.Value ?? "";
            if (path.Contains('.') || path.StartsWith("/_"))
            {
                await next();
                return;
            }

            var originalBody = context.Response.Body;
            using var newBody = new MemoryStream();
            context.Response.Body = newBody;

            await next();

            newBody.Seek(0, SeekOrigin.Begin);
            context.Response.Body = originalBody;

            // Only process successful HTML responses
            if (context.Response.StatusCode is >= 200 and < 300
                && context.Response.ContentType?.Contains("text/html") == true
                && newBody.Length > 0)
            {
                var html = await new StreamReader(newBody).ReadToEndAsync();

                if (componentTypes.Length > 0)
                {
                    // Render ReconnectModal component
                    var renderer = context.RequestServices.GetRequiredService<HtmlRenderer>();
                    var stylesheets = new StringBuilder();
                    var componentsHtml = new StringBuilder();

                    foreach (var componentType in componentTypes)
                    {
                        // Split capitalized letters into separate words
                        var splitByCapitalization = componentType.Name.SplitByCapitalization();
                        var fileName = string.Join("-", splitByCapitalization);

                        var css = $"<link rel=\"stylesheet\" href=\"/_content/Hviktor/dist/assets/{fileName.ToLower()}.css\">";
                        stylesheets.Append(css);

                        var componentBodyHtml = await renderer.Dispatcher.InvokeAsync(async () =>
                        {
                            var output = await renderer.RenderComponentAsync(componentType);
                            return output.ToHtmlString();
                        });
                        componentsHtml.Append(componentBodyHtml);
                    }

                    // Append to end of head and body:
                    html = html.Replace("</head>", $"{stylesheets}</head>");
                    html = html.Replace("</body>", $"{componentsHtml}</body>");
                }

                html = html.Replace("</body>", "<script type=\"module\" src=\"/_content/Hviktor/dist/entry.js\"></script></body>");

                context.Response.ContentLength = null;
                await context.Response.WriteAsync(html);
            }
            else
            {
                // Copy original response without modification
                newBody.Seek(0, SeekOrigin.Begin);
                await newBody.CopyToAsync(originalBody);
            }
        });

        return app;
    }
}