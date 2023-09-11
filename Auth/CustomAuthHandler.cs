using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using StackExchange.Redis;

namespace Use_Wheels.Auth
{
	public class CustomAuthHandler
	{
        public bool IsValidToken(string jwtToken, string issuer, string audience, string metadataAddress)
        {
            var openIdConnectConfig = AuthConfigManager.GetMetaData(metadataAddress);
            var signingKeys = openIdConnectConfig.SigningKeys;
            return ValidateToken(jwtToken, issuer, audience, signingKeys);
        }

        private bool ValidateToken(string jwtToken, string issuer, string audience, ICollection<SecurityKey> signingKeys)
        {
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1),
                    RequireSignedTokens = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeys = signingKeys,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience
                };
                ISecurityTokenValidator tokenValidator = new JwtSecurityTokenHandler();
                var claim = tokenValidator.ValidateToken(jwtToken, validationParameters, out
                    var _);
                var scope = claim.FindFirst(c => c.Type.ToLower() == "<Here Scope Type>" && (c.Value.ToLower() == "<Here Scope>"));
                if (scope == null)
                    throw new BadHttpRequestException(Constants.Authorization.INVALID_SCOPE_AUTHORIZATION_FAILED, Constants.ResponseConstants.NOT_FOUND);

                var redis = ConnectionMultiplexer.Connect(Constants.Configurations.REDIS_CONNECTION_KEY);
                IDatabase db = redis.GetDatabase();
                // Retrieve the value associated with a key from this specific database
                var value = db.StringGet(jwtToken);

                if(value == "1")
                {
                    throw new BadHttpRequestException(Constants.Authorization.AUTHORIZATION_FAILED, Constants.ResponseConstants.NOT_FOUND);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new BadHttpRequestException(Constants.Authorization.AUTHORIZATION_FAILED, Constants.ResponseConstants.NOT_FOUND);
            }
        }
    }
}

