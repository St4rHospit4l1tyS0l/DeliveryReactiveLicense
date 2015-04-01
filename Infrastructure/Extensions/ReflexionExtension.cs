using System;
using System.Linq.Expressions;

namespace Infrastructure.Extensions
{
    public static class ReflexionExtension
    {
        public static string PropertyName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            var body = expression.Body as MemberExpression;
            return body == null ? string.Empty : body.Member.Name;
        }

    }
}
