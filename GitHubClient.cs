using Octokit;
using System;
using System.Linq;
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

		public async Task<IReadOnlyList<Octokit.Repository>> SearchRepositoriesForUser(string repoName)
		{
			var request = new SearchRepositoriesRequest(repoName)
			{
				User = "fferegrino"
			};
			var result = await _client.Search.SearchRepo(request);
			return result.Items;
		}

        public async Task<Octokit.Repository> GetRepo(string name)
        {
            return await _client.Repository.Get("fferegrino", name);
        }

        public async Task<IReadOnlyList<Issue>> GetIssues(string repoName)
		{
			if (LabelColors == null)
			{
				await GetLabels();
			}
			return await _client.Issue.GetAllForRepository("fferegrino", repoName);
		}

		public static Dictionary<string, int[]> LabelColors;
		public async Task GetLabels()
		{
			LabelColors = new Dictionary<string, int[]>();
			var labels = await _client.Issue.Labels.GetAllForRepository("fferegrino", "that-c-sharp-guy");
			var lbls = labels.Select(l => new { l.Name, Color = GetColor(l.Color) });
			foreach (var ll in lbls)
			{
				LabelColors.Add(ll.Name, ll.Color);
			}
		}

		private int[] GetColor(string hex)
		{
			var colors = new int[3];
			colors[0] = Int32.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
			colors[1] = Int32.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
			colors[2] = Int32.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
			return colors;
		}

	}
}
