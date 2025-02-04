using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common {
    /// <summary>
    /// 分页查询参数
    /// </summary>
    public class QueryPageParam {
        /// <summary>
        /// 查询字段
        /// </summary>
        [JsonProperty("queryFields")]
        public List<string> QueryFields { get; set; } = new List<string>();

        /// <summary>
        /// 关键字
        /// </summary>
        [JsonProperty("keyword")]
        public string Keyword { get; set; } = string.Empty;

        /// <summary>
        /// 页码
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; } = 0;

        /// <summary>
        /// 页大小
        /// </summary>
        [JsonProperty("size")]
        public int Size { get; set; } = 20;

        /// <summary>
        /// 排序项
        /// </summary>
        [JsonProperty("sortItem")]
        public List<SortItem> SortItem { get; set; } = new List<SortItem>();

        public QueryPageParam() {
        }

        public QueryPageParam(List<string> queryFields, string keyword, int? page = null, int? size = null, List<SortItem>? sortItem = null) {
            QueryFields = queryFields;
            Keyword = keyword;
            Page = page ?? 0; // 默认为 0
            Size = size ?? 20; // 默认为 20
            SortItem = sortItem ?? new List<SortItem>();
        }

        public override bool Equals(object? obj) {
            if(obj is QueryPageParam other) {
                return (QueryFields == other.QueryFields || (QueryFields != null && QueryFields.Equals(other.QueryFields))) &&
                       Keyword == other.Keyword &&
                       Page == other.Page &&
                       Size == other.Size &&
                       (SortItem == other.SortItem || (SortItem != null && SortItem.Equals(other.SortItem)));
            }
            return false;
        }

        public override int GetHashCode() {
            // 计算哈希代码
            return HashCode.Combine(QueryFields, Keyword, Page, Size, SortItem);
        }

        public override string ToString() {
            return $"QueryPageParam(QueryFields={string.Join(",", QueryFields)}, Keyword={Keyword}, Page={Page}, Size={Size}, SortItem={string.Join(",", SortItem)})";
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SortItem {
        [JsonProperty("column")]
        public string Column { get; set; } = string.Empty;

        [JsonProperty("asc")]
        public bool Asc { get; set; } = true;

        public SortItem() { }

        public SortItem(string column, bool asc) {
            Column = column;
            Asc = asc;
        }

        public override bool Equals(object? obj) {
            if(obj is SortItem other) {
                return Asc == other.Asc && string.Equals(Column, other.Column);
            }
            return false;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Column, Asc);
        }

        public override string ToString() {
            return $"SortItem(Column={Column}, Asc={Asc})";
        }
    }
}
