using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlow.Communication.Responses;
public class ResponseErrorJson
{
    public List<string> ErrorMessage { get; set; }

    public ResponseErrorJson(List<string> messages)
    {
        ErrorMessage = messages;
    }

    public ResponseErrorJson(string message)
    {
        ErrorMessage = [message];
    }
}
