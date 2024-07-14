using System.Linq.Expressions;
using Web.Helpers;

namespace Web.Extensions;

public static class ExpressionExtensions
{
    public static Expression<Func<T, bool>> Compose<T>(this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second, Func<Expression, Expression, Expression> merge)
    {
        // Build parameter map (from parameters of second to parameters of first)
        var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] })
            .ToDictionary(p => p.s, p => p.f);

        // Replace parameters in the second lambda expression with parameters from the first
        var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

        // Apply composition of lambda expression bodies to parameters from the first expression 
        return Expression.Lambda<Func<T, bool>>(merge(first.Body, secondBody), first.Parameters);
    }
}