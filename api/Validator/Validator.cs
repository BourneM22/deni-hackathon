using System;
using System.ComponentModel.DataAnnotations;

namespace api.Validator
{
    public class GenderValidationAttribute : ValidationAttribute
    {
        public override Boolean IsValid(object? value)
        {
            // Ensure the value is not null or empty
            if (value is Char gender && !String.IsNullOrEmpty(gender.ToString()))
            {
                // Only accept "M" or "F"
                return gender == 'M' || gender == 'F';
            }

            // If value is null or doesn't match "M" or "F", it's invalid
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be either 'M' or 'F'.";
        }
    }

    public class DateLessThanTodayAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateOnly dateTime)
            {
                // Compare the given date with the current date (ignoring time part)
                return dateTime <= DateOnly.FromDateTime(DateTime.Today);
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must be a date before today.";
        }
    }
}