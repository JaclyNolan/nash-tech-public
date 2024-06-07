using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Core_Assignment1.Application.Exceptions
{
    public class BorrowingValidationException : Exception
    {
        public BorrowingValidationException(string message) : base(message)
        {
        }
    }

    public class BorrowedDateMustBeBeforeReturnedException : BorrowingValidationException
    {
        public BorrowedDateMustBeBeforeReturnedException() : base("Borrowed date must be before the returned date.")
        {
        }
    }

    public class DuplicateBookIdException : BorrowingValidationException
    {
        public DuplicateBookIdException() : base("Duplicate BookIds are not allowed.")
        {
        }
    }
}
