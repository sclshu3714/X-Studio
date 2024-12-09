using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XStudio.Clouds;

namespace XStudio.Models.Requests {
    [Serializable]
    public class UploadFileRequest {

        [JsonProperty("fileInfo")]
        public UploadFileDto FileInfo { get; set; } = new UploadFileDto();

        [JsonProperty("qCloud")]
        public TencentCloudDto QCloud { get; set; } = new TencentCloudDto();
    }
}
