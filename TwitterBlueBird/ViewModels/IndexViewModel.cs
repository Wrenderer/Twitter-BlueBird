using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterBlueBird.ViewModels
{
	public class IndexViewModel
	{
		public List<Tweet> RatableTweets { get; set; }
		public List<Word> TopHappyWords { get; set; }
		public List<Word> TopAngryWords { get; set; }

		public IndexViewModel(List<Tweet> t, List<Word> h, List<Word> a)
		{
			RatableTweets = t;
			TopHappyWords = h;
			TopAngryWords = a;
		}
	}
}