using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Infrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "We want users to be able to extend this class")]
    public class MinMaxIntegerAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets the maximum allowable length of the array/string data.
        /// </summary>
        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }

        public MinMaxIntegerAttribute(int minValue, int maxValue)
            : base(() => DefaultErrorMessageString)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxLengthAttribute"/> class.
        /// The maximum allowable length supported by the database will be used.
        /// </summary>
        public MinMaxIntegerAttribute()
            : base(() => DefaultErrorMessageString)
        {
            MinValue = int.MinValue;
            MaxValue = int.MaxValue;
        }

        private static string DefaultErrorMessageString
        {
            get { return "El valor está fuera de los límites establecidos (min:{1}, max:{2})"; }
        }

        /// <summary>
        /// Determines whether a specified object is valid. (Overrides <see cref = "ValidationAttribute.IsValid(object)" />)
        /// </summary>
        /// <remarks>
        /// This method returns <c>true</c> if the <paramref name = "value" /> is null.  
        /// It is assumed the <see cref = "RequiredAttribute" /> is used if the value may not be null.
        /// </remarks>
        /// <param name = "value">The object to validate.</param>
        /// <returns><c>true</c> if the value is null or less than or equal to the specified maximum length, otherwise <c>false</c></returns>
        /// <exception cref = "InvalidOperationException">Length is zero or less than negative one.</exception>
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            int iValue;

            if (int.TryParse(value.ToString(), out iValue) == false)
                return false;


            return iValue >= MinValue || iValue <= MaxValue;
        }

        /// <summary>
        /// Applies formatting to a specified error message. (Overrides <see cref = "ValidationAttribute.FormatErrorMessage" />)
        /// </summary>
        /// <param name = "name">The name to include in the formatted string.</param>
        /// <returns>A localized string to describe the maximum acceptable length.</returns>
        public override string FormatErrorMessage(string name)
        {
            // An error occurred, so we know the value is greater than the maximum if it was specified
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, MinValue, MaxValue);
        }

    }
}
