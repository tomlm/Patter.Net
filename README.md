# Patter.net
A simple pattern matching library for fluently extracting data from text.

# Rationale
I frequently find myself trying to pull a bit of structured data out of a stream of text.  Regex is an amazingly powerful tool, but I have
always struggled to get it to return structured results, and frequently just write my own little parser to extract the information I want. 
Patter is a simple library to describe seeking and grabbing the chunks of data you want without the complexity of Regex.

* Is it more powerful than Regex?  Absolutely not, if Regex is your jam, use Regex.
* Is it easier to read and get data out? IMHO yes.

# Extracting text

You can use Patter<string> to extract text.

For example:

```C#
var pattern = new Patter<string>()
    .SeekPast("<foo>")
    .CaptureUntil("</foo>");

var results = pattern.Matches("Show <foo>one</foo> and <foo>two</foo>");
```

returns

```json
["one","two"]
```



# Extracting complex objects

Let's say you want to extract anchors from a blob of text.  You want just the href and the link text. 

To do that you define the result object:
```csharp

public class ALink
{
   public string Text {get;set;}
   public Uri Url {get;set;}
}
```

And then define a pattern using Patter<ALink>:
```c#
    // define a patter to return enumeration of ALink objects.
    var pat = new Patter<ALink>()
        // seek to <a 
        .Seek("<a") 
        // seek past href attribute
        .SeekPast("href=") 
        // skip quotes if there any 
        .SkipChars(Chars.Quotes) 
        // Capture everything up to closing tag or end quote, and convert it a Uri and store in Alink.Url
        .CaptureUntilChars(">'\"", (context) => context.Match.Url = new Uri(context.MatchText))
        // skip quotes if there any 
        .SkipChars(Chars.Quotes)
        // seek past end of opening tag
        .SeekPast(">") 
        // capture everything up to the close </a tag and put it into the Alink.Text 
        .CaptureUntil("</a", (context) => context.Match.Text = context.MatchText.Trim()); 

    var matches = pat.Matches("this is a <a href=\"http://foo.com\">link1</a> <a href=http://bar.com>link2</a>").ToList();
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
| **SeekPast(text)**                     | Move the cursor to just past the next instance of *text*     |
| **SeekChars(chars)**                   | Move the cursor to next instance of one of the *chars*       |
| **SeekPastChars(chars)**               | Move the cursor to just first instance of set of chars and then to first instance of not the chars |
| **SkipChars(chars)**                   | Move the cursor to first char that is not in the set of chars |
| **CaptureChars(chars, func)**          | Capture chars while they are in the set of chars, call **func(context)** to give you ability to extra info from the **context.MatchText** and put into **context.Match** |
| **CaptureUntil(text, func)**           | Capture characters until text is found, then call **func(context)** to give you ability to extract info from the **context.MatchText** and put into the **context.Match** |
| **CaptureUntilPast(text, func)**       | Capture characters until text is found including text, then call **func(context)** to give you ability to extract info from the **context.MatchText** and put into the **context.Match** |
| **CaptureUntilChars(chars, func)**     | Capture characters until one of *chars* is found, call **func(context)** to give you ability to extract info from the **context.MatchText** and put into the **context.Match** |
| **CaptureUntilPastChars(chars, func)** | Capture characters until one of *chars* is found, including all chars, call **func(context)** to give you ability to extract info from **context.MatchText** and put into **context.Match** |
| **Custom(func)**                       | Let's you write a custom pattern operation, you are responsible for changing **context** properties directly (**Pos, MatchText, Match, HasMatch**) |

# PatternContext

The pattern context represents the current state of parser. It has the following properties of interest

| Properpty       | Description                                                  |
| --------------- | ------------------------------------------------------------ |
| **Pos**         | The current index into the string. It will be -1 when you are past the end of the string. |
| **Text**        | The text of the string that is being worked on               |
| **MatchText**   | The current matched text for a **CaptureXXX() ** method      |
| **HasMatch**    | Indicates that there is a match to be returned in the enumeration.  At the end of enumerating the operations if there is a HasMatch **context.Match** is yielded to the caller. |
| **Match**       | The object of type T that is yielded to the caller           |
| **CurrentChar** | Shortcut for the current char pos.  If it has Pos == -1 it will be ***(char)0*** |



# Technical notes

Patter patterns are 100% reusable and thread safe (meaning multiple threads can be evaluating a Patter pattern against strings safely). 