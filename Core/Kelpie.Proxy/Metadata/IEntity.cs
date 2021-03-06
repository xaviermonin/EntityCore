﻿
using System.Collections.Generic;
namespace Kelpie.Proxy.Metadata
{
    [BindedEntity(Name = "Entity")]
    public interface IEntity : IBaseEntity
    {
        string Name
        {
            get;
            set;
        }

        string Description
        {
            get;
            set;
        }

        bool Managed
        {
            get;
            set;
        }

        bool Metadata
        {
            get;
            set;
        }

        bool Association
        {
            get;
            set;
        }

        [BindedNavigationProperty]
        ICollection<IAttribute> Attributes { get; }

        [BindedNavigationProperty]
        ICollection<IProxy> Proxies { get; }
    }
}
