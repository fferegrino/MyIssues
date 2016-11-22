using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIssues.Models
{
    public class Repository
    {
        public string Url { get; set; }

        public long Id { get;  set; }

        public string Owner { get;  set; }

        public string Name { get;  set; }

        public string FullName { get;  set; }

        public string Description { get;  set; }

        public string Homepage { get;  set; }

        public string Language { get;  set; }

        public bool IsPrivate { get;  set; }

        public int StargazersCount { get;  set; }

        public int OpenIssuesCount { get;  set; }

        public DateTimeOffset? PushedAt { get;  set; }

        public DateTimeOffset CreatedAt { get;  set; }

        public DateTimeOffset UpdatedAt { get;  set; }

        public bool HasIssues { get;  set; }
    }
}
