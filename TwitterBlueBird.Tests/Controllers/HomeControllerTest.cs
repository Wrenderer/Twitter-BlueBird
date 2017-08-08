using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwitterBlueBird;
using TwitterBlueBird.Controllers;

namespace TwitterBlueBird.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		private static Random random = new Random();

		[ClassInitialize]
		public static void Setup(TestContext test_context)
		{
			using (var context = new TwitterAPIContainer())
			{
				context.Database.ExecuteSqlCommand("TRUNCATE TABLE Tweets");
				context.Database.ExecuteSqlCommand("TRUNCATE TABLE Words");

				for (int i = 0; i < 10; i++)
				{
					context.Tweets.Add(new Tweet() { Text = RandomString(140), Mood = null });
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
		public void Index()
		{
			HomeController controller = new HomeController();
			ViewResult result = controller.Index() as ViewResult;
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void IndexRetrievesFiveRandomTweets()
		{
			HomeController controller = new HomeController();
			ViewResult result = controller.Index() as ViewResult;

			ViewModels.IndexViewModel view_model = (ViewModels.IndexViewModel) result.Model;

			Assert.IsNotNull(view_model.RatableTweets);
			Assert.IsTrue(view_model.RatableTweets.Count == 5);
			Assert.IsNotNull(view_model.RatableTweets.First().Text);
		}

		[TestMethod]
		public void IndexRetrievesTopTenHappyTweets()
		{
			HomeController controller = new HomeController();
			ViewResult result = controller.Index() as ViewResult;

			ViewModels.IndexViewModel view_model = (ViewModels.IndexViewModel)result.Model;

			Assert.IsNotNull(view_model.TopHappyWords);
			Assert.IsTrue(view_model.TopHappyWords.Count == 10);
			Assert.IsNotNull(view_model.TopHappyWords.First().Text);
		}

		[TestMethod]
		public void IndexRetrievesTopTenAngryTweets()
		{
			HomeController controller = new HomeController();
			ViewResult result = controller.Index() as ViewResult;

			ViewModels.IndexViewModel view_model = (ViewModels.IndexViewModel)result.Model;

			Assert.IsNotNull(view_model.TopAngryWords);
			Assert.IsTrue(view_model.TopAngryWords.Count == 10);
			Assert.IsNotNull(view_model.TopAngryWords.First().Text);
		}
	}
}
