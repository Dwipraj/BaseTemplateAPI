using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseTemplateAPI.Errors
{
	public class ApiResponse
	{
		public ApiResponse(int statusCode, string message = null)
		{
			StatusCode = statusCode;
			Message = message ?? GetDefaultMessageForStatusCode(statusCode);
		}

		public int StatusCode { get; set; }
		public string Message { get; set; }

		private string GetDefaultMessageForStatusCode(int statusCode)
		{
			return statusCode switch
			{
				400 => "Bad Request"/*"A bad request, you have made"*/,
				401 => "Unauthorized Access"/*"Authorized, you are not"*/,
				404 => "Resource not Found"/*"Resource found, it was not"*/,
				403 => "Not Eligible"/*"Eligible, you are not"*/,
				500 => "Server Error"/*"Errors are the path to the dark side. Error leads to anger. Anger leads to hate. Hate leads to career change."*/,
				_ => null
			};
		}
	}
}
