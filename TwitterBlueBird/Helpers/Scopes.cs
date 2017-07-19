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

		public static List<Word> HappyWords(int limit = 10)
		{
			using (var context = new TwitterAPIContainer())
			{
				return (from t in context.Words
						where t.HappyCount > t.AngryCount
						select t)
						.OrderBy(t => t.HappyCount)
						.Take(limit)
						.ToList();
			}
		}

		public static List<Word> AngryWords(int limit = 10)
		{
			using (var context = new TwitterAPIContainer())
			{
				return (from t in context.Words
						where t.HappyCount < t.AngryCount
						select t)
						.OrderBy(t => t.AngryCount)
						.Take(limit)
						.ToList();
			}
		}
	}
}