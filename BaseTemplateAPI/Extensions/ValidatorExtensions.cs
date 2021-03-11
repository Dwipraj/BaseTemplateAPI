using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseTemplateAPI.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password must be at least of 6 characters")
                .Matches("[A-Z]").WithMessage("Password must contains a uppercase character")
                .Matches("[a-z]").WithMessage("Password must contains a lowercase character")
                .Matches("[0-9]").WithMessage("Password must contains a numeric character")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contains a non alphanumeric character");

            return options;
        }

        public static IRuleBuilder<T, string> Phone<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .Matches("[0-9]{10}").WithMessage("Phone number must contains 10 digits only");

            return options;
        }
    }
}
