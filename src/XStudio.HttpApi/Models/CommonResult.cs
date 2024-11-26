using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Models {
    //public class CommonResult<T> {
    //    private HttpStatusCode _httpStatusCode = HttpStatusCode.OK;
    //    public string Code { get; set; } = "0";
    //    public string Message { get; set; } = "Success";
    //    public T? Data { get; set; } = default;

    //    public HttpStatusCode HttpStatusCode {
    //        get => _httpStatusCode;
    //        set {
    //            if (value < 0) {
    //                throw new ArgumentOutOfRangeException(nameof(value), "HTTP status code cannot be negative.");
    //            }
    //            _httpStatusCode = value;
    //        }
    //    }

    //    /// <summary>
    //    /// 为JSON字符串转换将CommonResult
    //    /// </summary>
    //    /// <param name="json"></param>
    //    /// <returns></returns>
    //    public static T? Parse(string json) {
    //        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    //    }

    //    /// <summary>
    //    /// 将Data转换为指定类型,最多支持两层，1直接将Data转换为指定类型，2如果Data是JObject，则尝试从指定keyfield中获取值并转换为指定类型
    //    /// </summary>
    //    /// <typeparam name="U"></typeparam>
    //    /// <param name="keyfield"></param>
    //    /// <returns></returns>
    //    public U? DataAs<U>(string keyfield = "data") {
    //        if (Data is JObject dataObject) {
    //            if (dataObject.ContainsKey(keyfield) && dataObject[keyfield] is JObject fieldObject) {
    //                return fieldObject.ToObject<U>();
    //            }
    //            else {
    //                return dataObject.ToObject<U>();
    //            }
    //        }
    //        return default;
    //    }
    //}

    [Serializable]
    public class CommonResult<T> : ActionResult {

        [Required]
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
        public string Code { get; set; } =  "0";
        public string Message { get; set; } = "Success";
        public T? Data { get; set; }

        public CommonResult() { }

        public CommonResult(HttpStatusCode httpStatusCode, string code, string message, T? data) {
            HttpStatusCode = httpStatusCode;
            Code = code;
            Message = message;
            Data = data;
        }

        public static CommonResult<T> Success() {
            return Success(default);
        }

        public static CommonResult<T> Success(T? data) {
            return new CommonResult<T>(HttpStatusCode.OK, CommonEnum.SUCCESS.GetCode(), CommonEnum.SUCCESS.GetMessage(), data);
        }

        public static CommonResult<T> Fail() {
            return Fail(CommonEnum.FAILED.GetMessage());
        }

        public static CommonResult<T> Fail(string msg) {
            return Fail(HttpStatusCode.InternalServerError, CommonEnum.FAILED.GetCode(), msg);
        }

        public static CommonResult<T> Fail(IBaseError resultCode) {
            return Fail(HttpStatusCode.InternalServerError, resultCode);
        }

        public static CommonResult<T> Fail(HttpStatusCode httpStatus, IBaseError resultCode) {
            return Fail(httpStatus, resultCode.Code, resultCode.Message, default);
        }

        public static CommonResult<T> Fail(HttpStatusCode httpStatus, IBaseError resultCode, T? data) {
            return Fail(httpStatus, resultCode.Code, resultCode.Message, data);
        }

        public static CommonResult<T> Fail(HttpStatusCode httpStatus, string code, string message) {
            return new CommonResult<T>(httpStatus, code, message, default);
        }

        public static CommonResult<T> Fail(HttpStatusCode httpStatus, string code, string message, T? data) {
            return new CommonResult<T>(httpStatus, code, message, data);
        }

        public static CommonResult<T> InvalidParameterReturnMessage(string message) {
            return new CommonResult<T>(HttpStatusCode.BadRequest, CommonEnum.FAILED.GetCode(), message, default);
        }

        public static CommonResult<T> InvalidParameterReturnMessage(string message, T? data) {
            return new CommonResult<T>(HttpStatusCode.BadRequest, CommonEnum.FAILED.GetCode(), message, data);
        }

        public static CommonResult<T> NotFound(IBaseError resultCode) {
            return new CommonResult<T>(HttpStatusCode.BadRequest, resultCode.Code, resultCode.Message, default);
        }

        public static CommonResult<T> NoContent() {
            return new CommonResult<T>(HttpStatusCode.NoContent, CommonEnum.SUCCESS.GetCode(), CommonEnum.SUCCESS.GetMessage(), default);
        }

        public override bool Equals(object? obj) {
            if (obj is CommonResult<T> other) {
                return HttpStatusCode == other.HttpStatusCode &&
                       Code == other.Code &&
                       Message == other.Message &&
                       EqualityComparer<T>.Default.Equals(Data, other.Data);
            }
            return false;
        }

        public override int GetHashCode() {
            return HashCode.Combine(HttpStatusCode, Code, Message, Data);
        }

        public override string ToString() {
            return $"CommonResult(HttpStatusCode={HttpStatusCode}, Code={Code}, Message={Message}, Data={Data})";
        }
    }


    public interface IBaseError {
        string Code { get; set; }
        string Message { get; set; }
    }

    public enum CommonEnum {
        SUCCESS = 0,
        FAILED = -1,
        UNAUTHORIZED = 1,
        NOT_FOUND,
        METHOD_NOT_ALLOWED,
        UNSUPPORTED_MEDIA_TYPE,
        EXCEPTION,
        TRAFFIC_LIMITING,
        API_GATEWAY_ERROR,
        PARAM_ERROR,
        PARAM_FORMAT_ERROR,
        BUSINESS_ERROR,
        ILLEGAL_REQUEST,
        RPC_ERROR
    }

    public static class CommonEnumExtensions {
        public static string GetCode(this CommonEnum commonEnum) {
            return commonEnum switch {
                CommonEnum.SUCCESS => "0",
                CommonEnum.FAILED => "-1",
                CommonEnum.UNAUTHORIZED => "CLOUD-401",
                CommonEnum.NOT_FOUND => "CLOUD-404",
                CommonEnum.METHOD_NOT_ALLOWED => "CLOUD-405",
                CommonEnum.UNSUPPORTED_MEDIA_TYPE => "CLOUD-415",
                CommonEnum.EXCEPTION => "CLOUD-500",
                CommonEnum.TRAFFIC_LIMITING => "CLOUD-429",
                CommonEnum.API_GATEWAY_ERROR => "API-9999",
                CommonEnum.PARAM_ERROR => "CLOUD-100",
                CommonEnum.PARAM_FORMAT_ERROR => "CLOUD-200",
                CommonEnum.BUSINESS_ERROR => "CLOUD-400",
                CommonEnum.ILLEGAL_REQUEST => "CLOUD-ILLEGAL_REQUEST",
                CommonEnum.RPC_ERROR => "RPC-510",
                _ => throw new ArgumentOutOfRangeException(nameof(commonEnum), commonEnum, null)
            };
        }

        public static string GetMessage(this CommonEnum commonEnum) {
            return commonEnum switch {
                CommonEnum.SUCCESS => "成功",
                CommonEnum.FAILED => "服务器错误",
                CommonEnum.UNAUTHORIZED => "无权限访问",
                CommonEnum.NOT_FOUND => $"哎呀，无法找到这个资源啦({HttpStatusCode.NotFound})",
                CommonEnum.METHOD_NOT_ALLOWED => $"请换个姿势操作试试({HttpStatusCode.MethodNotAllowed})",
                CommonEnum.UNSUPPORTED_MEDIA_TYPE => $"呀，不支持该媒体类型({HttpStatusCode.UnsupportedMediaType})",
                CommonEnum.EXCEPTION => "服务器开小差，请稍后再试",
                CommonEnum.TRAFFIC_LIMITING => "哎呀，网络拥挤请稍后再试试",
                CommonEnum.API_GATEWAY_ERROR => "网络繁忙，请稍后再试",
                CommonEnum.PARAM_ERROR => "参数错误",
                CommonEnum.PARAM_FORMAT_ERROR => "参数格式错误",
                CommonEnum.BUSINESS_ERROR => "业务异常",
                CommonEnum.ILLEGAL_REQUEST => "非法请求",
                CommonEnum.RPC_ERROR => "呀，网络出问题啦！",
                _ => throw new ArgumentOutOfRangeException(nameof(commonEnum), commonEnum, null)
            };
        }
    }
}
