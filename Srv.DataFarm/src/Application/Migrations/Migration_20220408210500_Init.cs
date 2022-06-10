using System;
using FluentMigrator;

namespace UniCryptoLab.Srv.ServiceName.Migrations
{
    //具体使用和参考
    //https://blog.osmosys.asia/2018/06/15/fluentmigrator-database-migration/
    //https://crosscuttingconcerns.com/How-I-use-Fluent-Migrator
    //https://www.cnblogs.com/lwqlun/p/10649949.html

    /// <summary>
    /// 初始化表结构
    /// </summary>
    [Tags("platform")]
    [Migration(20220408210500, "Init DB Schema(ServiceName)")]
    public class Migration_20220408210500 : Migration
    {
        public override void Up()
        {
            this.Execute.EmbeddedScript("UniCryptoLab.Srv.ServiceName.Migrations.Scripts.baseline.sql");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
