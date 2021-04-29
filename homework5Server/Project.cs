using System;
using System.Collections.Generic;
using System.Text;

namespace homework5Server
{
	class Project
	{
		public int Id { get; set; }

		public string ProjectName { get; set; }
		public string ProjectType { get; set; }
		public string Description { get; set; }


		public List<Art> Arts { get; set; }
	}
}
