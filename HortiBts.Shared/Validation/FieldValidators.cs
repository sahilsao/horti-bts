using System.Text.RegularExpressions;

namespace HortiBts.Shared.Validation
{
    public static class FieldValidators
    {
        public static string? EnglishName(string? value, Func<string, string, string> text)
        {
            if (string.IsNullOrWhiteSpace(value))
                return text(
                    "English name is required.",
                    "अंग्रेज़ी नाम आवश्यक है।");

            return Regex.IsMatch(value, @"^[a-zA-Z\s\-'\d]+$")
                ? null
                : text(
                    "Only English letters are allowed.",
                    "केवल अंग्रेज़ी अक्षर मान्य हैं।");
        }

        public static string? HindiName(string? value, Func<string, string, string> text)
        {
            if (string.IsNullOrWhiteSpace(value))
                return text(
                    "Hindi name is required.",
                    "हिंदी नाम आवश्यक है।");

            return Regex.IsMatch(value, @"^[\u0900-\u097F\s\-\d]+$")
                ? null
                : text(
                    "Only Hindi characters are allowed.",
                    "केवल हिंदी अक्षर मान्य हैं।");
        }

        public static string? EnglishDescription(string? value, Func<string, string, string> text)
        {
            if (string.IsNullOrWhiteSpace(value))
                return text(
                    "English description is required.",
                    "अंग्रेज़ी विवरण आवश्यक है।");

            return Regex.IsMatch(value, @"^[a-zA-Z\s\-'\d]+$")
                ? null
                : text(
                    "Only English letters are allowed.",
                    "केवल अंग्रेज़ी अक्षर मान्य हैं।");
        }

        public static string? HindiDescription(string? value, Func<string, string, string> text)
        {
            if (string.IsNullOrWhiteSpace(value))
                return text(
                    "Hindi description is required.",
                    "हिंदी विवरण आवश्यक है।");

            return Regex.IsMatch(value, @"^[\u0900-\u097F\s\-\d]+$")
                ? null
                : text(
                    "Only Hindi characters are allowed.",
                    "केवल हिंदी अक्षर मान्य हैं।");
        }

        public static string? EnglishHindiWithSpecialCharsName(string? value, Func<string, string, string> text)
        {
            if (string.IsNullOrWhiteSpace(value))
                return text(
                    "Name is required.",
                    "नाम आवश्यक है।");

            return Regex.IsMatch(
                value,
                @"^[a-zA-Z\u0900-\u097F0-9\s.\-(),'/]+$")
                ? null
                : text(
                    "Only English, Hindi, numbers, spaces, dots, dashes, brackets, commas, apostrophes and slashes are allowed.",
                    "केवल अंग्रेज़ी, हिंदी, अंक, स्पेस, डॉट, डैश, ब्रैकेट, कॉमा, एपोस्ट्रोफ और स्लैश मान्य हैं।");
        }
    }
}
