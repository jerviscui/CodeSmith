using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeSmith.Abp.Model
{
    public class EnumItem
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        /// <summary>
        /// 包含命名空间
        /// </summary>
        public string TypeName { get; set; }        
    }

    
}
