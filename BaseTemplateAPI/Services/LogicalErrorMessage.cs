using BaseTemplateAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseTemplateAPI.Services
{
    public class LogicalErrorMessage : ILogicalErrorMessage
    {
        public string Message { get; set; } = "Some Error Occured";

        public void SetMessage(string message)
        {
            Message = message;
        }
    }
}
