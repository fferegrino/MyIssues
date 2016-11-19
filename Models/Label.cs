using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyIssues.Models
{
    public class Label
    {
        /// <summary>
        /// Name of the label
        /// </summary>
        public string Name { get; set; }
        
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }
}
