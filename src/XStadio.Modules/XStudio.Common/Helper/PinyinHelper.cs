using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common.Helper
{
    public static class PinyinHelper
    {
        /// <summary>
        /// 获取单个字符的拼音
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetPinyin(char c)
        {
            return TinyPinyin.PinyinHelper.GetPinyin(c);
        }

        /// <summary>
        /// 获取文本字符串的拼音，允许设定拼音分割符
        /// </summary>
        /// <param name="text">要获取拼音的文本</param>
        /// <param name="separator">拼音分割符，默认空格</param>
        /// <returns></returns>
        public static string GetPinyin(string text, string separator = " ")
        {
            return TinyPinyin.PinyinHelper.GetPinyin(text, separator);
        }

        /// <summary>
        /// 获取拼音首字母
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator">拼音分割符，默认空字符串（不分割）</param>
        /// <returns></returns>
        public static string GetPinyinInitials(string str, string separator = "")
        {
            return TinyPinyin.PinyinHelper.GetPinyinInitials(str, separator);
        }

        /// <summary>
        /// 判断单个字符是否是中文
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsChinese(char c)
        {
            return TinyPinyin.PinyinHelper.IsChinese(c);
        }
    }
}
