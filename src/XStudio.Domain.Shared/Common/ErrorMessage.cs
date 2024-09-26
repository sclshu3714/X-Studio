using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static OpenIddict.Abstractions.OpenIddictExceptions;
using System.Xml.Schema;

namespace XStudio.Common
{
    public static class ErrorMessage
    {
        private static readonly Dictionary<Type, string> _errorMessages;

        static ErrorMessage()
        {
            _errorMessages = new Dictionary<Type, string>() {
                { typeof(ArgumentNullException), "参数不能为空" },
                { typeof(InvalidOperationException), "操作无效" },
                { typeof(UnauthorizedAccessException), "未授权访问" },
                { typeof(ArgumentException), "参数无效" },
                { typeof(ArgumentOutOfRangeException), "参数超出范围" },
                { typeof(FormatException), "格式错误" },
                { typeof(IndexOutOfRangeException), "索引超出范围" },
                { typeof(NotSupportedException), "不支持的操作" },
                { typeof(NullReferenceException), "空引用" },
                { typeof(OverflowException), "数值超出范围" },
                { typeof(DivideByZeroException), "除数不能为零" },
                { typeof(FileNotFoundException), "文件未找到" },
                { typeof(DirectoryNotFoundException), "目录未找到" },
                { typeof(IOException), "I/O错误" },
                { typeof(StackOverflowException), "堆栈溢出" },
                { typeof(OutOfMemoryException), "内存不足" },
                { typeof(TimeoutException), "操作超时" },
                { typeof(AggregateException), "一个或多个异常发生" },
                { typeof(ConcurrencyException), "并发错误" },
                { typeof(InsufficientMemoryException), "内存不足" },
                { typeof(InvalidCastException), "无效的类型转换" },
                { typeof(InvalidDataException), "无效的数据" },
                { typeof(InvalidProgramException), "程序无效" },
                { typeof(InvalidTimeZoneException), "无效的时区" },
                { typeof(InvalidEnumArgumentException), "枚举值无效" },
                //{ typeof(InvalidOperationAttemptedException), "操作尝试无效" },
                //{ typeof(InvalidProtocolException), "协议无效" },
                { typeof(InvalidTimeZoneException), "时区无效" },
                { typeof(MemberAccessException), "成员访问异常" },
                { typeof(MethodAccessException), "方法访问异常" },
                { typeof(MissingFieldException), "缺少字段" },
                { typeof(MissingMemberException), "缺少成员" },
                { typeof(MissingMethodException), "缺少方法" },
                { typeof(NotFiniteNumberException), "非有限数字" },
                //{ typeof(NonSerializedAttributeException), "非序列化属性异常" },
                { typeof(NotImplementedException), "未实现" },
                { typeof(PlatformNotSupportedException), "平台不支持" },
                { typeof(RankException), "数组秩无效" },
                { typeof(SynchronizationLockException), "同步锁定异常" },
                { typeof(ThreadAbortException), "线程中止" },
                { typeof(ThreadInterruptedException), "线程中断" },
                { typeof(ThreadStateException), "线程状态异常" },
                { typeof(UnauthorizedAccessException), "未授权访问" },
                //{ typeof(UnauthorizedProgramException), "程序未授权" },
                { typeof(ValidationException), "验证失败" },
                //{ typeof(VariantTypeException), "变体类型异常" },
                //{ typeof(VersionParseException), "版本解析错误" },
                //{ typeof(WeakReferenceException), "弱引用异常" },
                { typeof(XmlSchemaException), "XML架构错误" },
            };
        }

        public static string GetErrorMessage(this Exception ex)
        {
            return _errorMessages.TryGetValue(ex.GetType(), out var message) ? message : "发生了一个未知错误";
        }
    }
}
