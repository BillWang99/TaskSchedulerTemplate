using TaskSchedulerTemplate.ViewModels.Home;

namespace TaskSchedulerTemplate.Interface.Home
{
    public interface ILoginSerivce
    {
        //檢查帳號是否存在
        bool AcExist(string Member_Account_);

        //登入驗證
        bool LoginValidation(LoginViewModel model);

        //取得cookie資料
        UserCookie UserCookie(string Member_Account_);
    }
}
