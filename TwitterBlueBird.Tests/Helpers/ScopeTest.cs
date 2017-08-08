using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterBlueBird.Helpers;

namespace TwitterBlueBird.Tests.Helpers
{
	[TestClass]
	public class ScopeTest
	{
		private static Random random = new Random();
		private const int unrated_tweet_count = 5;
		private const int happy_tweet_count = 3;
		private const int angry_tweet_count = 2;

		[ClassInitialize]
		public static void Setup(TestContext test_context)
		{
			using (var context = new TwitterAPIContainer())
			{
				context.Database.ExecuteSqlCommand("TRUNCATE TABLE Tweets");
				context.Database.ExecuteSqlCommand("TRUNCATE TABLE Words");

				for (int i = 0; i < unrated_tweet_count; i++)
				{
					context.Tweets.Add(new Tweet() { Text = RandomString(140), Mood = null });
					context.Words.Add(new Word() { Text = RandomString(6), HappyCount = 100 });
					context.Words.Add(new Word() { Text = RandomString(6), AngryCount = 100 });
				}

				for (int i = 0; i < happy_tweet_count; i++)
				{
					context.Tweets.Add(new Tweet() { Text = RandomString(140), Mood = Parser.HAPPY });
					context.Words.Add(new Word() { Text = RandomString(6), HappyCount = 100 });
					context.Words.Add(new Word() { Text = RandomString(6), AngryCount = 100 });
				}

				for (int i = 0; i < angry_tweet_count; i++)
				{
					context.Tweets.Add(new Tweet() { Text = RandomString(140), Mood = Parser.ANGRY });
					context.Words.Add(new Word() { Text = RandomString(6), HappyCount = 100 });
					context.Words.Add(new Word() { Text = RandomString(6), AngryCount = 100 });
				}

				context.SaveChanges();
			}
		}

		[ClassCleanup]
		public static void Teardown()
		{
			using (var context = new TwitterAPIContainer())
			{
				context.Database.ExecuteSqlCommand("TRUNCATE TABLE Tweets");
				context.Database.ExecuteSqlCommand("TRUNCATE TABLE Words");
			}
		}

		public static string RandomString(int length)
		{
			const string chars = " ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[random.Next(s.Length)]).ToArray());
		}

		[TestMethod]
		public void UnratedTweetsReturnsAllTweetsWithoutMoods()
		{
			int limit = 10;

			List<Tweet> tweets = Scope.UnratedTweets(limit);

			Assert.IsNotNull(tweets);
			Assert.IsTrue(tweets.Count == unrated_tweet_count);
			Assert.IsNull(tweets.Where(x => x.Mood != null).FirstOrDefault());
		}

		[TestMethod]
		public void AngryTweetsReturnsTweetsWithAngryMood()
		{
			int limit = 10;

			List<Tweet> tweets = Scope.AngryTweets(limit);

			Assert.IsNotNull(tweets);
			Assert.IsTrue(tweets.Count == angry_tweet_count);
			Assert.IsNull(tweets.Where(x => x.Mood != Parser.ANGRY).FirstOrDefault());
		}

		[TestMethod]
		public void HappyTweetsReturnsTweetsWithHappyMood()
		{
			int limit = 10;

			List<Tweet> tweets = Scope.HappyTweets(limit);

			Assert.IsNotNull(tweets);
			Assert.IsTrue(tweets.Count == happy_tweet_count);
			Assert.IsNull(tweets.Where(x => x.Mood != Parser.HAPPY).FirstOrDefault());
		}
	}
}
