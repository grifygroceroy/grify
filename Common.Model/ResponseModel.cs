using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class ResponseModel<T> : ResponseModel
    {
        public T? Data { get; set; }
    }
    public class ResponseModel
    {
        public int Code { get; set; } = 1;
        public string Message { get; set; } = "Success";
        public Object Data { get; set; } 
    }
}
