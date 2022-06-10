using FluentMigrator.Runner.VersionTableInfo;

namespace UniCryptoLab.Srv.Notify
{
    [VersionTableMetaData]
    public class DBVersionTable : IVersionTableMetaData
    {
        public object ApplicationContext { get; set; }

        public bool OwnsSchema { get { return false; } }
        
        public string ColumnName
        {
            get { return "Version"; }
        }

        public string SchemaName
        {
            get { return ""; }
        }

        public string TableName
        {
            get { return $"_DBVersionSrv{Program.AppName}"; }
        }

        public string UniqueIndexName
        {
            get { return "UC_Version"; }
        }

        public virtual string AppliedOnColumnName
        {
            get { return "AppliedOn"; }
        }

        public virtual string DescriptionColumnName
        {
            get { return "Description"; }
        }
    }
}