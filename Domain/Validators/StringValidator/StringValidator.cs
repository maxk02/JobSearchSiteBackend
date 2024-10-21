using System.Text.RegularExpressions;
using Domain.Errors.Validation;
using Domain.Shared.Errors;

namespace Domain.Validators.StringValidator;

public class StringValidator : ValidatorBase<string?>
{
    private string _charactersOutOfWhitelists;
    private readonly HashSet<string> _blacklistedCharsFound = [];

    private readonly HashSet<string> _whitelistedCharsDefinitionSet = [];
    private readonly HashSet<string> _blacklistedCharsDefinitionSet = [];
    
    public StringValidator(string? value, string className, string propertyName) : base(value, className, propertyName)
    {
        _charactersOutOfWhitelists = Value ?? "";
    }

    private void _handleBlacklistViolations()
    {
        if (_blacklistedCharsFound.Count == 0)
            return;
        
        _errors.Add(new ValidationErrors.Strings.BlacklistError(_className,
            _propertyName, _blacklistedCharsFound, _blacklistedCharsDefinitionSet));
    }
    
    private void _handleWhitelistViolations()
    {
        if (_charactersOutOfWhitelists.Length == 0)
            return;
        
        var nonWhitelistedCharsFound = new HashSet<string>();

        if (_charactersOutOfWhitelists.Contains('\t'))
        {
            nonWhitelistedCharsFound.Add("tab");
            _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace("\t", "");
        }
        if (_charactersOutOfWhitelists.Contains(' '))
        {
            nonWhitelistedCharsFound.Add("space");
            _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace(" ", "");
        }
        if (_charactersOutOfWhitelists.Contains('\n'))
        {
            nonWhitelistedCharsFound.Add("newline");
            _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace("\n", "");
        }

        foreach (var x in _charactersOutOfWhitelists)
        {
            nonWhitelistedCharsFound.Add(x.ToString());
        }
        
        _errors.Add(new ValidationErrors.Strings.WhitelistError(_className,
            _propertyName, nonWhitelistedCharsFound, _whitelistedCharsDefinitionSet));
    }
    
    //https://www.regular-expressions.info/unicode.html
    
    public StringValidator AllowLetters()
    {
        if (string.IsNullOrEmpty(Value))
            return this;
        
        _whitelistedCharsDefinitionSet.Add("letters");

        _charactersOutOfWhitelists = Regex.Replace(_charactersOutOfWhitelists, @"[\p{L}\p{M}]", "");
        
        return this;
    }
    
    public StringValidator AllowDigits()
    {
        if (string.IsNullOrEmpty(Value))
            return this;
        
        _whitelistedCharsDefinitionSet.Add("digits");

        _charactersOutOfWhitelists = Regex.Replace(_charactersOutOfWhitelists, @"\d", "");
        
        return this;
    }
    
    public StringValidator AllowPunctuation()
    {
        if (string.IsNullOrEmpty(Value))
            return this;
        
        _whitelistedCharsDefinitionSet.Add("punctuation characters");

        _charactersOutOfWhitelists = Regex.Replace(_charactersOutOfWhitelists, @"\p{P}", "");
        
        return this;
    }
    
    public StringValidator AllowSymbols()
    {
        if (string.IsNullOrEmpty(Value))
            return this;
        
        _whitelistedCharsDefinitionSet.Add("symbols");

        _charactersOutOfWhitelists = Regex.Replace(_charactersOutOfWhitelists, @"\p{S}", "");
        
        return this;
    }
    
    public StringValidator AllowSpaces()
    {
        if (string.IsNullOrEmpty(Value))
            return this;
        
        _whitelistedCharsDefinitionSet.Add("spaces");

        _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace(" ", "");
        
        return this;
    }
    
    public StringValidator AllowTabs()
    {
        if (string.IsNullOrEmpty(Value))
            return this;
        
        _whitelistedCharsDefinitionSet.Add("tabs");

        _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace("\t", "");
        
        return this;
    }
    
    public StringValidator AllowNewlines()
    {
        if (string.IsNullOrEmpty(Value))
            return this;
        
        _whitelistedCharsDefinitionSet.Add("newlines");

        _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace("\n", "");
        
        return this;
    }
    
    public StringValidator AllowCustomChars(IEnumerable<char> customChars)
    {
        if (string.IsNullOrEmpty(Value))
            return this;
        
        foreach (var x in customChars)
        {
            _whitelistedCharsDefinitionSet.Add(x.ToString());
            _charactersOutOfWhitelists = _charactersOutOfWhitelists.Replace(x.ToString(), "");
        }
        
        return this;
    }
    
    public StringValidator ForbidCustomChars(IEnumerable<char> customChars)
    {
        if (string.IsNullOrEmpty(Value))
            return this;
        
        foreach (var x in customChars)
        {
            _blacklistedCharsDefinitionSet.Add(x.ToString());

            if (Value.Contains(x)) 
                _blacklistedCharsFound.Add(x.ToString());
        }
        
        return this;
    }
    
    public StringValidator ForbidWhitespaceOrEmpty()
    {
        if (string.IsNullOrWhiteSpace(Value) && Value is not null)
        {
            _errors.Add(new ValidationErrors.Strings.WhitespaceOrEmptyStringError(_className, _propertyName));
        }
        
        return this;
    }
    
    public StringValidator LengthBetween(int min, int max)
    {
        if (string.IsNullOrEmpty(Value))
            return this;
        
        if (Value.Length < min || Value.Length > max)
        {
            _errors.Add(new ValidationErrors.Strings.StringInvalidLengthError(_className, _propertyName, min, max));
        }
        
        return this;
    }
    
    public override StringValidator ForbidNull()
    {
        if (Value is null)
        {
            _errors.Add(new ValidationErrors.General.NullValueError(_className, _propertyName));
        }
        
        return this;
    }

    public override IEnumerable<DomainLayerValidationError> ReturnErrors()
    {
        _handleBlacklistViolations();
        _handleWhitelistViolations();
        
        return _errors;
    }

    public override IDictionary<string, string> ReturnSpecification()
    {
        throw new NotImplementedException();
    }
}
