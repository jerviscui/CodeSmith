using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace CodeSmith.Abp.Template.Test.Application
{
    public class DefaultPagedResultRequest : IPagedResultRequest, ISortedResultRequest
    {
        /// <summary>
        /// 最大结果行数
        /// </summary>
        [Range(1, int.MaxValue)]
        public int MaxResultCount
        {
            get
            {
                return Rows;
            }
            set { }
        }

        /// <summary>
        /// 过滤多少行
        /// </summary>
        [Range(0, int.MaxValue)]
        public int SkipCount
        {
            get
            {
                return Rows * (Page - 1);
            }
            set {   }
        }

        public DefaultPagedResultRequest()
        {
            MaxResultCount = 10;
            Sorting = "Id";
        }

        /// <summary>
        /// 页
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 行
        /// </summary>
        public int Rows { get; set; }

        public string Sorting { get; set; }
    }
}