namespace UniCryptoLab.Models
{
    public enum EnumBarSyncTaskStatus
    {
        /// <summary>
        /// 待处理
        /// </summary>
        Pending=1,
        
        /// <summary>
        /// 处理中
        /// </summary>
        Processing=2,
        
        /// <summary>
        /// 已完成
        /// </summary>
        Completed=3,
        
        /// <summary>
        /// 已终止
        /// </summary>
        Terminated = 4,
        
        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 5,
    }
}