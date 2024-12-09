using hyjiacan.py4n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace XStudio.Common.Comparers {
    public class PinyinComparer : IComparer<string>, ITransientDependency {
        public int Compare(string? x, string? y) {
            if (x == null && y == null) return 0;
            if (x == null) return -1; // null 排在前面
            if (y == null) return 1;  // null 排在前面

            // 将中文字符转换为拼音
            string pinyinX = GetPinyin(x);
            string pinyinY = GetPinyin(y);

            return string.Compare(pinyinX, pinyinY, StringComparison.Ordinal);
        }

        private string GetPinyin(string input) {
            if (string.IsNullOrEmpty(input)) {
                return string.Empty;
            }

            //hyjiacan.py4n 是一个.NET 库，用于将中文转换为拼音。
            return Pinyin4Net.GetPinyin(input, PinyinFormat.WITHOUT_TONE);
        }
    }
}
