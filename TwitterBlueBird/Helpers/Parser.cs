﻿using System;
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
		private const String HAPPY = "Happy";
		private const String ANGRY = "Angry";

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
	}
}