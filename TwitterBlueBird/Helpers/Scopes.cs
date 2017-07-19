using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterBlueBird.Helpers
{
	public static class Scope
	{
		public static List<Tweet> UnratedTweets(int limit = 5)
		{
			using (var context = new TwitterAPIContainer())
			{
				return (from t in context.Tweets
						where t.Mood == null
						select t)
						.OrderBy(x => Guid.NewGuid())
						.Take(limit)
						.ToList();
			}
		}
	}
}