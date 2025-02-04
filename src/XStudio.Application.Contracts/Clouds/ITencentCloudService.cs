using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Clouds {
    public interface ITencentCloudService {

        Task<TencentCloudCredentialsDto?> GetCredentialsAsync();
   
        Task<UploadFileResultDto> UploadFileAsync(IFormFile form, TencentCloudCredentialsDto tencent, UploadFileDto file);

        Task<Tuple<bool, object>> RefreshDownloadUrl(TencentCloudCredentialsDto tencent, DownloadDto refresh);

        Task<DownloadResultDto> DownloadObject(TencentCloudCredentialsDto tencent, DownloadDto download);

        Task<FileStreamResult?> DownloadToMemory(TencentCloudCredentialsDto tencent, DownloadDto download);
    }
}
