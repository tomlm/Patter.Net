# Patter.net
A simple pattern matching library for fluently extracting patterned data from text.

# Rationale
I frequently find myself trying to pull a bit of structured data out of a stream of text.  Regex is an amazingly powerful tool, but I have
always struggled to get it to return structured results, and frequently just end up writing my own little parser to extract the information I want. 
Patter is a simple library to describe seeking and grabbing the chunks of data you want without the complexity of Regex.

* Is it more powerful than Regex?  Absolutely not, if Regex is your jam, use Regex.
* Is it easier to read and get data out? In many cases (and in my honest opinion) yes.

# Extracting text
You use PatterBuilder<string> to define a Pattern which knows how to extract data from a string.

For example:

```C#
var pattern = new PatternBuilder<string>()
    .SeekPast("<foo>")
    .CaptureUntil("</foo>")
    .Build();

var results = pattern.Matches("Show <foo>one</foo> and <foo>two</foo>");
```

returns a list of strings, 

```json
["one","two"]
```


# Extracting complex objects
Let's say you want to extract anchors from a blob of textinto an object (Alink):
```csharp

public class ALink
{
   public string Text {get;set;}
   public Uri Url {get;set;}
}
```

And then define a pattern using PatternBuilder with ALink as the result type:
```c#
    // define a patter to return enumeration of ALink objects.
    var pattern = new PatternBuilder<ALink>()
        // seek to <a 
        .Seek("<a") 
        // seek past href attribute
        .SeekPast("href=") 
        // skip quotes if there any 
        .Skip(Chars.Quotes) 
        // Capture everything up to closing tag or end quote, and convert it a Uri and store in Alink.Url
        .CaptureUntil(">'\"".ToArray(), (context) => context.Match.Url = new Uri(context.MatchText))
        // skip quotes if there any 
        .Skip(Chars.Quotes)
        // seek past end of opening tag
        .SeekPast(">") 
        // capture everything up to the close </a tag and put it into the Alink.Text 
        .CaptureUntil("</a", (context) => context.Match.Text = context.MatchText.Trim())
        .Build(); 

    var matches = pattern.Matches("this is a <a href=\"http://foo.com\">link1</a> <a href=http://bar.com>link2</a>").ToList();
    Debug.WriteLine(JsonConvert.SeriializeObject(matches));
```

This will extract the text and urls from the tags.  It's an enumerable, so you can use LINQ statements to further manipulate the results.
```json
[
    { 
          "Text":"link1"
          "Url":"http://foo.com"
    },
    { 
          "Text":"link2"
          "Url":"http://bar.com"
    }
]
```


# Methods

| Method                                 | Description                                                  |
| -------------------------------------- | ------------------------------------------------------------ |
| **Seek(text)**                         | Move the cursor to next instance of *text*                   |
| **Seek(char[])**                       | Move the cursor to next instance of one of the *chars*       |
| **SeekPast(text)**                     | Move the cursor to just past the next instance of *text*     |
| **SeekPast(char[])**                   | Move the cursor to just first instance of set of chars and then to first instance of not the chars |
| **Skip(char[])**                       | Move the cursor to first char that is not in the set of chars |
| **Capture(char[], func)**              | Capture chars while they are in the set of chars, call **func(context)** to give you ability to extra info from the **context.MatchText** and put into **context.Match** |
| **CaptureUntil(text, func)**           | Capture characters until text is found, then call **func(context)** to give you ability to extract info from the **context.MatchText** and put into the **context.Match** |
| **CaptureUntil(char[], func)**         | Capture characters until one of *chars* is found, call **func(context)** to give you ability to extract info from the **context.MatchText** and put into the **context.Match** |
| **CaptureUntilPast(text, func)**       | Capture characters until text is found including text, then call **func(context)** to give you ability to extract info from the **context.MatchText** and put into the **context.Match** |
| **CaptureUntilPast(char[], func)**     | Capture characters until one of *chars* is found, including all chars, call **func(context)** to give you ability to extract info from **context.MatchText** and put into **context.Match** |
| **Capture(func)**                      | Let's you write a custom pattern operation, you are responsible for changing **context** properties directly (**Pos, MatchText, Match, HasMatch**) |

# PatternContext

The ```PatternContext``` object represents the current state of parser and is passed to It has the following properties of interest

| Property        | Description                                                  |
| --------------- | ------------------------------------------------------------ |
| **Pos**         | The current index into the string. It will be -1 when you are past the end of the string. |
| **Text**        | The full text of the string that is being worked on               |
| **MatchText**   | The current matched text for a **CaptureXXX() ** method      |
| **HasMatch**    | Indicates that there is a match to be returned in the enumeration.  At the end of enumerating the operations if there is a HasMatch **context.Match** is yielded to the caller. |
| **Match**       | The object of type T that is yielded to the caller. You modify this object to build up the object that is yielded to the caller as a match.           |
| **CurrentChar** | Shortcut for the current char value for the current Pos.  If it has Pos == -1 it will be ***(char)0*** |


# Chars
The Chars class defines classes of useful characters for matching:
| Name            | Description |
| --------------- | ------------|
| Digits          | Digits - 0..9 |
| Letters         | Alphabetical ascii letters |
| LettersOrDigits | Digits and Letters combined|
| Quotes          | Single and Double quotes |
| SingleQuote     | Single Quotes |
| DoubleQuote     | Double Quotes |
| Whitespace      | Whitespace chars (tab, space, EOL, etc.) |
| EOL             | End of line chars (\r, \n) |

Example:
```C#
  var pattern = new PatternBuilder<string>()
     .SeekPast("Name:")
     .Skip(Chars.Whitespace)
     .Capture(Chars.LettersOrDigits)
     .SkipPast(Chars.EOL)
     .Build();
```

# Technical notes
Patterns are 100% reusable and thread safe (meaning multiple threads can be evaluating a Patter pattern against strings safely). 

# Changes from 1.x
Version was bumped to major 2.x for semantic versioning rules, aka it has breaking changes which clean up the usage around character matching methods.
* Switched to PatternBuilder().Build() => Pattern(), which makes it clearer when you are defining the pattern versus using the pattern. Only Pattern(T)() has Matches() method.
* functions were simplified to simply using char[] as the signature to know it's character based pattern, renaming methods like SeekChars() => Seek(char[] )
* char[] methods as appropriate use ```params``` nomenclature, so you can write ```.Skip('x','y','z')```




