using Domain.Shared.Errors;

namespace Domain.Errors.Validation;

public static class ValidationErrors
{
    public static class Ids
    {
        public class IdFormatError(string className, string propertyName) 
            : DomainLayerValidationError(className, propertyName, "Invalid ID format.");
    }
    
    
    public static class Decimals
    {
        public class DecimalPrecisionError(string className, string propertyName,
            int actualPrecision, int minPrecision, int maxPrecision) 
            : DomainLayerValidationError(className, propertyName, 
                $"Only decimal values with precision between" +
                $" {minPrecision} and {maxPrecision}" + 
                $" are allowed for this field. Actual precision: {actualPrecision}.");
        
        public class DecimalValueError(string className, string propertyName,
            decimal actualValue, decimal minValue, decimal maxValue) 
            : DomainLayerValidationError(className, propertyName,
                $"Only decimal values between" +
                $" {minValue} and {maxValue} are allowed for this field." + 
                $" Actual value: {actualValue}.");
    }

    
    public static class General
    {
        public class NullValueError(string className, string propertyName) 
            : DomainLayerValidationError(className, propertyName, 
                "Can't perform an operation with NULL value in this field.");
    }

    
    public static class LongIntegers
    {
        public class LongIntTooSmallNumberError(string className, string propertyName,
            long actualValue, long minValue)
            : DomainLayerValidationError(className, propertyName, $"Only values above {minValue}" +
                                                                  $" are allowed for this field." + 
                                                                  $" Actual value: {actualValue}.");
        
        public class LongIntTooBigNumberError(string className, string propertyName,
            long actualValue, long maxValue)
            : DomainLayerValidationError(className, propertyName, $"Only values under {maxValue}" +
                                                                  $" are allowed for this field." + 
                                                                  $" Actual value: {actualValue}.");
        public class LongIntOutOfRangeError(string className, string propertyName, long actualValue,
            long minValue, long maxValue)
            : DomainLayerValidationError(className, propertyName, $"Only values between {minValue}" +
                                                                  $" and {maxValue} are allowed for this field." + 
                                                                  $" Actual value: {actualValue}.");
        
        public class LongIntOutOfAllowedListError(string className, string propertyName,
            long actualValue, IEnumerable<long> allowedValues)
            : DomainLayerValidationError(className, propertyName,
                $"Only values {allowedValues} are allowed for this field." + 
                $" Actual value: {actualValue}.");
    }
    
    public static class Doubles
    {
        public class DoubleTooSmallNumberError(string className, string propertyName,
            double actualValue, double minValue)
            : DomainLayerValidationError(className, propertyName, $"Only values above {minValue}" +
                                                                  $" are allowed for this field." + 
                                                                  $" Actual value: {actualValue}.");
        
        public class DoubleTooBigNumberError(string className, string propertyName,
            double actualValue, double maxValue)
            : DomainLayerValidationError(className, propertyName, $"Only values under {maxValue}" +
                                                                  $" are allowed for this field." + 
                                                                  $" Actual value: {actualValue}.");
        public class DoubleOutOfRangeError(string className, string propertyName, double actualValue,
            double minValue, double maxValue)
            : DomainLayerValidationError(className, propertyName, $"Only values between {minValue}" +
                                                                  $" and {maxValue} are allowed for this field." + 
                                                                  $" Actual value: {actualValue}.");
    }

    public static class Lists
    {
        public class EmptyListError(string className, string propertyName)
            : DomainLayerValidationError(className, propertyName, "This list can not be empty.");
        
        public class ListInvalidLengthError(string className, string propertyName, int minLength, int maxLength) 
            : DomainLayerValidationError(className, propertyName,
                $"This list has invalid length." + 
                $" Allowed length is between {minLength} and {maxLength} elements.");
    }


    public static class Strings
    {
        private static string _getListlikeString(IEnumerable<string> collection)
        {
            string finalStr = "";

            foreach (var x in collection)
            {
                finalStr += $"'{x}', ";
            }

            finalStr = finalStr[..^2];

            return finalStr;
        }
        
        public class BlacklistError(string className, string propertyName,
            HashSet<string> invalidCharsFound, HashSet<string> forbiddenChars)
            : DomainLayerValidationError(className, propertyName, 
                $"Invalid chars found -> {_getListlikeString(invalidCharsFound)}." + 
                $" Forbidden chars for this field -> {_getListlikeString(forbiddenChars)}.");
        
        public class WhitelistError(string className, string propertyName,
            HashSet<string> invalidCharsFound, HashSet<string> allowedCharDescriptions)
            : DomainLayerValidationError(className, propertyName,
                $"Invalid chars found -> {_getListlikeString(invalidCharsFound)}." +
                $" Allowed chars for this field (unless not forbidden explicitly) -> {_getListlikeString(allowedCharDescriptions)}.");
        
        public class EmailFormatError(string className, string propertyName)
            : DomainLayerValidationError(className, propertyName, "Invalid email format.");
        
        public class WhitespaceOrEmptyStringError(string className, string propertyName)
            : DomainLayerValidationError(className, propertyName, "This string should not be empty or be whitespace.");
        
        public class StringInvalidLengthError(string className, string propertyName, int minLength, int maxLength) 
            : DomainLayerValidationError(className, propertyName,
                $"This string has invalid length." + 
                $" Allowed length is between {minLength} and {maxLength} elements.");
    }
}