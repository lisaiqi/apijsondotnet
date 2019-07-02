using SqlSugar;
using System;

namespace APIJSON.NET.Models
{
    public class rolelist
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public int id { get; set; }
        [SugarColumn(Length = 100, ColumnDescription = "角色代码")]
        public string rolecode { get; set; }
        [SugarColumn(Length = 100, ColumnDescription = "增删改查动作")]
        public string operation { get; set; }
        [SugarColumn(Length = 200, ColumnDescription = "可操作表")]
        public string tables { get; set; }
        [SugarColumn(Length = 100, ColumnDescription = "可操作字段")]
        public string column { get; set; }
        [SugarColumn(Length = 100, ColumnDescription = "筛选条件")]
        public string where { get; set; }

    }
}
