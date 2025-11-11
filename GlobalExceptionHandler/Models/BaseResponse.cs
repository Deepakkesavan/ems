using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace GlobalExceptionHandler.Models
{
    public class BaseResponse<T>
    {
        public List<string> Warnings { get; set; } = new();
        public List<ErrorResponse> Errors { get; set; } = new();
        public T? Result { get; set; }
    }
}
