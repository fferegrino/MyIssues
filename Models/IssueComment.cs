using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIssues.Models
{
    public class IssueComment
    {
        /// <summary>
        /// The Id for this issue
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Details about the issue.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The user that created the issue.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The date the issue was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        public int TotalReactions { get; set; }
        public int Plus1 { get; set; }
        public int Minus1 { get; set; }
        public int Laugh { get; set; }
        public int Confused { get; set; }
        public int Heart { get; set; }
        public int Hooray { get; set; }
    }
}