using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManagment.Application.common
{
    public sealed record ValidationError(
     IReadOnlyDictionary<string, string[]> Errors)
     : Error(
         "Validation.Error",
         "One or more validation errors occurred.",
         ErrorType.Validation);
}
