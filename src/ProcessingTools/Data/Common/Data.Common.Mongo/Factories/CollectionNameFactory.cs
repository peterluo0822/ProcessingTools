﻿namespace ProcessingTools.Data.Common.Mongo.Factories
{
    using System;
    using System.Linq;
    using Attributes;

    public static class CollectionNameFactory
    {
        public static string Create(Type entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            var collectioNameAttribute = entityType.GetCustomAttributes(typeof(CollectionNameAttribute), false)?.SingleOrDefault() as CollectionNameAttribute;

            if (collectioNameAttribute != null)
            {
                return collectioNameAttribute.Name;
            }
            else
            {
                string name = entityType.Name.ToLower();
                int nameLength = name.Length;
                if (name.ToCharArray()[nameLength - 1] == 'y')
                {
                    return $"{name.Substring(0, nameLength - 1)}ies";
                }
                else
                {
                    return $"{name}s";
                }
            }
        }

        public static string Create<TEntity>()
        {
            return Create(typeof(TEntity));
        }
    }
}
