using System;
using System.Collections.Generic;
using System.Text;

namespace homework5Server
{
	class Post
	{
		public int Id { get; set; }

		public string TitlePost { get; set; }
		public string TextContentPost { get; set; }
		/*хештеги*/

		public DateTime DateTimePostPublished { get; set; }

		public List<Art> Arts { get; set; }

	}
}
