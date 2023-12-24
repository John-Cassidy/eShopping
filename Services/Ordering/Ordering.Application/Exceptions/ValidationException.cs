using System.Linq;
using FluentValidation.Results;

namespace Ordering.Application.Exceptions;

public class ValidationException : ApplicationException
{
    // create IDictionary<string, string[]> Errors property
    public IDictionary<string, string[]> Errors { get; }

    // create ValidationException constructor
    public ValidationException()
        : base("One or more validation errors have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    // create ValidationException constructor with parameter
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failure => failure.Key, failure => failure.ToArray());
    }
}
