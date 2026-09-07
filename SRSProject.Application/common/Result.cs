using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StockManagment.Application.common
{
    public class Result
    {
        protected Result(
            bool isSuccess,
            Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException(
                    "A successful result cannot contain an error.");
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException(
                    "A failed result must contain an error.");
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public static Result Success()
        {
            return new Result(
                true,
                Error.None);
        }

        public static Result Failure(Error error)
        {
            ArgumentNullException.ThrowIfNull(error);

            return new Result(
                false,
                error);
        }
    }
}
