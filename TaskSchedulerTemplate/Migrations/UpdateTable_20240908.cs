using FluentMigrator;


namespace TaskSchedulerTemplate.Migrations
{
    [Migration(20240908-1)]
    public class UpdateTable_20240908 : Migration
    {
        public override void Up()
        {
            Alter.Table("SystemLog")
                .AddColumn("Log_CreateTime_").AsDateTime().NotNullable();

            
        }

        public override void Down()
        {
            //Delete.Table("SystemLog");
            //Delete.Table("Member");
        }
    }
}
