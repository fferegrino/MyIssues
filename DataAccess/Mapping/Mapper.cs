using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyIssues.Util;

namespace MyIssues.DataAccess.Mapping
{
    public static class MappingExtensions
    {
        public static MyIssues.Models.Label Map(this Octokit.Label label)
        {
            var colors = label.Color.GetInts();
            var l = new Models.Label
            {
                Name = label.Name,
                R = colors[0],
                G = colors[1],
                B = colors[2]
            };
            return l;
        }

        public static MyIssues.Models.Repository Map(this Octokit.Repository repo)
        {
            var l = new Models.Repository
            {
                CreatedAt = repo.CreatedAt,
                Description = repo.Description,
                FullName = repo.FullName,
                HasIssues = repo.HasIssues,
                Homepage = repo.Homepage,
                Id = repo.Id,
                Language = repo.Language,
                Name = repo.Name,
                OpenIssuesCount = repo.OpenIssuesCount,
                Owner = repo.Owner.Login,
                IsPrivate = repo.Private,
                PushedAt = repo.PushedAt,
                StargazersCount = repo.StargazersCount,
                UpdatedAt = repo.UpdatedAt,
                Url = repo.Url
            };
            return l;
        }

        public static MyIssues.Models.Issue Map(this Octokit.Issue issue)
        {
            var i = new Models.Issue
            {
                Assignee = issue.Assignee?.Login,
                Author = issue.User.Login,
                Body = issue.Body,
                ClosedAt = issue.ClosedAt,
                CreatedAt = issue.CreatedAt,
                HtmlUrl = issue.HtmlUrl,
                Id = issue.Id,
                IsOpen = issue.State == Octokit.ItemState.Open,
                Locked = issue.Locked,
                Number = issue.Number,
                Milestone = issue.Milestone?.Title,
                MilestoneDueDate = issue.Milestone?.DueOn,
                Title = issue.Title,
                Labels = issue.Labels.Select(m => m.Map()).ToList()
            };
            i.FillReactions(issue.Reactions);
            return i;
        }



        public static MyIssues.Models.IssueComment Map(this Octokit.IssueComment issue)
        {
            var i = new Models.IssueComment
            {
                Author = issue.User.Login,
                Body = issue.Body,
                CreatedAt = issue.CreatedAt,
                Id = issue.Id,
            };
            i.FillReactions(issue.Reactions);
            return i;
        }

        private static void FillReactions(this Models.IssueComment comment, Octokit.ReactionSummary reactions)
        {
            if(reactions != null)
            {
                comment.TotalReactions = reactions.TotalCount;
                comment.Plus1 = reactions.Plus1;
                comment.Minus1 = reactions.Minus1;
                comment.Heart = reactions.Heart;
                comment.Hooray = reactions.Hooray;
                comment.Laugh = reactions.Laugh;
                comment.Confused = reactions.Confused;
            }
        }


    }
}
