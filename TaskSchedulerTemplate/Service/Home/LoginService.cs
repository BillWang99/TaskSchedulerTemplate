using Dapper;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using TaskSchedulerTemplate.ViewModels.Home;
using TaskSchedulerTemplate.Interface.Home;

namespace TaskSchedulerTemplate.Service.Home
{
    public class LoginService:ILoginSerivce
    {
        //連線字串
        private readonly SqlConnection _con;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public LoginService(SqlConnection con, IHttpContextAccessor httpContextAccessor)
        {
            _con = con;
            _HttpContextAccessor = httpContextAccessor;
        }

        //檢查登入帳號是否存在
        public bool AcExist(string Member_Account_)
        {
            //建立sql
            string sql = @"SELECT  Member_Account_ FROM Member WHERE Member_Account_ = @Member_Account_";

            //取出結果
            string Ac = _con.QueryFirstOrDefault<string>(sql, new { Member_Account_ });

            if (Ac != null && Ac != "")
            {
                //帳號存在
                return true;
            }
            else
            {
                //帳號不存在
                return false;
            }
        }

        //取出密碼並進行登入驗證
        public bool LoginValidation(LoginViewModel model)
        {
            //建立sql
            string sql = @"SELECT Member_Account_, Member_Password_ FROM Member WHERE Member_Account_ = @Member_Account_";

            //取出資料
            LoginViewModel Data = _con.QueryFirstOrDefault<LoginViewModel>(sql, new { model.Member_Account_ });

            //如果無符合資料
            if(Data == null || Data.Member_Account_ == "")
            {
                return false;
            }

            //雜湊演算法
            var sha256 = SHA256.Create();

            //讀取密碼前3個字元
            var salt = model.Member_Password_.Substring(0, 3);

            //原始密碼加工
            var passwordSalt = model.Member_Password_ + salt;

            //使用UTF8編碼
            var byteValue = Encoding.UTF8.GetBytes(passwordSalt);

            //進行加密
            var byteHash = sha256.ComputeHash(byteValue);

            //將加密結果存回obj.Password
            model.Member_Password_ = Convert.ToBase64String(byteHash);

            if(model.Member_Password_ == Data.Member_Password_)
            {
                //密碼正確，建立登入Log
                CreateLoginLog(model.Member_Account_);

                //回到Controller建立驗證
                return true;
            }
            else
            {
                //密碼錯誤
                return false;
            }

        }

        //登入Log
        private void CreateLoginLog(string Member_Account_) 
        {
            //建立sql
            string sql = @"INSERT INTO SystemLog(
                            SystemLog_Option_,
                            SystemLog_Discription_,
                            Member_Account_,
                            Log_CreateTime_
                            )
                            VALUES(
                            '登入',
                            @Member_Account_  + '於' + convert(varchar, getdate(), 20)+'登入系統',
                            @Member_Account_,
                            GETDATE()
                            )";
            //紀錄Log
            _con.Execute(sql, new { Member_Account_ });
        }

        //取得要寫入cookie的資料
        public UserCookie UserCookie(string Member_Account_)
        {
            //建立sql
            string sql = @"SELECT Member_Account_, Member_Name_, Member_StaffCode_ FROM Member WHERE Member_Account_ = @Member_Account_";

            //取得資料
            UserCookie Data = _con.QueryFirstOrDefault<UserCookie>(sql, new {Member_Account_});

            //回傳結果
            return Data;
        }
    }
}
