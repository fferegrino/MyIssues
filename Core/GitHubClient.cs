using Octokit;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyIssues
{
    public class GitHubClientFactory
    {
        public static GitHubClient CreateClient(string accessToken)
        {
            var productHeader = new Octokit.ProductHeaderValue("MyIssues", "0.1");
            var token = new Credentials(accessToken);

            var _client = new Octokit.GitHubClient(productHeader);
            _client.Credentials = token;

            return _client;
        }
    }
}