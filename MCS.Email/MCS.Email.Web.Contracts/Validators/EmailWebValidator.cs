using FluentValidation;
using MCS.Email.Web.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Email.Web.Contracts.Validators
{
    public class EmailWebValidator : AbstractValidator<EmailWeb>
    {
        public EmailWebValidator()
        {
            RuleFor(p => p.Id).NotEmpty().WithMessage("Id must be not empty"); 
            RuleFor(p => p.Content).NotEmpty().WithMessage("Content must be not empty");
            RuleFor(p => p.Subject).NotEmpty().WithMessage("Subject must be not empty");
            RuleForEach(p => p.Emails).EmailAddress().WithMessage("Not all emails are valid");
        }
    }
}
