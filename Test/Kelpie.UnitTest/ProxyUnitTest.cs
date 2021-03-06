﻿using Kelpie;
using Kelpie.DynamicEntity;
using Kelpie.Proxy;
using Kelpie.Proxy.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Kelpie.UnitTest
{
    [TestClass]
    public class ProxyUnitTest
    {
        private static DynamicEntityContext entityContext;
        
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            entityContext = Context.New(Effort.DbConnectionFactory.CreatePersistent("ProxyUnitTest"));
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            entityContext.Dispose();
            entityContext = null;
        }

        [TestMethod]
        [TestCategory("Proxy")]
        public void RetrieveNavigationPropertyWithLazyLoading()
        {
            Assert.IsTrue(entityContext.Configuration.LazyLoadingEnabled);
            var attribute = entityContext.ProxySet<IAttribute>("Attribute").First();

            Assert.IsNotNull(attribute.Entity);
            Assert.IsInstanceOfType(attribute.Entity, typeof(IEntity));

            Assert.IsNotNull(attribute.Type);
            Assert.IsNotNull(attribute.Type.Attributes);
            Assert.IsNotNull(attribute.Type.Attributes.FirstOrDefault());

            Assert.IsNotNull(attribute.Entity.Proxies);
            Assert.IsNotNull(attribute.Entity.Proxies.FirstOrDefault());
        }

        [TestMethod]
        [TestCategory("Proxy")]
        public void RetrieveSingleProxyEntity()
        {
            IAttributeType stringType = entityContext.ProxySet<IAttributeType>("AttributeType")
                                                     .Where(c => c.ClrName == "System.String")
                                                     .Single();

            Assert.AreEqual(stringType.ClrName, "System.String");
            Assert.AreEqual(stringType.SqlServerName, "nvarchar");
        }

        [TestMethod]
        [TestCategory("Proxy")]
        public void RetrieveIncludedProxyEntity()
        {
            var attributesUsingStringType = entityContext.ProxySet<IAttributeType>("AttributeType")
                                                            .Include(c => c.Attributes)
                                                            .Where(c => c.ClrName == "System.String")
                                                            .OrderBy(c => c.Id)
                                                            .SelectMany(c => c.Attributes)
                                                            .ToArray();

            Assert.IsInstanceOfType(attributesUsingStringType, typeof(IAttribute[]));
            Assert.AreEqual(attributesUsingStringType.Length, 8);
            Assert.IsNotNull(attributesUsingStringType.FirstOrDefault());
        }

        [TestMethod]
        [TestCategory("Proxy")]
        public void ManyToOneNavigation()
        {
            var attributeName = entityContext.ProxySet<IAttribute>("Attribute")
                                             .Include(c => c.Entity)
                                             .Where(c => c.Name == "Name" &&
                                                         c.Entity.Name == "Entity")
                                             .Single();

            Assert.AreEqual(attributeName.Name, "Name");
            Assert.IsNotNull(attributeName.Entity);
            Assert.AreEqual(attributeName.Entity.Name, "Entity");
            Assert.IsFalse(attributeName.IsNullable);
            Assert.IsTrue(attributeName.Entity.Managed);
        }

        [TestMethod]
        [TestCategory("Proxy")]
        public void OneToManyNavigation()
        {
            var entity = entityContext.ProxySet<IEntity>("Entity")
                                      .Include(c => c.Attributes)
                                      .Where(c => c.Name == "Entity")
                                      .Single();

            Assert.IsNotNull(entity.Attributes);
            Assert.AreEqual(entity.Attributes.Count, 5);

            var nameAttribute = entity.Attributes.Where(c => c.Name == "Name")
                                                 .Single();

            Assert.AreEqual(nameAttribute.Name, "Name");
        }

        [TestMethod]
        [TestCategory("Proxy")]
        public void BindedEntity()
        {
            var attributeEntity = entityContext.ProxySet<IEntity>()
                                               .Where(e => e.Name == "Attribute")
                                               .Single();

            Assert.IsInstanceOfType(attributeEntity, typeof(IEntity));
            Assert.AreEqual(attributeEntity.Name, "Attribute");
            Assert.IsTrue(attributeEntity.Managed);
        }

        [TestMethod]
        [TestCategory("Proxy")]
        public void RetrieveWithNoTracking()
        {
            IAttributeType stringType = entityContext.ProxySet<IAttributeType>("AttributeType").AsNoTracking()
                                                     .Where(c => c.ClrName == "System.String")
                                                     .Single();

            Assert.AreEqual(stringType.ClrName, "System.String");
            Assert.AreEqual(stringType.SqlServerName, "nvarchar");
        }

        [TestMethod]
        [TestCategory("Proxy")]
        public void PublishEntity()
        {
            Database.Delete("Name=DataDbContext");

            string currentPath = Assembly.GetExecutingAssembly().Location;

            AppDomainSetup setup = new AppDomainSetup()
            {
                ConfigurationFile = $"{currentPath}.config",
                ApplicationBase = Path.GetDirectoryName(currentPath),
            };
            var appDomain = AppDomain.CreateDomain("PublishEntity", null, setup);

            try
            {
                var entryPoint = (PublishingEntryPoint)appDomain.CreateInstanceFromAndUnwrap(currentPath,
                                                                            typeof(PublishingEntryPoint).FullName);
                entryPoint.Modify();
            }
            finally
            {
                AppDomain.Unload(appDomain);
            }

            using (var context = Context.New("Name=DataDbContext"))
                context.Set("PublishedEntity"); // Check PublishedEntity existance
        }

        protected class PublishingEntryPoint : MarshalByRefObject
        {
            public void Modify()
            {
                // Effort can't be used here because 
                // Effort memory can't be shared between AppDomain
                using (var context = Context.New("Name=DataDbContext"))
                {
                    var publishedEntity = context.ProxySet<IEntity>().Create();
                    publishedEntity.Name = "PublishedEntity";

                    context.ProxySet<IEntity>().Add(publishedEntity);
                    context.SaveChanges();
                }
            }
        }
    }
}
