using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Infrastructure.Models;

namespace Infrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "We want users to be able to extend this class")]
    public class ValidDateAttribute : ValidationAttribute
    {
        public ValidDateAttribute()
            : base(() => DefaultErrorMessageString)
        {
        }

        private static string DefaultErrorMessageString
        {
            get { return "La fecha es inválida"; }
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

            DateTime dtValue;

            if (DateTime.TryParseExact(value.ToString(), SharedConstants.DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtValue) == false)
                return false;

            return true;
        }
    }
}
