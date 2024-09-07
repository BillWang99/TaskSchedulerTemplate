using FluentValidation;
using Ganss.Xss;
using Microsoft.Identity.Client;

namespace TaskSchedulerTemplate.ViewModels.Home
{
    public class RegisterViewModel
    {
        //姓名
        private string _Name;
        public string Member_Name_ { get => _Name; set => _Name = new HtmlSanitizer().Sanitize(value); }

        //帳號
        private string _Account;
        public string Member_Account_ { get => _Account; set => _Account = new HtmlSanitizer().Sanitize(value); }

        //電子郵件
        private string _Email;
        public string Member_Email_ { get => _Email; set => _Email = new HtmlSanitizer().Sanitize(value); }

        //密碼
        private string _Password;
        public string Member_Password_ { get => _Password; set => _Password = new HtmlSanitizer().Sanitize(value); }

        //手機
        private string _Phone;
        public string Member_Phone_ { get => _Phone; set => _Phone = new HtmlSanitizer().Sanitize(value); }

        //電話
        private string _Tel_Phone;
        public string Member_Tel_Phone_ { get => _Tel_Phone; set => _Tel_Phone = new HtmlSanitizer().Sanitize(value); }
    }

    public class RegisterValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterValidator()
        {
            //姓名規則
            RuleFor(x => x.Member_Name_).NotEmpty().NotNull().WithName("姓名").WithErrorCode("{propertyName}不可為空!");

            //帳號
            RuleFor(x => x.Member_Account_).NotEmpty().NotNull().WithName("帳號").WithMessage("{propertyName}不可為空!");

            //電子郵件
            RuleFor(x => x.Member_Email_).NotEmpty().NotNull().WithName("電子郵件").WithMessage("{propertyName}不可為空!")
                .EmailAddress().WithMessage("{propertyName}格式錯誤!");

            //密碼
            RuleFor(x => x.Member_Password_).NotEmpty().NotNull().WithName("密碼").WithMessage("{propertyName}不可為空!")
                .MinimumLength(8).WithMessage("{propertyName}不可小於8碼!");

            //手機
            RuleFor(x => x.Member_Phone_).NotEmpty().NotNull().WithName("手機").WithMessage("{propertyName}不可為空!");

            //電話
            //RuleFor(x => x.Member_Tel_Phone_).NotEmpty().NotNull().WithName("電話").WithMessage("{propertyName}不可為空!");
        }
    }

}
