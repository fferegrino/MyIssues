using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIssues
{
    public class Startup
    {
        public static void Start()
        {
            MyIssues.DataAccess.Storage.Init();
        }
    }
}
