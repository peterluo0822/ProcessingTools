﻿namespace ProcessingTools.Data.Common.Expressions
{
    using Contracts;

    using ProcessingTools.Contracts.Expressions;

    public static class ExpressionBuilder<T>
    {
        public static IUpdateExpression<T> UpdateExpression => new UpdateExpression<T>();
    }
}