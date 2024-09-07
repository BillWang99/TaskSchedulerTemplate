using TaskSchedulerTemplate.ViewModels.Home;

namespace TaskSchedulerTemplate.Interface.Home
{
    public interface IRegisterService
    {
        //檢查帳號是否重複
        bool AcHasBeenUsed(string Member_Account_);

        //註冊帳號
        void RegisterAccount(RegisterViewModel model);
    }
}
