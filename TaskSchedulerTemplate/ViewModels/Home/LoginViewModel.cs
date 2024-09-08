using FluentValidation;
using Ganss.Xss;

namespace TaskSchedulerTemplate.ViewModels.Home
{
    public class LoginViewModel
    {
        //帳號
        private string _Account;
        public string Member_Account_ { get => _Account; set => _Account = new HtmlSanitizer().Sanitize(value); }

        //密碼
        private string _Password;
        public string Member_Password_ { get => _Password; set => _Password = new HtmlSanitizer().Sanitize(value); }
    }

    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator() 
        {
            //帳號不可為空
            RuleFor(x => x.Member_Account_).NotEmpty().WithName("帳號").WithMessage("{propertyName}不可為空!");

            //密碼不可為空
            RuleFor(x => x.Member_Password_).NotEmpty().WithName("密碼").WithMessage("{propertyName}不可為空!");
        }
    } 

    //登入驗證時儲存於cookie的資料
    public class UserCookie
    {
        //帳號
        public string Member_Account_ { get; set; }
        //姓名
        public string Member_Name_ { get; set; }
        //員工編號
        public string Member_Staffcode_ { get; set; }
    }
}
