using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common {

    /// <summary>
    /// 分页模型。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasePageModel<T> {
        private ICollection<T> records;
        private PageMetadata page;

        public static BasePageModel<T> Of(ICollection<T> records, PageMetadata metadata) {
            return new BasePageModel<T>(records, metadata);
        }

        public BasePageModel(ICollection<T> records, PageMetadata metadata) {
            this.records = records;
            this.page = metadata;
        }

        public BasePageModel(ICollection<T> records, long size, long current, long totalElements, long totalPages) :
             this(records, new PageMetadata(size, current, totalElements, totalPages)) {
            
        }

        /// <summary>
        /// 分页数据。
        /// </summary>
        public ICollection<T> Records {
            get => records;
            set => records = value;
        }

        /// <summary>
        /// 分页元数据。
        /// </summary>
        public PageMetadata Page {
            get => page;
            set => page = value;
        }

        public override bool Equals(object? obj) {
            if(obj is BasePageModel<T> other) {
                return EqualityComparer<ICollection<T>>.Default.Equals(records, other.records) &&
                       EqualityComparer<PageMetadata>.Default.Equals(page, other.page);
            }
            return false;
        }

        public override int GetHashCode() {
            int hash = 17;
            hash = hash * 23 + (records?.GetHashCode() ?? 0);
            hash = hash * 23 + (page?.GetHashCode() ?? 0);
            return hash;
        }

        public override string ToString() {
            return $"BasePageModel(Records={Records}, Page={Page})";
        }
    }
    /// <summary>
    /// 分页元数据。
    /// </summary>
    public class PageMetadata {
        /// <summary>
        /// 每页记录数。
        /// </summary>
        [JsonProperty]
        public long Size { get; private set; }

        /// <summary>
        /// 总记录数。
        /// </summary>

        [JsonProperty]
        public long TotalElements { get; private set; }

        /// <summary>
        /// 总页数。
        /// </summary>
        [JsonProperty("totalPages")]
        public long TotalPages { get; private set; }

        /// <summary>
        /// 当前页码。
        /// </summary>
        [JsonProperty("currentPage")]
        public long CurrentPage { get; private set; }

        public PageMetadata(long size, long current, long totalElements, long totalPages) {
            if(size < 0)
                throw new ArgumentException("Size must not be negative!");
            if(current < 0)
                throw new ArgumentException("Current Page must not be negative!");
            if(current > totalPages)
                throw new ArgumentException("Current Page must not be greater than total pages!");
            if(totalElements < 0)
                throw new ArgumentException("Total elements must not be negative!");
            if(totalPages < 0)
                throw new ArgumentException("Total pages must not be negative!");

            Size = size;
            CurrentPage = current;
            TotalElements = totalElements;
            TotalPages = totalPages;
        }

        public PageMetadata(long size, long number, long totalElements)
            : this(size, number, totalElements, size == 0 ? 0 : (long)Math.Ceiling((double)totalElements / size)) {
        }

        public override bool Equals(object? obj) {
            if(obj is PageMetadata other) {
                return CurrentPage == other.CurrentPage &&
                       Size == other.Size &&
                       TotalElements == other.TotalElements &&
                       TotalPages == other.TotalPages;
            }
            return false;
        }

        public override int GetHashCode() {
            int hash = 17;
            hash = hash * 31 + CurrentPage.GetHashCode();
            hash = hash * 31 + Size.GetHashCode();
            hash = hash * 31 + TotalElements.GetHashCode();
            hash = hash * 31 + TotalPages.GetHashCode();
            return hash;
        }

        public override string ToString() {
            return string.Format("Metadata {{ CurrentPage: {0}, Total Pages: {1}, Total Elements: {2}, Size: {3} }}", CurrentPage, TotalPages, TotalElements, Size);
        }
    }
}
