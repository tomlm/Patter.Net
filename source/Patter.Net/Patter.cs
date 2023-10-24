using Patter.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("Patter.Tests")]

namespace Patter
{
    public class Patter<T>
    {
        private List<PatternOp<T>> _operations = new List<PatternOp<T>>();

        public Patter()
        {
        }


        /// <summary>
        /// Seek current position to the next instances of seekText
        /// </summary>
        /// <param name="seekText">text to seek for</param>
        /// <param name="comparisonType">case rules (Default is ignore case)</param>
        /// <returns>this</returns>
        public Patter<T> Seek(string seekText, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new SeekText<T>(seekText, comparisonType));
            return this;
        }

        /// <summary>
        /// Seek current position to the next instance of one of the characters
        /// </summary>
        /// <param name="chars">chars to search fore</param>
        /// <returns>this</returns>
        public Patter<T> SeekChars(char[] chars)
        {
            _operations.Add(new SeekChars<T>(chars));
            return this;
        }

        /// <summary>
        /// Seek current position to the next instance of one of the characters (in the string)
        /// </summary>
        /// <param name="chars">chars as a string</param>
        /// <returns>this</returns>
        public Patter<T> SeekChars(string chars)
        {
            return SeekChars(chars.ToArray());
        }


        /// <summary>
        /// Seek current position to the just past the instance of seekText 
        /// </summary>
        /// <param name="seekText">text to seek</param>
        /// <param name="comparisonType">string comparison (default is igonore case)</param>
        /// <returns>this</returns>
        public Patter<T> SeekPast(string seekText, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new SeekPast<T>(seekText, comparisonType));
            return this;
        }

        /// <summary>
        /// Seek current position to the the next instance of a character and then seek past that to first instance of not that character.
        /// </summary>
        /// <param name="chars">chars to search for</param>
        /// <returns>this</returns>
        public Patter<T> SeekPastChars(char[] chars)
        {
            _operations.Add(new SeekPastChars<T>(chars));
            return this;
        }

        /// <summary>
        /// Seek current position to first character past a set of characters.
        /// </summary>
        /// <param name="chars">chars as a string</param>
        /// <returns>this</returns>
        public Patter<T> SeekPastChars(string chars)
        {
            return SeekPastChars(chars.ToArray());
        }

        /// <summary>
        /// Move current position past text.
        /// </summary>
        /// <param name="skipText"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public Patter<T> Skip(string skipText, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new SkipText<T>(skipText, comparison));
            return this;
        }

        /// <summary>
        /// Move current position past chars
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public Patter<T> SkipChars(char[] chars)
        {
            _operations.Add(new SkipChars<T>(chars));
            return this;
        }

        /// <summary>
        /// Move current position past chars (as a string)
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public Patter<T> SkipChars(string chars)
        {
            return SkipChars(chars.ToArray());
        }

        /// <summary>
        /// Capture text while they match chars, call func() when done, where context.MatchText will have current match text
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public Patter<T> CaptureChars(char[] chars, Action<PatternContext<T>>? func = null)
        {
            _operations.Add(new CaptureChars<T>(chars, func));
            return this;
        }

        /// <summary>
        /// Capture text until you find endText, call func() when done, where context.MatchText will have current match text
        /// </summary>
        /// <param name="endText"></param>
        /// <param name="comparison"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public Patter<T> CaptureUntil(string endText, StringComparison comparison, Action<PatternContext<T>>? func = null)
        {
            _operations.Add(new CaptureToText<T>(endText, comparison, func));
            return this;
        }

        /// <summary>
        /// Capture text until you  find end text, call func() when done where context.MatchText will have current match text.
        /// </summary>
        /// <param name="endText"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public Patter<T> CaptureUntil(string endText, Action<PatternContext<T>>? func = null)
        {
            _operations.Add(new CaptureToText<T>(endText, StringComparison.OrdinalIgnoreCase, func));
            return this;
        }

        /// <summary>
        /// Capture chars until you get to one of the chars, then call func() when done where context.MatchText will have current match text.
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public Patter<T> CaptureUntilChars(char[] chars, Action<PatternContext<T>>? func = null)
        {
            _operations.Add(new CaptureToChars<T>(chars, func));
            return this;
        }

        /// <summary>
        /// Capture chars until you get to one of the chars in the string, then call func() when done where context.MatchText will have current match text.
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public Patter<T> CaptureUntilChars(string chars, Action<PatternContext<T>>? func = null)
        {
            return CaptureUntilChars(chars.ToArray(), func);
        }


        /// <summary>
        /// Custon action, call func(context) and context will have current position
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Patter<T> Custom(Action<PatternContext<T>> func)
        {
            _operations.Add(new Custom<T>(func));
            return this;
        }


        /// <summary>
        /// Search for matches of the pattern in text.
        /// </summary>
        /// <param name="text">text to search in</param>
        /// <returns>enumeration of objects</returns>
        public IEnumerable<T> Matches(string text)
        {
            var context = new PatternContext<T>(text);

            while (context.Pos >= 0 && context.Pos < context.Text.Length)
            {
                context.ResetMatch();

                foreach (var op in _operations)
                {
                    op.Execute(context);
                }

                if (context.HasMatch)
                    yield return context.Match!;
            }
            yield break;
        }
    }
}
