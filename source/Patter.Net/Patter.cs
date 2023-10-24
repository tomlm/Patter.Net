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

        public Patter<T> SeekChars(params char[] chars)
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
        public Patter<T> Skip(string skipText, StringComparer comparer = null)
        {
            _operations.Add(new SkipText<T>(skipText, comparer));
            return this;
        }

        public Patter<T> SkipChars(params char[] chars)
        {
            _operations.Add(new SkipChars<T>(chars));
            return this;
        }

        public Patter<T> SkipChars(string chars)
        {
            return SkipChars(chars.ToArray());
        }

        //// Start/EndCapture
        //public Patter<T> StartCapture()
        //{
        //    _operations.Add(new StartCapture<T>());
        //    return this;
        //}

        public Patter<T> CaptureValue(Action<PatternContext<T>, string> func, string endText, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new CaptureValueToText<T>(func, endText, comparison));
            return this;
        }

        public Patter<T> CaptureValueToChars(Action<PatternContext<T>, string> func, char[] chars)
        {
            _operations.Add(new CaptureValueToChars<T>(func, chars));
            return this;
        }

        public Patter<T> CaptureValueToChars(Action<PatternContext<T>, string> func, string chars)
        {
            return CaptureValueToChars(func, chars.ToArray());
        }

        //public Patter<T> EndCapture()
        //{
        //    _operations.Add(new EndCapture<T>());
        //    return this;
        //}

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
                    if (context.Pos < 0)
                    {
                        context.HasMatch = false;
                        break;
                    }
                }

                if (context.HasMatch)
                    yield return context.CurrentMatch!;
            }
            yield break;
        }
    }
}
