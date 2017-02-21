using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIssues.Models
{
    public class Issue : IssueComment
    {
        /// <summary>
        /// The URL for the HTML view of this issue.
        /// </summary>
        public Uri HtmlUrl { get; set; }

        /// <summary>
        /// The issue number.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Whether the issue is open or closed.
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Title of the issue
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The set of labels applied to the issue
        /// </summary>
        public IReadOnlyList<Label> Labels { get; set; }

        /// <summary>
        /// The user this issue is assigned to.
        /// </summary>
        public string Assignee { get; set; }

        /// <summary>
        /// The milestone, if any, that this issue is assigned to.
        /// </summary>
        public string Milestone { get; set; }

        /// <summary>
        /// The date the issue was closed if closed.
        /// </summary>
        public DateTimeOffset? MilestoneDueDate { get; set; }

        /// <summary>
        /// The date the issue was closed if closed.
        /// </summary>
        public DateTimeOffset? ClosedAt { get; set; }


        /// <summary>
        /// If the issue is locked or not.
        /// </summary>
        public bool Locked { get; set; }
    }
}