using Patter.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleToAttribute("Patter.Tests")]

namespace Patter
{
    public class Pattern<T>
    {
        private List<PatternOp<T>> _operations;

        internal Pattern(List<PatternOp<T>> operations)
        {
            _operations = operations;
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
                    yield return context.Match;
            }
            yield break;
        }
    }
}
