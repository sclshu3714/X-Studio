using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.DivideintoClasses.DivideintoClasses
{
    public enum CodeNumber
    {
        [Description("未知")]
        Unknown = 0, // 未知
        [Description("无消息")]
        None = 1,    // 无消息
        [Description("成功")]
        Success = 2, // 成功
        [Description("警告")]
        Warn = 3,    // 警告
        [Description("错误")]
        Error = 4,   // 错误
    }

    

    public static class CodeNumberHelper
    {
        public static string? ToCodeMessage(this CodeNumber code)
        {
            return code.GetEnumDescription();
        }

        // 扩展方法：为属性添加注释
        private static string? GetEnumDescription<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type");
            }

            var enumType = enumValue.GetType();
            var name = Enum.GetName(enumType, enumValue);
            if (name != null)
            {
                var field = enumType.GetField(name);
                if (field != null)
                {
                    var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 消息结构提
    /// </summary>
    [Serializable]
    public class MessageStruct
    {
        /// <summary>
        /// 消息代码
        /// </summary>
        public CodeNumber CodeNumber { get; set; } = CodeNumber.None;

        /// <summary>
        /// 状态消息
        /// </summary>
        public string? Message { get; set; } = string.Empty;

        /// <summary>
        /// 异常消息
        /// </summary>
        public string? ExceptionMessage { get; set; } = string.Empty;

        /// <summary>
        /// 异常堆栈
        /// </summary>
        public string? ExceptionStackTrace { get; set; } = string.Empty;

        /// <summary>
        /// 异常来源
        /// </summary>
        public string? ExceptionSource { get; set; } = string.Empty;


        /// <summary>
        /// 构造
        /// </summary>
        public MessageStruct()
        {
            //none
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="codeNumber"></param>
        public MessageStruct(CodeNumber codeNumber):
            this()
        {
            CodeNumber = codeNumber;
            Message = codeNumber.ToCodeMessage();
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="codeNumber"></param>
        /// <param name="message"></param>
        public MessageStruct(CodeNumber codeNumber, string message):
            this(codeNumber)
        {
            Message = message;
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="codeNumber"></param>
        /// <param name="message"></param>
        /// <param name="exceptionMessage"></param>
        /// <param name="exceptionStackTrace"></param>
        /// <param name="exceptionSource"></param>
        public MessageStruct(CodeNumber codeNumber, string message, string exceptionMessage, string exceptionStackTrace, string exceptionSource)
            : this(codeNumber, message)
        {
            ExceptionMessage = exceptionMessage;
            ExceptionStackTrace = exceptionStackTrace;
            ExceptionSource = exceptionSource;
        }
    }
}
