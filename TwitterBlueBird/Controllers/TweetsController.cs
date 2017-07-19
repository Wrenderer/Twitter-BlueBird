using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TwitterBlueBird;
using TwitterBlueBird.Helpers;

namespace TwitterBlueBird.Controllers
{
    public class TweetsController : Controller
    {
        private TwitterAPIContainer db = new TwitterAPIContainer();

        // GET: Tweets
        public ActionResult Index()
        {
			return View(Scope.UnratedTweets());
        }

        // GET: Tweets/Details/:ID
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tweet tweet = db.Tweets.Find(id);
            if (tweet == null)
            {
                return HttpNotFound();
            }
            return View(tweet);
        }

		// Ajax POST: Tweets/RateAngry/:ID
		[HttpPost]
		public ActionResult RateHappy(int? id)
		{
			if (id != null)
			{
				Tweet tweet = db.Tweets.Find(id);
				if (tweet != null && tweet.Mood == null)
				{
					tweet.Mood = "Happy";
					using (var context = new TwitterAPIContainer())
					{
						context.Entry(tweet).State = EntityState.Modified;
						context.SaveChanges();
					}
				}
			}
			return Content(id.ToString());
		}

		// Ajax POST: Tweets/RateAngry/:ID
		[HttpPost]
		public ActionResult RateAngry(int? id)
		{
			if (id != null)
			{
				Tweet tweet = db.Tweets.Find(id);
				if (tweet != null && tweet.Mood == null)
				{
					tweet.Mood = "Angry";
					using (var context = new TwitterAPIContainer())
					{
						context.Entry(tweet).State = EntityState.Modified;
						context.SaveChanges();
					}
				}
			}
			return Content(id.ToString());
		}

		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
