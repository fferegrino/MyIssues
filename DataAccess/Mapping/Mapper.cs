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
    }
}
