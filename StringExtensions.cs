public static class StringExtensions 
{
    // The 'this string str' marks this as an extension method for 'string'
    public static int WordCount(this string str) 
    {
        return str.Split(new char[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

// How you call it:
string sentence = "C# extension methods are powerful";
int count = sentence.WordCount(); // Looks like a native instance method!
