﻿using System.Collections.Generic;
using System.Data.Entity;
using Models = EntityCore.Initialization.Metadata.Models;

namespace EntityCore.Initialization.Metadata
{
    internal class MetadataInitializer : DropCreateDatabaseAlways<MetadataContext>
    {
        protected List<Models.AttributeType> attributesTypes = new List<Models.AttributeType>();
        protected List<Models.Entity> entities = new List<Models.Entity>();
        protected List<Models.Proxy> interfaces = new List<Models.Proxy>();

        public MetadataInitializer()
        {
            #region AttributeType

            Models.AttributeType stringType = null;
            Models.AttributeType boolType = null;
            Models.AttributeType intType = null;

            attributesTypes.Add(stringType = new Models.AttributeType()
            {
                ClrName = "System.String",
                SqlServerName = "nvarchar"
            });

            attributesTypes.Add(boolType = new Models.AttributeType()
            {
                ClrName = "System.Boolean",
                SqlServerName = "bit"
            });

            attributesTypes.Add(intType = new Models.AttributeType()
            {
                ClrName = "System.Int32",
                SqlServerName = "int"
            });

            attributesTypes.Add(new Models.AttributeType()
            {
                ClrName = "System.Decimal",
                SqlServerName = "real"
            });

            attributesTypes.Add(new Models.AttributeType()
            {
                ClrName = "System.DateTime",
                SqlServerName = "datetime"
            });

            #endregion

            #region Description of Metadata entities

            // Entity

            Models.Entity entityEntity = null;
            Models.Entity attributeTypeEntity = null;

            entities.Add(entityEntity = new Models.Entity()
            {
                Name = "Entity",
                Description = "Describe an entity",
                Attributes =
                {
                    new Models.Attribute()
                    {
                        Name = "Name",
                        IsNullable = false,
                        Type = stringType,
                        Managed = true,
                    },
                    new Models.Attribute()
                    {
                        Name = "Description",
                        IsNullable = true,
                        Type = stringType,
                        Managed = true,
                    },
                    new Models.Attribute()
                    {
                        Name = "Managed",
                        IsNullable = true,
                        Type = boolType,
                        Managed = true,
                    },
                }
            });

            // Attribute

            entities.Add(new Models.Entity()
            {
                Name = "Attribute",
                Description = "Describe an entity attribute",
                Managed = true,
                Metadata = true,
                Attributes =
                {
                    new Models.Attribute()
                    {
                        Name = "Name",
                        IsNullable = false,
                        Type = stringType,
                        Managed = true,
                        Metadata = true,
                    },
                    new Models.Attribute()
                    {
                        Name = "IsNullable",
                        IsNullable = true,
                        Type = intType,
                        Managed = true,
                        Metadata = true,
                    },
                    new Models.Attribute()
                    {
                        Name = "DefaultValue",
                        IsNullable = true,
                        Type = stringType,
                        Managed = true,
                        Metadata = true,
                    },
                    new Models.Attribute()
                    {
                        Name = "Length",
                        IsNullable = true,
                        Type = intType,
                        Managed = true,
                        Metadata = true,
                    },
                    new Models.Attribute()
                    {
                        Name = "Managed",
                        IsNullable = true,
                        Type = boolType,
                        Managed = true,
                        Metadata = true,
                    },
                }
            });

            // AttributeType

            entities.Add(attributeTypeEntity = new Models.Entity()
            {
                Name = "AttributeType",
                Description = "Describe an attribute type",
                Attributes =
                {
                    new Models.Attribute()
                    {
                        Name = "ClrName",
                        IsNullable = true,
                        Type = stringType,
                        Managed = true,
                        Metadata = true,
                    },
                    new Models.Attribute()
                    {
                        Name = "SqlServerName",
                        IsNullable = true,
                        Type = stringType,
                        Managed = true,
                        Metadata = true,
                    },
                    new Models.Attribute()
                    {
                        Name = "DefaultLength",
                        IsNullable = true,
                        Type = intType,
                        Managed = true,
                        Metadata = true,
                    },
                }
            });

            // Interface

            entities.Add(new Models.Entity()
            {
                Name = "Interface",
                Description = "Describe an interface",
                Attributes =
                {
                    new Models.Attribute()
                    {
                        Name = "FullyQualfiedTypeName",
                        IsNullable = true,
                        Type = stringType,
                        Managed = true,
                        Metadata = true,
                    },
                    new Models.Attribute()
                    {
                        Name = "Managed",
                        IsNullable = true,
                        Type = boolType,
                        Managed = true,
                        Metadata = true,
                    },
                }
            });

            #endregion

            #region Proxies

            interfaces.Add(new Models.Proxy()
            {
                Entity = attributeTypeEntity,
                Managed = true,
                FullyQualifiedTypeName = typeof(EntityCore.Proxy.Metadata.IAttributeType).AssemblyQualifiedName
            });

            #endregion

            #region Behaviors

            /*interfaces.Add(new Models.Interface()
            {
                Entity = entityEntity,
                Managed = true,
                FullyQualifiedTypeName = typeof(EntityCore.DynamicEntity.Behavior.EntityToSqlStructure).AssemblyQualifiedName,
            });*/

            #endregion
        }

        protected override void Seed(MetadataContext context)
        {
            context.AttributeTypes.AddRange(attributesTypes);
            context.Entities.AddRange(entities);
            context.Interfaces.AddRange(interfaces);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
