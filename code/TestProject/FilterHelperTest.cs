namespace TestProject;

using HellMapManager.Helpers;
using HellMapManager.Models;
public class FilterHelperTest
{
    [Fact]
    public void TestKeyword()
    {
        var keyword = new FilterKeyword
        {
            Type = FilterKeywordType.Key,
            Value = "abc",
            PartialMatch = false,
        };
        Assert.True(keyword.Match("abc", FilterKeywordType.Key));
        Assert.False(keyword.Match("abcd", FilterKeywordType.Key));
        Assert.False(keyword.Match("abc", FilterKeywordType.Type));
        keyword.Type = FilterKeywordType.Any;
        Assert.True(keyword.Match("abc", FilterKeywordType.Key));
        Assert.False(keyword.Match("abcd", FilterKeywordType.Key));
        keyword.PartialMatch = true;
        Assert.True(keyword.Match("abcd", FilterKeywordType.Key));
        keyword.Not = true;
        Assert.False(keyword.Match("abcd", FilterKeywordType.Key));
        keyword.PartialMatch = false;
        Assert.True(keyword.Match("abcd", FilterKeywordType.Key));
        Assert.False(keyword.Match("abc", FilterKeywordType.Key));
    }
    [Fact]
    public void TestFilter()
    {
        //分词
        var input = "abc";
        var result = FilterHelper.ParseKeywords(input);
        Assert.Equal(new List<string> { "abc" }, result.ConvertAll(x => x.Value));
        input = "";
        result = FilterHelper.ParseKeywords(input);
        Assert.Equal(new List<string> { }, result.ConvertAll(x => x.Value));
        input = " ";
        result = FilterHelper.ParseKeywords(input);
        Assert.Equal(new List<string> { }, result.ConvertAll(x => x.Value));
        input = "abc,def";
        result = FilterHelper.ParseKeywords(input);
        Assert.Equal(new List<string> { "abc", "def" }, result.ConvertAll(x => x.Value));
        input = "abc ,, , def,h ij ";
        result = FilterHelper.ParseKeywords(input);
        Assert.Equal(new List<string> { "abc", "def", "h ij" }, result.ConvertAll(x => x.Value));

        //转义
        input = @"abc\,def";
        result = FilterHelper.ParseKeywords(input);
        Assert.Equal(new List<string> { "abc,def" }, result.ConvertAll(x => x.Value));
        input = @"abc\ \,, ,\ def,h ij\ ";
        result = FilterHelper.ParseKeywords(input);
        Assert.Equal(new List<string> { "abc ,", " def", "h ij " }, result.ConvertAll(x => x.Value));
        input = @"\\\n\=\,\ \!";
        result = FilterHelper.ParseKeywords(input);
        Assert.Equal(new List<string> { "\\\n=, !" }, result.ConvertAll(x => x.Value));
        input = @"search";
        var keyword = FilterHelper.ParseKeyword(input);
        Assert.Equal(FilterKeywordType.Any, keyword.Type);
        Assert.Equal("search", keyword.Value);
        Assert.True(keyword.PartialMatch);
        Assert.False(keyword.Not);
        input = @"!name=example, notexists=!=value , search";
        result = FilterHelper.ParseKeywords(input);
        Assert.Equal(3, result.Count);
        keyword = result[0];
        Assert.Equal(FilterKeywordType.Name, keyword.Type);
        Assert.Equal("example", keyword.Value);
        Assert.False(keyword.PartialMatch);
        Assert.True(keyword.Not);
        keyword = result[1];
        Assert.Equal(FilterKeywordType.Wrong, keyword.Type);
        Assert.Equal("!=value", keyword.Value);
        Assert.False(keyword.PartialMatch);
        Assert.False(keyword.Not);
        keyword = result[2];
        Assert.Equal(FilterKeywordType.Any, keyword.Type);
        Assert.Equal("search", keyword.Value);
        Assert.True(keyword.PartialMatch);
        Assert.False(keyword.Not);

    }
}