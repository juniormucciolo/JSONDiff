using System.Web.Http;
using WebActivatorEx;
using JsonDiff;
using Swashbuckle.Application;
using System;
using System.IO;
using Swashbuckle.Swagger;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace JsonDiff
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {

                        c.SingleApiVersion("v1", "JsonDiff");
                        c.IncludeXmlComments(GetXmlCommentsPath());
                        new Info()
                        {
                            title = "JsonDiff",
                            contact = new Contact() {name = "Antonio W. Mucciolo Junior", email = "juniormucciolo@hotmail.com"},
                            description = "JsonDiff is a WEB API that can show differences between two json encoded as base64 binary.",
                            version = "v1"
                        };

                    })
                .EnableSwaggerUi(c =>
                    {

                    });
        }

        private static string GetXmlCommentsPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\JsonDiff.xml");
        }
    }
}
