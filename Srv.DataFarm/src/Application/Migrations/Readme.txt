PaymentOrder:支付订单
PaymentBizOrder：业务订单
PaymentOrderStage：支付阶段
Payment:支付
PaymentTransaction：支付交易
PaymentRefund:退款
PaymentRefundTransaction：退款交易
PaymentCODTxn：货到付款交易
PaymentAccount：支付模块收款账户
PaymentAccountTxn：支付模块收款账户流水



1.本地编写Migration并运行
2.运行程序后更新数据库
3.运行T4模版生成数据对象
4.继续业务处理

    ///// <summary>
    ///// 自动回退Migration
    ///// https://fluentmigrator.github.io/articles/migration/migration-auto-reversing.html
    ///// </summary>
    //[Migration(20201212105802)]
    //public class Migration_20201212105802 : AutoReversingMigration
    //{
    //    public override void Up()
    //    {
    //        //Create.Table("Log")
    //        //    .WithColumn("Id").AsInt64().PrimaryKey().Identity()
    //        //    .WithColumn("Text").AsString();
    //    }
    //}


    //[Migration(20201212105801)]
    //public class Migration_20201212105801 : Migration
    //{
    //    public override void Up()
    //    {
    //        //添加字段
    //        Alter.Table("Log")
    //            //.InSchema("")
    //            .AddColumn("Field1")
    //            .AsString(40)
    //            .NotNullable()
    //            .WithDefaultValue("default value")
    //            .WithColumnDescription("测试Field1");

    //        //重命名
    //        //Rename.Table("Users").To("UsersNew");
    //        //Rename.Column("LastName").OnTable("Users").To("Surname");

    //        //删除
    //        //Delete.Table("Users");
    //        //Delete.Column("Field1");

    //    }

    //    public override void Down()
    //    {
    //        //删除字段
    //        Delete.Column("Field1").FromTable("Log");
    //    }
    //}