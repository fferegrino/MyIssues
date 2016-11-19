using Akavache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using MyIssues.Models;
using MyIssues.DataAccess.Mapping;

namespace MyIssues.DataAccess
{
    public partial class Storage
    {
        public static void Init()
        {
            BlobCache.ApplicationName = "MyIssues";
        }

        Octokit.GitHubClient _client;
        public string _user;
        public string _repo;


        static Storage _instance;
        public static Storage GetInstance()
        {
            return _instance ?? (_instance = new Storage());
        }

        public async Task SetClient(Octokit.GitHubClient client)
        {
            _client = client;

            var user = await _client.User.Current();
            _user = user.Login;
            await SaveCurrentLogin(_user);
        }

        public async Task SetWorkingRepo(string repo)
        {
            _repo = repo;
            await SaveWorkingRepo(repo);
            await FetchLabels(repo);
        }

        #region Labels 
        public static Dictionary<string, int[]> LabelColors;

        private async Task FetchLabels(string repo)
        {
            var serviceLabels = await _client.Issue.Labels.GetAllForRepository(_user, repo);
            var labels = serviceLabels.Select(l => l.Map()).ToList();
            await BlobCache.LocalMachine.InsertObject(Labels, labels);

            LabelColors = labels.ToDictionary(l => l.Name, l => new int[] { l.R, l.G, l.B });
        }

        public async Task<List<Label>> GetLabels()
        {
            return await BlobCache.LocalMachine.GetObject<List<Label>>(Labels);
        }

        #endregion


        public async Task<IReadOnlyList<Octokit.Repository>> GetRepositoriesForUser()
        {
            return await _client.Repository.GetAllForCurrent();
        }

        public async Task<IReadOnlyList<Octokit.Repository>> SearchRepositoriesForUser(string repoName)
        {
            var request = new Octokit.SearchRepositoriesRequest(repoName)
            {
                User = _user
            };
            var result = await _client.Search.SearchRepo(request);
            return result.Items;
        }

        public async Task<Octokit.Repository> GetRepo(string name)
        {
            return await _client.Repository.Get(_user, name);
        }

        public async Task<Octokit.Issue> GetIssue(int number)
        {

            return await _client.Issue.Get(_user, _repo, number);
        }

        public async Task<IReadOnlyList<Octokit.IssueComment>> GetIssueComments(int number)
        {

            return await _client.Issue.Comment.GetAllForIssue(_user, _repo, number);
        }

        public async Task<IReadOnlyList<Octokit.Issue>> GetIssues()
        {
            return await _client.Issue.GetAllForRepository(_user, _repo);
        }
    }
}
