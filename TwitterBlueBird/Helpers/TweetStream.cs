using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using LinqToTwitter;

namespace TwitterBlueBird.Helpers
{
	public class TweetStream
	{
		private const int tweetlimit = 5;

		public static List<Tweet> GetTweets()
		{
			List<Tweet> stored_tweets = Scope.UnratedTweets(tweetlimit);

			if (stored_tweets.Count == tweetlimit) return stored_tweets;

			var auth = new SingleUserAuthorizer
			{
				CredentialStore = new InMemoryCredentialStore()
				{
					ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
					ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"],
					OAuthToken = ConfigurationManager.AppSettings["accessToken"],
					OAuthTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"]
				}

			};
			var twitterCtx = new TwitterContext(auth);
			var newTweets = new List<Tweet>();
			int count = 0;
			var statusResponse = (from tweet in twitterCtx.Streaming
								  where tweet.Type == StreamingType.Sample
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
								  select tweet).StartAsync(async strm =>
								  {
									  /*
									   * Synchronous call to simplify demo. Would never do this in a production environment
									   * The better solution is to load all tweets, asyncronous to the view, to invisibly
									   * grab more if needed.
									   */
									  if (strm.EntityType.ToString() == "Status")
									  {
										  Status streamed_status = (Status)strm.Entity;
										  if (streamed_status.Lang == "en")
										  {
											  newTweets.Add(new Tweet() { Text = streamed_status.Text });
										  }
									  }

									  if (count++ >= 100)
									  {
										  strm.CloseStream();
									  }
								  });
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

			statusResponse.Wait();

			if (newTweets.Count > 0)
			{
				using (var ctx = new TwitterAPIContainer())
				{
					var test = ctx.Tweets.AddRange(newTweets);
					ctx.SaveChanges();
				}
			}
			else
			{
				throw new Exception("Communication or Rate Limit Error!");
			}

			return newTweets.Take(tweetlimit).ToList();
		}
	}
}