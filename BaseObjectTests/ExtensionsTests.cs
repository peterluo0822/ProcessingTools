﻿namespace ProcessingTools.Base
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void MatchWithStringList_Test()
        {
            string[] words = new string[] { "a", "accommodated", "after", "all", "also", "altered", "an", "and", "article", "at", "attendance", "attracting", "August", "becoming", "been", "before", "built", "by", "capacity", "Carrow", "City", "city", "club", "Club", "concerts", "crowd", "crowds", "Crystal", "Cup", "current", "days", "deemed", "destroyed", "devastating", "during", "east", "Elton", "England", "FA", "far", "fire", "football", "Football", "football", "for", "from" };

            List<string> wordList = words.ToList();

            string[] matches = new string[] { "east", "football" };

            List<string> matchesList = matches.ToList();

            {
                IEnumerable<string> matchedValuesCaseInsensitive = wordList.MatchWithStringList(matchesList, false, false);

                Assert.AreEqual(
                    "east football Football football",
                    String.Join(" ", matchedValuesCaseInsensitive),
                    "CaseInsensitive non-regex match failed.");
            }

            {
                IEnumerable<string> matchedValuesCaseSesitive = wordList.MatchWithStringList(matchesList, false, true);

                Assert.AreEqual(
                    "east football football",
                    String.Join(" ", matchedValuesCaseSesitive),
                    "CaseSensitive non-regex match failed.");
            }

            {
                IEnumerable<string> matchedValuesCaseInsensitive = wordList.MatchWithStringList(matchesList, true, false);

                Assert.AreEqual(
                    "east football Football football",
                    String.Join(" ", matchedValuesCaseInsensitive),
                    "CaseInsensitive regex match failed.");
            }

            {
                IEnumerable<string> matchedValuesCaseSesitive = wordList.MatchWithStringList(matchesList, true, true);

                Assert.AreEqual(
                    "east football football",
                    String.Join(" ", matchedValuesCaseSesitive),
                    "CaseSensitive regex match failed.");
            }
        }

        [TestMethod]
        public void DistinctWithStringList_Test()
        {
            string[] words = new string[] { "a", "accommodated", "after", "all", "also", "altered", "an", "and", "article", "at", "attendance", "attracting", "August", "becoming", "been", "before", "built", "by", "capacity", "Carrow", "City", "city", "club", "Club", "concerts", "crowd", "crowds", "Crystal", "Cup", "current", "days", "deemed", "destroyed", "devastating", "during", "east", "Elton", "England", "FA", "far", "fire", "football", "Football", "football", "for", "from" };

            List<string> wordList = words.ToList();

            string[] matches = new string[] { "a", "accommodated", "after", "all", "also", "altered", "an", "and", "article", "at", "attendance", "attracting", "August", "becoming", "been", "before", "built", "by", "capacity", "Carrow", "club", "Club", "concerts", "crowd", "crowds", "Crystal", "Cup", "current", "days", "deemed", "destroyed", "devastating", "during", "east", "Elton", "England"};

            List<string> matchesList = matches.ToList();

            {
                IEnumerable<string> matchedValuesCaseInsensitive = wordList.DistinctWithStringList(matchesList, false, false);

                Assert.AreEqual(
                    "City city fire for from",
                    String.Join(" ", matchedValuesCaseInsensitive),
                    "CaseInsensitive non-regex distinct failed.");
            }

            {
                IEnumerable<string> matchedValuesCaseSesitive = wordList.DistinctWithStringList(matchesList, false, true);

                Assert.AreEqual(
                    "City city FA fire for from",
                    String.Join(" ", matchedValuesCaseSesitive),
                    "CaseSensitive non-regex distinct failed.");
            }

            {
                IEnumerable<string> matchedValuesCaseInsensitive = wordList.DistinctWithStringList(matchesList, true, false);

                Assert.AreEqual(
                    "City city FA far fire football Football football for from",
                    String.Join(" ", matchedValuesCaseInsensitive),
                    "CaseInsensitive regex distinct failed.");
            }

            {
                IEnumerable<string> matchedValuesCaseSesitive = wordList.DistinctWithStringList(matchesList, true, true);

                Assert.AreEqual(
                    "City city FA far fire football Football football for from",
                    String.Join(" ", matchedValuesCaseSesitive),
                    "CaseSensitive regex distinct failed.");
            }
        }
    }
}
