using Dapper;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using TaskSchedulerTemplate.ViewModels.Home;
using TaskSchedulerTemplate.Interface.Home;

namespace TaskSchedulerTemplate.Service.Home
{
    public class RegisterService:IRegisterService
    {
        //連線字串
        private readonly SqlConnection _con;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public RegisterService(SqlConnection con, IHttpContextAccessor httpContextAccessor)
        {
            _con = con;
            _HttpContextAccessor = httpContextAccessor;
        }

        //查詢帳號是否存在
        public bool AcHasBeenUsed(string Member_Account_)
        {
            //建立sql
            string sql = @"SELECT  Member_Account_ FROM Member WHERE Member_Account_ = @Member_Account_";

            //取出結果
            string Ac = _con.QueryFirstOrDefault<string>(sql, new { Member_Account_ });

            if (Ac != "" && Ac != null){
                //帳號已被使用
                return true;
            }
            else
            {
                //帳號可被註冊
                return false;
            }
        }

        //註冊帳號
        public void RegisterAccount(RegisterViewModel model)
        {
            //建立sql
            var sql = @"INSERT INTO Member 
                        (
                         Member_Id_,
                         Member_Name_,
                         Member_StaffCode_,
                         Member_Account_,
                         Member_Email_,
                         Member_Password_,
                         Member_Dept_,
                         Member_Phone_,
                         Member_Tel_Phone_,
                         Member_CreateTime_,
                         Member_UpdateTime_)
                         VALUES(@Id,
                         @Name,
                         @StaffCode,
                         @Account,
                         @Email,
                         @Password,
                         @Dept,
                         @Phone,
                         @Tel_Phone,
                         GETDATE(),
                         GETDATE())";

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

            //製作員工編號
            string StaffCode = CreateStaffCode();

            //製作員工Guid
            Guid Member_Id_ = Guid.NewGuid();

            //寫入資料庫
            _con.Execute(sql, new { 
                Id = Member_Id_,
                Name = model.Member_Name_,
                StaffCode = StaffCode,
                Account = model.Member_Account_,
                Email = model.Member_Email_,
                Password = model.Member_Password_,
                Dept = "",
                Phone = model.Member_Phone_,
                Tel_Phone = model.Member_Tel_Phone_,
                });
        }

        //製作員工編號
        public string CreateStaffCode()
        {
            //以亂數產生員工編號
            Random r = new Random((int)DateTime.Now.Ticks);

            //僅為數字
            const string chars = "0123456789";

            //建立字串
            string preCode = new string(Enumerable.Repeat(chars, 10).Select(s => s[r.Next(chars.Length)]).ToArray());

            //檢查編號是否重複
            bool CheckCode = StaffCodeHasBeenUsed(preCode);

            if (CheckCode) 
            {
                //編號重複，重新產出
                return CreateStaffCode();
            }
            else
            {
                //編號無重複，回傳編號
                return preCode;
            }
            
        }

        //確認員工編號是否重複
        public bool StaffCodeHasBeenUsed(string preCode)
        {
            //檢查是否有相同編號
            string sql = @"SELECT Member_StaffCode_ FROM Member WHERE Member_StaffCode_ = @preCode";

            //取出結果
            string HasCode = _con.QueryFirstOrDefault(sql, new { preCode });

            if (HasCode != "" && HasCode != null)
            {
                //編號重複
                return true;
            }
            else
            {
                //編號不重複
                return false;
            }
        }
    }
}
