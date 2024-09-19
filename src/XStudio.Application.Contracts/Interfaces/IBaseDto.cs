using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.Common;

namespace XStudio.Interfaces
{
    public interface IBaseDto
    {
        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// 加密模式
        /// </summary>
        public EncryptionMode @Mode { get; set; }

        /// <summary>
        /// 秘钥
        /// </summary>
        public string EncryptionKey { get; set; }
    }
}
