﻿namespace Commons.Token;

public static class SwaggerGenOptionsExtensions
{
    public static void AddAuthenticationHeader(this SwaggerGenOptions c)
    {
        c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
        {
            Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Authorization"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Authorization"
                        },
                        Scheme = "oauth2",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
    }
}