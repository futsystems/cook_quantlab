using System;
using UniCryptoLab.Common.Data.PetaPocoExt;

namespace UniCryptoLab.Models
{
    public class SyncBarTaskModel
    {
        public string Id { get; set; }

        public string Exchange { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public EnumBarSyncTaskStatus Status { get; set; }

        public string Reason { get; set; }

        public DateTime SyncedTime { get; set; }
    }
}