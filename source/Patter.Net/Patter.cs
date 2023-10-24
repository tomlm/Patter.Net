using Patter.Operations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Patter
{
    public class Patter<T>
    {
        private List<PatternOp<T>> _operations = new List<PatternOp<T>>();

        public Patter()
        {
        }

        static Patter()
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

        // Start/EndCapture
        public Patter<T> StartCapture()
        {
            _operations.Add(new StartCapture<T>());
            return this;
        }

        public Patter<T> CaptureValue(Action<T, string, int, int> func, string endText, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _operations.Add(new CaptureValueToText<T>(func, endText, comparison));
            return this;
        }

        public Patter<T> CaptureValueToChars(Action<T, string, int, int> func, char[] chars)
        {
            _operations.Add(new CaptureValueToChars<T>(func, chars));
            return this;
        }

        public Patter<T> CaptureValueToChars(Action<T, string, int, int> func, string chars)
        {
            return CaptureValueToChars(func, chars.ToArray());
        }

        public Patter<T> EndCapture()
        {
            _operations.Add(new EndCapture<T>());
            return this;
        }

        // perform match
        public IEnumerable<T> Match(string text)
        {
            var context = new PatternContext<T>(text);

            while (context.Pos >= 0 && context.Pos < context.Text.Length)
            {
                foreach (var op in _operations)
                {
                    var result = op.Execute(context);
                    if (result != null)
                    {
                        yield return result;
                    }
                    if (context.Pos < 0)
                        break;
                }
            }
            yield break;
        }
    }
}
