using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using YorkDayOfCode.Api.Models.Users;

namespace YorkDayOfCode.Api.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage("Gender must be specified with a valid value");

            RuleFor(x => x.Age)
                .IsInEnum()
                .WithMessage("Age must be specified with a valid value");

            RuleFor(x => x.Area)
                .IsInEnum()
                .WithMessage("Area must be specified with a valid value");

            RuleFor(x => x.Experience)
                .IsInEnum()
                .WithMessage("Experience must be specified with a valid value");
        }
    }
}
