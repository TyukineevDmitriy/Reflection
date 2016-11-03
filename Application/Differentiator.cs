using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Application
{
    public static class Differentiator
    {
        public static Expression<Func<double, double>> Differentiate(Expression expression)
        {
            if (expression is LambdaExpression)
            {
                var lambda = (LambdaExpression)expression;
                if (lambda.Parameters.Count != 1)
                    throw new ArgumentException("Lambda must have 1 parameter");
                return Differentiate(lambda.Body);
            }
            if (expression is BinaryExpression)
            {
                if (expression.NodeType == ExpressionType.Add)
                {
                    var addition = (BinaryExpression)expression;
                    var left = Differentiate(addition.Left).Body;
                    var right = Differentiate(addition.Right).Body;
                    return Expression.Lambda<Func<double, double>>(Expression.Add(left, right), AssignName());
                }
                if (expression.NodeType == ExpressionType.Multiply)
                {
                    var multiplication = (BinaryExpression)expression;
                    var left = Expression.Multiply(Differentiate(multiplication.Left).Body, multiplication.Right);
                    var right = Expression.Multiply(multiplication.Left, Differentiate(multiplication.Right).Body);
                    return Expression.Lambda<Func<double, double>>(Expression.Add(left, right), AssignName());
                }
                else
                {
                    throw new ArgumentException("Binary Expression must be only addition or multiplication");
                }
            }
            if (expression is MethodCallExpression)
            {
                if (((MethodCallExpression)expression).Method.Name != "Sin")
                    throw new ArgumentException("Lambda must contain only multiplication, addiction or sine operation");
                var sin = ((MethodCallExpression)expression).Arguments[0];
                var cos = Expression.Call(typeof(Math).GetMethod("Cos"), sin);
                var exp = Differentiate(sin).Body;
                return Expression.Lambda<Func<double, double>>(Expression.Multiply(cos, exp), AssignName());
            }
            if (expression is ParameterExpression)
            {
                return x => 1;
            }
            if (expression is ConstantExpression)
            {
                return x => 0;
            }
            else
            {
                throw new ArgumentException("Lambda must contain only multiplication, addiction or sine operation");
            }
        }
        private static ParameterExpression AssignName()
        {
            return Expression.Parameter(typeof(double), "x");
        }
    }
}
