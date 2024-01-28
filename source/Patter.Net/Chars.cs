namespace Patter
{
    public class Chars
    {
        /// <summary>
        /// Digits - 0..9
        /// </summary>
        public static char[] Digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// Letters
        /// </summary>
        public static char[] Letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        /// <summary>
        ///  Letters and digits
        /// </summary>
        public static char[] LettersOrDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        /// <summary>
        /// Single quotes
        /// </summary>
        public static char[] SingleQuote = new char[] { '\'', '`' };

        /// <summary>
        /// Double Quotes
        /// </summary>
        public static char[] DoubleQuote = new char[] { '"' };

        /// <summary>
        /// Double and Single quotes
        /// </summary>
        public static char[] Quotes = new char[] { '"', '\'', '`' };

        /// <summary>
        /// Tab char
        /// </summary>
        public static char[] Tab = new char[] { '\t' };

        /// <summary>
        /// Whitespace chars (space, tab, eol)
        /// </summary>
        public static char[] Whitespace = new char[] {
            ' ', '\t', '\r', '\n', '\f','\v',
            (char)0x00a0, (char)0x1680, (char)0x2000, (char)0x2001, (char)0x2002,
            (char)0x2003, (char)0x2004, (char)0x2005, (char)0x2006, (char)0x2007,
            (char)0x2008, (char)0x2009, (char)0x200a, (char)0x2028, (char)0x2029,
            (char)0x202f, (char)0x205f, (char)0x3000, (char)0xfeff
        };

        /// <summary>
        /// End of line
        /// </summary>
        public static char[] EOL = new char[] { '\r', '\n' };
    }
}
