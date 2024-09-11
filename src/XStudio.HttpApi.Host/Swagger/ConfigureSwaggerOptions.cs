using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IO;
using System;
using System.Linq;

namespace XStudio.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
            options.DocInclusionPredicate((docName, description) =>
            {
                if (description.Properties.ContainsKey(typeof(ApiVersion)))
                {
                    //ApiVersion apiVersion = (ApiVersion)description.Properties[typeof(ApiVersion)];
                    return docName.Equals(description.GroupName);
                }
                return true;
                //return true;
            });

            //加载说明文档
            string? docxml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Docs");
            while (!string.IsNullOrWhiteSpace(docxml) && !Directory.Exists(docxml))
            {
                docxml = Directory.GetParent(docxml)?.Parent?.FullName;
                if (docxml == null)
                {
                    break;
                }
                docxml = Path.Combine(docxml, "Docs");
            }
            if (!string.IsNullOrWhiteSpace(docxml) && Directory.Exists(docxml))
            {
                Directory.GetFiles(docxml, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file, true);
                });
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "XStudio API",
                Version = description.ApiVersion.ToString(),
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
