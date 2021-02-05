using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ColossusFileManager.Shared.Models
{

    public class ApiResponse
    {
        public ApiResponse()
        {
            StatusCode = HttpStatusCode.OK;
        }

        public HttpStatusCode StatusCode { get; set; }

        public Dictionary<string, string> Errors { get; set; }

    }



    public class ApiResponse<T> : ApiResponse
    {

        public ApiResponse(T data)
        {
            Data = data;
            StatusCode = HttpStatusCode.OK;
        }

        public ApiResponse(T data, HttpStatusCode statusCode)
        {
            Data = data;
            StatusCode = statusCode;
        }

        public ApiResponse(T data, HttpStatusCode statusCode, Dictionary<string, string> errors)
        {
            Data = data;
            StatusCode = statusCode;
            Errors = errors;
        }

        public T Data { get; set; }

    }
}
