using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using TwitterBlueBird.Helpers;

namespace TwitterBlueBird
{
	[ServiceContract(Namespace = "TwitterService")]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class Twitter
	{
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json)]
		public List<String> GetTweets(int amount)
		{
			List<Tweet> random_tweets = Scope.UnratedTweets(amount);
			return random_tweets.ConvertAll(x => x.Text);
		}

		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json)]
		public Tuple<string, string> GetMoodTweet()
		{
			string tweet = GetTweets(1).FirstOrDefault();

			return new Tuple<string, string>(tweet, Parser.ParseMood(tweet));
		}

	}
}
