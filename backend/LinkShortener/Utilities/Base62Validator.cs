namespace LinkShortener.Utilities;

using System;
using System.Linq;
using System.Text;

/// <summary>
/// Provides validation and conversion utilities for Base62 encoding.
/// Base62 uses alphanumeric characters (0-9, A-Z, a-z) for encoding data.
/// </summary>
public static class Base62Validator
{
    /// <summary>
    /// The character set used for Base62 encoding (0-9, A-Z, a-z).
    /// </summary>
    private const string Base62Charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    /// <summary>
    /// Pre-calculated value of 62^8, used as a boundary for Base62 values.
    /// This represents the maximum value that can be stored in 8 Base62 characters.
    /// </summary>
    public static readonly ulong Pow62_8 = (ulong)Math.Pow(62, 8);
    
    /// <summary>
    /// Validates if a string is maximum 8 characters and in Base62 format (alphanumeric: 0-9, A-Z, a-z).
    /// </summary>
    /// <param name="input">The string to validate.</param>
    /// <returns>True if the string is valid (max 8 characters and only Base62 chars), false otherwise.</returns>
    /// <example>
    /// <code>
    /// bool isValid = Base62Validator.IsValidBase62("Ab3x9Z");  // Returns true
    /// bool isInvalid = Base62Validator.IsValidBase62("Ab3$9Z"); // Returns false (contains non-Base62 char)
    /// </code>
    /// </example>
    public static bool IsValidBase62(string input)
    {
        // Check for null or empty string
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        // Check if string length is at most 8 characters
        if (input.Length > 8)
        {
            return false;
        }

        // Check if all characters are in the Base62 character set
        return input.All(c => 
            (c >= '0' && c <= '9') || 
            (c >= 'A' && c <= 'Z') || 
            (c >= 'a' && c <= 'z')
        );
    }
    
    /// <summary>
    /// Converts a Base62 encoded string to a long integer value.
    /// </summary>
    /// <param name="base62String">The Base62 encoded string to convert.</param>
    /// <returns>The decoded long integer value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input string is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the input string contains non-Base62 characters.</exception>
    /// <exception cref="OverflowException">Thrown when the result exceeds the range of a long integer.</exception>
    /// <example>
    /// <code>
    /// long value = Base62Validator.Base62ToLong("3D7");  // Converts "3D7" to its numeric value
    /// </code>
    /// </example>
    public static long Base62ToLong(string base62String)
    {
        // Input validation
        if (base62String == null)
        {
            throw new ArgumentNullException(nameof(base62String), "Base62 string cannot be null");
        }
        
        if (base62String.Length == 0)
        {
            return 0;
        }

        long result = 0;
        
        // Convert each character to its numeric value and calculate the sum
        foreach (char c in base62String)
        {
            int charValue = Base62Charset.IndexOf(c);
            
            if (charValue == -1)
            {
                throw new ArgumentException($"Invalid Base62 character: {c}", nameof(base62String));
            }
            
            // Check for overflow before performing the multiplication
            if (result > long.MaxValue / 62)
            {
                throw new OverflowException("Base62 value is too large for long integer");
            }
            
            result = result * 62 + charValue;
            
            // Check for overflow after the addition
            if (result < 0)
            {
                throw new OverflowException("Base62 value is too large for long integer");
            }
        }
    
        return result;
    }
    
    /// <summary>
    /// Converts a long integer value to a Base62 encoded string.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A Base62 encoded string representation of the value.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the input value is negative.</exception>
    /// <example>
    /// <code>
    /// string encoded = 12345L.LongToBase62();  // Converts 12345 to Base62 string
    /// </code>
    /// </example>
    public static string LongToBase62(this long value)
    {
        // Handle special case for zero
        if (value == 0)
        {
            return "0";
        }
        
        // Base62 doesn't support negative numbers
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), 
                "Base62 encoding does not support negative numbers");
        }
    
        // Use StringBuilder for efficient string construction
        var result = new StringBuilder();
    
        // Make a copy to avoid modifying the input parameter
        long remaining = value;
    
        // Convert the number to Base62 by repeatedly dividing by 62
        while (remaining > 0)
        {
            // Get the remainder when divided by 62 (modulo operation)
            result.Insert(0, Base62Charset[(int)(remaining % 62)]);
            remaining /= 62;
        }
    
        return result.ToString();
    }
    
    /// <summary>
    /// Extension method that converts a Base62 string to its long integer value.
    /// </summary>
    /// <param name="base62String">The Base62 string to convert.</param>
    /// <returns>The decoded long integer value.</returns>
    /// <example>
    /// <code>
    /// long value = "3D7".FromBase62();  // Extension method usage
    /// </code>
    /// </example>
    public static long FromBase62(this string base62String)
    {
        return Base62ToLong(base62String);
    }
    
    /// <summary>
    /// Extension method that converts a long integer to its Base62 string representation.
    /// </summary>
    /// <param name="value">The long integer to convert.</param>
    /// <returns>The Base62 encoded string.</returns>
    /// <example>
    /// <code>
    /// string encoded = 12345L.ToBase62();  // Extension method usage
    /// </code>
    /// </example>
    public static string ToBase62(this long value)
    {
        return LongToBase62(value);
    }
}