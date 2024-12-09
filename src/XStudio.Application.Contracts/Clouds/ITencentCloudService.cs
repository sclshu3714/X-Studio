using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Clouds {
    public interface ITencentCloudService {
        Task<TencentCloudDto?> GetCredentialsAsync();
        Task<Tuple<bool, string>> UploadFileAsync(IFormFile form, TencentCloudDto tencent, UploadFileDto file);
    }
}
