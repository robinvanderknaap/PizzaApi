using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Api.Infrastructure.Util
{
    public static class AuthenticationHeaderValueExtensions
    {
        public static string Username(this AuthenticationHeaderValue authenticationHeaderValue)
        {
            return DecryptAuthenticationHeaderValue(authenticationHeaderValue).Key;
        }

        public static string Password(this AuthenticationHeaderValue authenticationHeaderValue)
        {
            return DecryptAuthenticationHeaderValue(authenticationHeaderValue).Value;
        }

        private static KeyValuePair<string, string> DecryptAuthenticationHeaderValue(AuthenticationHeaderValue authenticationHeaderValue)
        {
            // Base64 decode authentication header
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var credentials = encoding.GetString(Convert.FromBase64String(authenticationHeaderValue.Parameter));

            // Extract api client id and api access key from authentication header
            var parts = credentials.Split(':');
            var username = parts[0].Trim();
            var password = parts[1].Trim();

            // When one of the parts is empty, throw exception
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new FormatException();
            }

            return new KeyValuePair<string, string>(username, password);
        }

    }
}