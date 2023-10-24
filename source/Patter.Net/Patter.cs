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


        // SKIP 
        public Patter<T> Seek(string seekText, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new SeekText<T>(seekText, comparisonType));
            return this;
        }

        public Patter<T> SeekChars(char[] chars)
        {
            _operations.Add(new SeekChars<T>(chars));
            return this;
        }

        public Patter<T> SeekChars(string chars)
        {
            return SeekChars(chars.ToArray());
        }


        public Patter<T> SeekAndSkip(string seekText, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new SeekAndSkipText<T>(seekText, comparisonType));
            return this;
        }

        public Patter<T> SeekAndSkipChars(char[] chars)
        {
            _operations.Add(new SeekAndSkipChars<T>(chars));
            return this;
        }


        public Patter<T> SeekAndSkipChars(string chars)
        {
            return SeekAndSkipChars(chars.ToArray());
        }

        // Skip
        public Patter<T> Skip(string skipText, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new SkipText<T>(skipText, comparison));
            return this;
        }

        public Patter<T> SkipChars(char[] chars)
        {
            _operations.Add(new SkipChars<T>(chars));
            return this;
        }

        public Patter<T> SkipChars(string chars)
        {
            return SkipChars(chars.ToArray());
        }

        public Patter<T> CaptureChars(char[] chars, Action<PatternContext<T>> func = null)
        {
            _operations.Add(new CaptureChars<T>(chars, func));
            return this;
        }

        public Patter<T> CaptureUntil(string endText, StringComparison comparison, Action<PatternContext<T>> func = null)
        {
            _operations.Add(new CaptureToText<T>(endText, comparison, func));
            return this;
        }

        public Patter<T> CaptureUntil(string endText, Action<PatternContext<T>> func = null)
        {
            _operations.Add(new CaptureToText<T>(endText, StringComparison.OrdinalIgnoreCase, func));
            return this;
        }

        public Patter<T> CaptureUntilChars(char[] chars, Action<PatternContext<T>> func = null)
        {
            _operations.Add(new CaptureToChars<T>(chars, func));
            return this;
        }

        public Patter<T> CaptureToChars(string chars, Action<PatternContext<T>> func = null)
        {
            return CaptureUntilChars(chars.ToArray(), func);
        }

        // perform match
        public IEnumerable<T> Match(string text)
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
