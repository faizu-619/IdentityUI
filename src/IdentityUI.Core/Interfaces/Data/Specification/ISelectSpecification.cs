﻿using SSRD.IdentityUI.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SSRD.IdentityUI.Core.Interfaces.Data.Specification
{
    public interface ISelectSpecification<TEntity, TData> : IBaseSpecification<TEntity> where TEntity : class, IBaseEntity
    {
        Expression<Func<TEntity, TData>> Select { get; }
    }
}
