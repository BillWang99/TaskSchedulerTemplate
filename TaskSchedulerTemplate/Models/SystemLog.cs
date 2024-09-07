namespace TaskSchedulerTemplate.Models
{
    public class SystemLog
    {
        //log編號
        public int SystemLog_Id_ { get; set; }
        //操作內容(新增、修改、刪除)
        public string SystemLog_Option_ {  get; set; }
        //操作描述
        public string SystemLog_Discription_ { get; set; }
        //操作人員
        public string Member_Account_ { get; set; }
    }
}
