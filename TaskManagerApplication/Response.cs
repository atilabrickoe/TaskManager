using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApplication
{
    public enum ErrorCodes
    {
        //General error codes 0 to 99
        NONE = 0,
        NOT_FOUND = 1,
        INVALID_INPUT = 2,
        UNAUTHORIZED = 3,
        INTERNAL_SERVER_ERROR = 4,
        COULD_NOT_STORE_DATA = 5,
        INVALID_PERSON_ID = 6,
        MISSING_INFORMATION = 8,

        //Tasks 100 to 199
        TASK_NOT_FOUND = 100,
        TASK_TITLE_ALREADY_EXISTS = 101,
        TASK_ALREADY_ASSOCIATED = 102,
        TASK_ASSOCIATED_CAN_NOT_BE_DELETED = 103,

        //Users 200 to 299
        USER_NOT_FOUND = 200,
        INFO_USER_C
    }
    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErrorCodes ErrorCode { get; set; }
    }
}
