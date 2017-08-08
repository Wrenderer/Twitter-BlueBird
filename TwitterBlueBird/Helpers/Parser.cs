using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TwitterBlueBird.Helpers
{
	public static class Parser
	{
		private static TwitterAPIContainer db = new TwitterAPIContainer();
		private static readonly String[] BLACKLIST = { "https", "the", "and", "or", "at", "rt", "to", "a", "in", "but", "is", "so", "as", "by" };
		private static readonly char[] WORD_SEPARATORS = { ' ', ',', '.', ':', '\t', '/', '-', '"' };
		public const String HAPPY = "Happy";
		public const String ANGRY = "Angry";
		public const String NEUTRAL = "Neither happy nor angry";

		public static void ParseTweet(int id, bool Happy)
		{
			Tweet tweet = db.Tweets.Find(id);
			if (tweet == null || tweet.Mood != null) return;

			tweet.Mood = (Happy)? HAPPY : ANGRY;

			using (var context = new TwitterAPIContainer())
			{
				context.Entry(tweet).State = EntityState.Modified;
				String[] words = tweet.Text.Trim().Split(WORD_SEPARATORS);

				foreach (String word in words)
				{
					Regex special_chars = new Regex(@"!|\.|\?|;|`|~");
					String cleaned_word = special_chars.Replace(word.ToLowerInvariant(),"").Trim();
					if (String.IsNullOrWhiteSpace(cleaned_word) || cleaned_word.ToCharArray()[0] == '@') continue;

					Word stored_word = context.Words.FirstOrDefault(w => w.Text == cleaned_word);

					if (stored_word == null && !BLACKLIST.Contains(cleaned_word))
					{
						int angry_count = (Happy) ? 0 : 1;
						int happy_count = (Happy) ? 1 : 0;

						context.Words.Add(new Word() { Text = cleaned_word, AngryCount = angry_count, HappyCount = happy_count });
					}
					else if (stored_word != null && !BLACKLIST.Contains(stored_word.Text))
					{
						if (Happy)
						{
							stored_word.HappyCount += 1;
						}
						else
						{
							stored_word.AngryCount += 1;
						}
						context.Entry(stored_word).State = EntityState.Modified;
					}
				}
				context.SaveChanges();
			}
		}

		public static string ParseMood(string tweet)
		{
			List<Word> matched_words = Scope.MatchedWords(tweet.Trim().ToLowerInvariant().Split(WORD_SEPARATORS).ToList());
			int happy_total = 0, angry_total = 0;

			foreach (Word matched_word in matched_words)
			{
				if (matched_word.HappyCount > matched_word.AngryCount)
				{
					happy_total++;
				}
				else if (matched_word.HappyCount < matched_word.AngryCount)
				{
					angry_total++;
				}
			}

			if (happy_total > angry_total)
			{
				return HAPPY;
			}
			else if (happy_total < angry_total)
			{
				return ANGRY;
			}

			return NEUTRAL;
		}
	}
}