using FluentMigrator;
using FluentMigrator.SqlServer;

namespace TaskSchedulerTemplate.Migrations
{
    [Migration(20240907-1)]
    public class CreateTable_20240907:Migration
    {
        public override void Up()
        {
            Create.Table("Member")
                .WithColumn("Member_Id_").AsGuid().PrimaryKey()
                .WithColumn("Member_StaffCode_").AsString().Unique().NotNullable()
                .WithColumn("Member_Name_").AsString().NotNullable()
                .WithColumn("Member_Account_").AsString().NotNullable().Unique()
                .WithColumn("Member_Email_").AsString().NotNullable()
                .WithColumn("Member_Password_").AsString().NotNullable()
                .WithColumn("Member_Phone_").AsString().NotNullable()
                .WithColumn("Member_Tel_Phone_").AsString().Nullable()
                .WithColumn("Member_Dept_").AsString().Nullable()
                .WithColumn("Member_CreateTime_").AsDateTime().NotNullable()
                .WithColumn("Member_UpdateTime_").AsDateTime()
                ;

            Create.Table("SystemLog")
                .WithColumn("SystemLog_Id_").AsInt32().PrimaryKey().Identity()
                .WithColumn("SystemLog_Option_").AsString().NotNullable()
                .WithColumn("SystemLog_Discription_").AsString().NotNullable()
                .WithColumn("Member_Account_").AsString().NotNullable().ForeignKey()
                ;

            Create.ForeignKey() // You can give the FK a name or just let Fluent Migrator default to one
                .FromTable("SystemLog").ForeignColumn("Member_Account_")
                .ToTable("Member").PrimaryColumn("Member_Account_");
        }

        public override void Down()
        {
            Delete.Table("SystemLog");
            Delete.Table("Member");
        }
    }
}
