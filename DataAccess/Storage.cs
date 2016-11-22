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
        public long _repoId;


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

        public async Task SetWorkingRepo(long repoId)
        {
            _repoId = repoId;
            await SaveWorkingRepo(repoId);
            await FetchLabels(repoId);
        }

        #region Labels 
        public static Dictionary<string, int[]> LabelColors;

        private async Task FetchLabels(long repoId)
        {
            var serviceLabels = await _client.Issue.Labels.GetAllForRepository(repoId);
            var labels = serviceLabels.Select(l => l.Map()).ToList();
            await BlobCache.LocalMachine.InsertObject(Labels, labels);

            LabelColors = labels.ToDictionary(l => l.Name, l => new int[] { l.R, l.G, l.B });
        }

        public async Task<List<Label>> GetLabels()
        {
            return await BlobCache.LocalMachine.GetObject<List<Label>>(Labels);
        }

        #endregion

        #region Issues

        public async Task<Octokit.Issue> GetIssue(int number)
        {

            return await _client.Issue.Get(_repoId, number);
        }

        public void GetIssues(Action<List<Models.Issue>> action)
        {
            Func<Task<List<Models.Issue>>> fetchFunc =   async () =>
                {
                    var a = await _client.Issue.GetAllForRepository(_repoId);
                    return a.Select(issue => issue.Map()).ToList();
            };
            BlobCache.LocalMachine.GetAndFetchLatest(Issues, fetchFunc)
                .Subscribe(action);
        }

        public async Task< List<Models.Issue>> GetIssues()
        {
            Func<Task<List<Models.Issue>>> fetchFunc = async () =>
            {
                var a = await _client.Issue.GetAllForRepository(_repoId);
                return a.Select(issue => issue.Map()).ToList();
            };
            return await BlobCache.LocalMachine.GetOrFetchObject(Issues, fetchFunc);
        }

        #endregion

        public async Task<List<Models.Repository>> GetRepositoriesForUser()
        {
            var repositoryRequest = new Octokit.RepositoryRequest
            {
                Type = Octokit.RepositoryType.Owner,
                Sort = Octokit.RepositorySort.Pushed
            };

            Func<Task<List<Models.Repository>>> fetchFunc = async () =>
            {
                var a = await _client.Repository.GetAllForCurrent(repositoryRequest);
                return a.Select(obj => obj.Map()).ToList();
            };

            return await BlobCache.LocalMachine.GetOrFetchObject(Repositories, fetchFunc);
        }

        public async Task<Octokit.Repository> GetRepo(long repoId)
        {
            return await _client.Repository.Get(repoId);
        }

        public async Task<IReadOnlyList<Octokit.IssueComment>> GetIssueComments(int number)
        {

            return await _client.Issue.Comment.GetAllForIssue(_repoId, number);
        }
    }
}
