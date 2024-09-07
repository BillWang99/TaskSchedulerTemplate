using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskSchedulerTemplate.Models
{
    //人員帳號Table
    public class Member
    {
        //帳號Id
        public Guid Member_Id_ { get; set; }
        //職員編號
        public string Member_StaffCode_ { get; set; }
        //姓名
        public string Member_Name_ { get; set; }
        //電子郵件
        public string Member_Email_ { get;set; }
        //密碼
        public string Member_Password_ { get; set; }
        //手機
        public string? Member_Phone_ { get; set; }
        //電話
        public string? Member_Tel_Phone_ { get; set; }
        //單位
        public string? Member_Dept_ { get; set; }
        //建立時間
        public DateTime Member_CreateTime_ {  get; set; }
        //更新時間
        public DateTime Member_UpdateTime_ { get; set; }
    }
}
