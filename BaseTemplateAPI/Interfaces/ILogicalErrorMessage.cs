using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseTemplateAPI.Interfaces
{
    interface ILogicalErrorMessage
    {
        string Message { get; }
        void SetMessage(string message);
    }
}
