using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyIssues.Models
{
	public class Label : IEquatable<Label>, IEquatable<string>
    {
        /// <summary>
        /// Name of the label
        /// </summary>
        public string Name { get; set; }
        
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			var lbl = obj as Label;
			return lbl == null ? false : Equals(lbl);
		}

		public bool Equals(Label other)
		{
			return this.Name.Equals(other.Name);
		}

		public bool Equals(string other)
		{
			return this.Name.Equals(other);
		}
	}
}
