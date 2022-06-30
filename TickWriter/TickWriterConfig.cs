namespace TickWriter
{
    public class TickWriterConfig
    {
        public string MasterTick { get; set; }

        public string SlaveTick { get; set; }

        public int DataPort { get; set; }

        public int ReqPort { get; set; }

        public string DataPath { get; set; }

        public string TickPrefix { get; set; }
    }
}