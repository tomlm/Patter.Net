using Patter.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("Patter.Tests")]

namespace Patter
{
    public class PatternBuilder<T>
    {
        private List<PatternOp<T>> _operations = new List<PatternOp<T>>();

        public PatternBuilder()
        {
        }


        /// <summary>
        /// Seek current position to the next instances of seekText
        /// </summary>
        /// <param name="seekText">text to seek for</param>
        /// <param name="comparisonType">case rules (Default is ignore case)</param>
        /// <returns>this</returns>
        public PatternBuilder<T> Seek(string seekText, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new SeekText<T>(seekText, comparisonType));
            return this;
        }

        /// <summary>
        /// Seek current position to the next instance of one of the characters
        /// </summary>
        /// <param name="chars">chars to search fore</param>
        /// <returns>this</returns>
        public PatternBuilder<T> Seek(params char[] chars)
        {
            _operations.Add(new SeekChars<T>(chars));
            return this;
        }

        /// <summary>
        /// Seek current position to the just past the instance of seekText 
        /// </summary>
        /// <param name="seekText">text to seek</param>
        /// <param name="comparisonType">string comparison (default is igonore case)</param>
        /// <returns>this</returns>
        public PatternBuilder<T> SeekPast(string seekText, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new SeekText<T>(seekText, comparisonType));
            _operations.Add(new SkipText<T>(seekText, comparisonType));
            return this;
        }

        /// <summary>
        /// Seek current position to the the next instance of a character and then seek past that to first instance of not that character.
        /// </summary>
        /// <param name="chars">chars to search for</param>
        /// <returns>this</returns>
        public PatternBuilder<T> SeekPast(params char[] chars)
        {
            _operations.Add(new SeekChars<T>(chars));
            _operations.Add(new SkipChars<T>(chars));
            return this;
        }

        /// <summary>
        /// Move current position past text.
        /// </summary>
        /// <param name="skipText"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public PatternBuilder<T> Skip(string skipText, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new SkipText<T>(skipText, comparison));
            return this;
        }

        /// <summary>
        /// Move current position past chars
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public PatternBuilder<T> Skip(params char[] chars)
        {
            _operations.Add(new SkipChars<T>(chars));
            return this;
        }

        /// <summary>
        /// Capture text while they match chars, call func() when done, where context.MatchText will have current match text
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public PatternBuilder<T> Capture(char[] chars, Action<PatternContext<T>> func = null)
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
        public PatternBuilder<T> CaptureUntil(string endText, StringComparison comparison, Action<PatternContext<T>> func = null)
        {
            _operations.Add(new CaptureToText<T>(endText, comparison, skipPast: false, func));
            return this;
        }

        /// <summary>
        /// Capture text until you  find end text, call func() when done where context.MatchText will have current match text.
        /// </summary>
        /// <param name="endText"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public PatternBuilder<T> CaptureUntil(string endText, Action<PatternContext<T>> func = null)
        {
            _operations.Add(new CaptureToText<T>(endText, StringComparison.OrdinalIgnoreCase, skipPast: false, func));
            return this;
        }

        /// <summary>
        /// Capture chars until you get to one of the chars, then call func() when done where context.MatchText will have current match text.
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public PatternBuilder<T> CaptureUntil(char[] chars, Action<PatternContext<T>> func = null)
        {
            _operations.Add(new CaptureToChars<T>(chars, skipPast: false, func));
            return this;
        }

        /// <summary>
        /// Capture text until you find end text, skip past the text
        /// then call func() when done where context.MatchText will have current match text.
        /// </summary>
        /// <param name="endText"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public PatternBuilder<T> CaptureUntilPast(string endText, Action<PatternContext<T>> func = null)
        {
            _operations.Add(new CaptureToText<T>(endText, StringComparison.OrdinalIgnoreCase, skipPast: true, func));
            return this;
        }

        /// <summary>
        /// Capture chars until you get to one of the chars, then past those chars
        /// then call func() when done where context.MatchText will have current match text.
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public PatternBuilder<T> CaptureUntilPast(char[] chars, Action<PatternContext<T>> func = null)
        {
            _operations.Add(new CaptureToChars<T>(chars, skipPast: true, func));
            return this;
        }


        /// <summary>
        /// call func(context) and context will have current position
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public PatternBuilder<T> Custom(Action<PatternContext<T>> func)
        {
            _operations.Add(new Call<T>(func));
            return this;
        }


        /// <summary>
        /// call func(context) and context will have current position
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public PatternBuilder<T> Call(Action<PatternContext<T>> func)
        {
            _operations.Add(new Call<T>(func));
            return this;
        }


        public Pattern<T> Build()
        {
            return new Pattern<T>(_operations);
        }
    }
}
