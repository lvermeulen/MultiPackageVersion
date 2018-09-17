using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using MultiPackageVersion.Core;

namespace MultiPackageVersion.Builders.Jenkins
{
    public class JenkinsBuilder : IBuilder
    {
        private readonly string _baseUrl;
        private readonly byte[] _encodedAuthorization;
        private readonly string _token;

        public JenkinsBuilder(string baseUrl, string userName, string password, string token)
        {
            _baseUrl = baseUrl;
            _encodedAuthorization = Encoding.ASCII.GetBytes($"{userName}:{password}");
            _token = token;
        }

        private bool TriggerBuildDefinition(string buildDefinition)
        {
            string url = $"{_baseUrl}/view/Libraries-Release/job/{buildDefinition}/buildWithParameters?token={_token}";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(_encodedAuthorization));
            client
                .PostAsync(url, new StringContent(""))
                .GetAwaiter()
                .GetResult();

            return true;
        }

        public bool Build(params string[] buildDefinitions)
        {
            foreach (string buildDefinition in buildDefinitions)
            {
                bool success = TriggerBuildDefinition(buildDefinition);
                if (!success)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
