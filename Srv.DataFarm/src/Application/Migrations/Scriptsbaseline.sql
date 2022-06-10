
CREATE TABLE IF NOT EXISTS `HistBar`  (
  `Id` bigint(20) unsigned   NOT NULL,
  `EndTime` datetime   NOT NULL COMMENT 'Bar结束时间',
  `Symbol` char(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '频率',
  `Interval` int(11)   NOT NULL COMMENT '周期数',
  `IntervalType` int(11)   NOT NULL COMMENT '周期类别',
  `Open` double   NOT NULL COMMENT '开盘价',
  `High` double   NOT NULL COMMENT '最高价',
  `Low` double   NOT NULL COMMENT '最低价',
  `Close` double   NOT NULL COMMENT '收盘价',
  `Volume` double   NOT NULL COMMENT '成交量',
  `TradeCount` int(11)   NOT NULL COMMENT '成交笔数',
PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;

CREATE TABLE IF NOT EXISTS `HistBarSyncTask`  (
  `Id` varchar(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Exchange` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `Symbol` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL COMMENT '品种',
  `Interval` int(11)   NOT NULL COMMENT '频率',
  `IntervalType` int(11)   NOT NULL COMMENT '频率数',
  `StartTime` datetime   NOT NULL COMMENT '开始时间',
  `EndTime` datetime   NOT NULL COMMENT '结束时间',
  `SyncedTime` datetime   NOT NULL COMMENT '同步时间',
  `Reason` text CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NULL COMMENT '终止原因',
  `Status` int(4)   NOT NULL COMMENT '任务状态',
  `CreateTime` datetime   NOT NULL COMMENT '创建时间',
  `UpdateTime` datetime   NOT NULL COMMENT '更新时间',
PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_unicode_ci ROW_FORMAT = Dynamic;
