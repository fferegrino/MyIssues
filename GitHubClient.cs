using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyIssues
{
	public class GitHubClient
    {
        static GitHubClient _clientInstance;

        public static GitHubClient Client(string accessToken = null)
        {
            if (accessToken == null && _clientInstance == null)
                throw new ArgumentNullException(nameof(accessToken));

            return _clientInstance ??
                (_clientInstance = new GitHubClient(accessToken));
        }

        string _accessToken;
        Octokit.GitHubClient _client;

        private GitHubClient(string accessToken)
		{
            _accessToken = accessToken;
            var productHeader = new Octokit.ProductHeaderValue("MyIssues", "0.1");
            var token = new Credentials(accessToken);

            _client = new Octokit.GitHubClient(productHeader);
            _client.Credentials = token;
            
            //_repoClient = new Octokit.RepositoriesClient();
            //_client.Credentials = token;
        }

        public async Task<IReadOnlyList<Octokit.Repository>> GetRepositoriesForUser()
        {
            return await _client.Repository.GetAllForCurrent();
        }
	}
}
