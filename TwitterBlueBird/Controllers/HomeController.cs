using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwitterBlueBird.Helpers;
using TwitterBlueBird.ViewModels;

namespace TwitterBlueBird.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			IndexViewModel vm = new IndexViewModel(TweetStream.GetTweets(),Scope.HappyWords(), Scope.AngryWords());
			return View(vm);
		}
	}
}