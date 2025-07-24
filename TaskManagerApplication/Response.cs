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
        None = 0,
        NotFound = 1,
        InvalidInput = 2,
        Unauthorized = 3,
        InternalServerError = 4,
        COULD_NOT_STORE_DATA = 5,
        INVALID_PERSON_ID = 6,
        MISSING_INFORMATION = 8,

        //Tasks 100 to 199
        TASK_NOT_FOUND = 100,

        //Users 200 to 299
        USER_NOT_FOUND = 200,
    }
    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErrorCodes ErrorCode { get; set; }
    }
}
