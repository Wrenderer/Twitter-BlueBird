using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwitterBlueBird.Helpers
{
	public static class Scope
	{
		public static List<Tweet> UnratedTweets(int limit = 5, string mood = null)
		{
			using (var context = new TwitterAPIContainer())
			{
				return (from t in context.Tweets
						where t.Mood == mood
						select t)
						.OrderBy(x => Guid.NewGuid())
						.Take(limit)
						.ToList();
			}
		}

		public static List<Tweet> HappyTweets(int limit = 1)
		{
			return UnratedTweets(limit, "Happy");
		}

		public static List<Tweet> AngryTweets(int limit = 1)
		{
			return UnratedTweets(limit, "Angry");
		}
	}
}