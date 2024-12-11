using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Clouds {
    public interface ITencentCloudService {
        Task<TencentCloudCredentialsDto?> GetCredentialsAsync();
        Task<UploadFileResultDto> UploadFileAsync(IFormFile form, TencentCloudCredentialsDto tencent, UploadFileDto file);
    }
}
