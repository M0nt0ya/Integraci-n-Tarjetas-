using System.Linq.Expressions;

using Shared.Domain.Interfaces;

namespace Shared.Infrastructure.Repository
{
    class EntityFrameworkCriteriaConverter<T>(ICriteria<T> criterias)
    {
        public IQueryable<T> ApplyCriteria(IQueryable<T> query)
        {
            try
            {
                foreach (var criteria in criterias.Filters)
                {
                    var operatorValue = GetOperatorByCriteriasFilters((IFilter)criteria);

                    query = query.Where(operatorValue);
                }

                var orderByList = criterias.OrderBy != null
                                    ? criterias.OrderBy.Cast<IOrderBy>().ToList()
                                    : [];

                query = OrderQueryByCriterias(query, orderByList);

                if (criterias.Limits.HasValue)
                {
                    query = query.Take(criterias.Limits.Value);
                }

            }
            catch (System.Exception)
            {
                throw;
            }

            return query;
        }

        public static IQueryable<T> OrderQueryByCriterias(IQueryable<T> query, List<IOrderBy>? listOrderBy)
        {
            // Si la lista de ordenamientos está vacía, ordena sin efecto.
            if (listOrderBy == null || listOrderBy.Count == 0)
            {
                return query.OrderBy(v => 0);
            }

            // Aplica cada ordenamiento en la lista.
            foreach (var orderBy in listOrderBy)
            {
                var parameter = Expression.Parameter(typeof(T), "v");
                var property = Expression.Property(parameter, orderBy.Field);
                var lambda = Expression.Lambda<Func<T, object>>(property, parameter);

                var orderByDirection = orderBy.Direction;

                query = orderByDirection switch
                {
                    "ASC" => query.OrderBy(lambda),
                    "DESC" => query.OrderByDescending(lambda),
                    _ => throw new ArgumentException($"Direction '{orderByDirection}' not supported.")
                };
            }

            return query;
        }


        public static Expression<Func<T, bool>> GetOperatorByCriteriasFilters(IFilter Filter)
        {
            var parameter = Expression.Parameter(typeof(T), "v");
            var property = Expression.Property(parameter, Filter.Field);
            ConstantExpression constant = Expression.Constant(Filter.ValueField);

            BinaryExpression binaryExpression;

            switch (Filter.OperatorField)
            {
                case "Equal":
                    // Condicion cuando el tipo de dato es DateTime
                    if (Filter.ValueField != null && DateTime.TryParse(Filter.ValueField.ToString(), out DateTime parsedDate))
                    {
                        DateTime fechaCompleta = parsedDate;

                        var hasValueProperty = Expression.Property(property, nameof(Nullable<DateTime>.HasValue));
                        var valueProperty = Expression.Property(property, nameof(Nullable<DateTime>.Value));
                        var dateProperty = Expression.Property(valueProperty, nameof(DateTime.Date));

                        var hasValueCondition = Expression.Equal(hasValueProperty, Expression.Constant(true));
                        var dateCondition = Expression.Equal(dateProperty, Expression.Constant(fechaCompleta.Date));

                        binaryExpression = Expression.AndAlso(hasValueCondition, dateCondition);
                    }
                    else
                    {
                        binaryExpression = Expression.Equal(property, constant);
                    }
                    //Console.WriteLine(binaryExpression);
                    break;
                case "GT": //GreaterThan
                    binaryExpression = Expression.GreaterThan(property, constant);
                    break;
                case "LT": //LessThan
                    binaryExpression = Expression.LessThan(property, constant);
                    break;
                case "Like":
                    var method = typeof(string).GetMethod("Contains", [typeof(string)])
                                    ?? throw new ArgumentException($"Method 'Contains' not found.");
                    MethodCallExpression methodCallExpression;
                    methodCallExpression = Expression.Call(property, method, constant);

                    return Expression.Lambda<Func<T, bool>>(methodCallExpression, parameter);

                default:
                    throw new ArgumentException($"Operator '{Filter.OperatorField}' not supported.");
            }

            return Expression.Lambda<Func<T, bool>>(binaryExpression, parameter);
        }


    }
}