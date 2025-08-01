using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApplication
{
    public enum ErrorCodes
    {
        //General error codes 0 to 20
        NONE = 0,
        NOT_FOUND = 1,
        INVALID_INPUT = 2,
        UNAUTHORIZED = 3,
        INTERNAL_SERVER_ERROR = 4,
        COULD_NOT_STORE_DATA = 5,
        INVALID_PERSON_ID = 6,
        MISSING_INFORMATION = 8,

        //Tasks 21 to 40
        TASK_NOT_FOUND = 21,
        TASK_TITLE_ALREADY_EXISTS = 22,
        TASK_ALREADY_ASSOCIATED = 23,

        //Users 41 to 60
        USER_NOT_FOUND = 41,
        USER_ALREADY_EXISTS = 42,
        LOGIN_WITH_WRONG_PASSWORD = 43
    }
    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErrorCodes ErrorCode { get; set; }
        public int ErrorCodeHttp => ErrorCodeHttpMapper.ToHttpStatusCode(ErrorCode);
    }

    public static class ErrorCodeHttpMapper
    {
        public static int ToHttpStatusCode(this ErrorCodes errorCode)
        {
            return errorCode switch
            {
                ErrorCodes.USER_NOT_FOUND => StatusCodes.Status404NotFound,
                ErrorCodes.TASK_NOT_FOUND => StatusCodes.Status404NotFound,

                ErrorCodes.USER_ALREADY_EXISTS => StatusCodes.Status409Conflict,
                ErrorCodes.TASK_TITLE_ALREADY_EXISTS => StatusCodes.Status409Conflict,
                ErrorCodes.TASK_ALREADY_ASSOCIATED => StatusCodes.Status409Conflict,

                ErrorCodes.LOGIN_WITH_WRONG_PASSWORD => StatusCodes.Status401Unauthorized,
                ErrorCodes.UNAUTHORIZED => StatusCodes.Status401Unauthorized,

                ErrorCodes.INVALID_INPUT => StatusCodes.Status400BadRequest,
                ErrorCodes.MISSING_INFORMATION => StatusCodes.Status400BadRequest,

                ErrorCodes.INTERNAL_SERVER_ERROR => StatusCodes.Status500InternalServerError,

                _ => StatusCodes.Status400BadRequest
            };
        }
    }

}
