using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Use_Wheels.Auth
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TokenValidator
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TokenValidator(RequestDelegate next, IConfiguration configuration)
        {
            _configuration = configuration;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.Authorization.Count == 0)
            {
                await _next(httpContext);
                return;
            }
            try
            {
                var path = httpContext.Request.Path;
                string token = string.Empty;
                string issuer = _configuration[Constants.Authorization.JWT_TOKEN_ISSUER]; //Get issuer value from your configuration
                string audience = _configuration[Constants.Authorization.JWT_TOKEN_AUDIENCE]; //Get audience value from your configuration
                string metaDataAddress = issuer + "/.well-known/oauth-authorization-server";
                CustomAuthHandler authHandler = new CustomAuthHandler();


                var header = httpContext.Request.Headers.Authorization.FirstOrDefault().Split(" ");
                
                if (header.ElementAtOrDefault(1) == null)
                    await _next(httpContext);
                string tokenValue = header.ElementAtOrDefault(1);

                var redis = ConnectionMultiplexer.Connect(Constants.Configurations.REDIS_CONNECTION_KEY);
                IDatabase db = redis.GetDatabase();

                // Retrieve the value associated with a key from Redis database
                var value = db.StringGet(tokenValue);

                if (value == Constants.Authorization.JWT_BLACKLISTED_TOKEN_VALUE)
                {
                    throw new BadHttpRequestException(Constants.Authorization.AUTHORIZATION_FAILED, Constants.ResponseConstants.BAD_REQUEST);
                }

            }
            catch (Exception)
            {
                throw new BadHttpRequestException(Constants.Authorization.UNAUTHORIZED, Constants.ResponseConstants.UNAUTHORIZED);
            }
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class TokenValidatorExtensions
    {
        public static IApplicationBuilder UseMiddlewareClassTemplate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenValidator>();
        }
    }
}

