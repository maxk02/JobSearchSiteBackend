using System.Text.RegularExpressions;

namespace Shared.FluentValidationAddons.StringFiltering;

public class WhitelistPolicy : CustomStringPolicy
{
    private string _charactersOutOfWhitelists = "";
    public HashSet<char> NonWhitelistedCharsFound => _charactersOutOfWhitelists.ToCharArray().ToHashSet();
    public HashSet<string> AllowedCharTypes { get; private set; } = [];

    //https://www.regular-expressions.info/unicode.html
    
    private void ExecuteAllowLetters()
    {
        _charactersOutOfWhitelists = Regex.Replace(_charactersOutOfWhitelists, @"[\p{L}\p{M}]", "");
    }
    public WhitelistPolicy Letters()
    {
        MethodCalls.Add(ExecuteAllowLetters);
        AllowedCharTypes.Add("letters");

        return this;
    }

    private void ExecuteAllowDigits()
    {
        _charactersOutOfWhitelists = Regex.Replace(_charactersOutOfWhitelists, @"\d", "");
    }
    public WhitelistPolicy Digits()
    {
        MethodCalls.Add(ExecuteAllowDigits);
        AllowedCharTypes.Add("digits");

        return this;
    }


    private void ExecuteAllowPunctuation()
    {
        _charactersOutOfWhitelists = Regex.Replace(_charactersOutOfWhitelists, @"\p{P}", "");
    }
    public WhitelistPolicy Punctuation()
    {
        MethodCalls.Add(ExecuteAllowPunctuation);
        AllowedCharTypes.Add("punctuation chars");

        return this;
    }


    private void ExecuteAllowSymbols()
    {
        _charactersOutOfWhitelists = Regex.Replace(_charactersOutOfWhitelists, @"\p{S}", "");
    }
    public WhitelistPolicy Symbols()
    {
        MethodCalls.Add(ExecuteAllowSymbols);
        AllowedCharTypes.Add("symbols");

        return this;
    }


    private void ExecuteAllowSpaces()
    {
        _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace(" ", "");
    }
    public WhitelistPolicy Spaces()
    {
        MethodCalls.Add(ExecuteAllowSpaces);
        AllowedCharTypes.Add("spaces");

        return this;
    }


    private void ExecuteAllowTabs()
    {
        _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace("\t", "");
    }
    public WhitelistPolicy Tabs()
    {
        MethodCalls.Add(ExecuteAllowTabs);
        AllowedCharTypes.Add("tabs");

        return this;
    }


    private void ExecuteAllowNewlines()
    {
        _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace("\n", "");
    }
    public WhitelistPolicy Newlines()
    {
        MethodCalls.Add(ExecuteAllowNewlines);
        AllowedCharTypes.Add("newlines");

        return this;
    }


    private void ExecuteAllowCustomChars(IEnumerable<char> customChars)
    {
        foreach (var x in customChars)
        {
            _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace(x.ToString(), "");
        }
    }
    public WhitelistPolicy CustomChars(IEnumerable<char> customChars)
    {
        MethodCalls.Add(() => ExecuteAllowCustomChars(customChars));

        foreach (var x in customChars)
        {
            AllowedCharTypes.Add(x.ToString());
        }

        return this;
    }
    

    protected override void RunPreValidationsOperations()
    {
        if (Value is null)
            throw ValueNullException;

        _charactersOutOfWhitelists = Value;
    }
}