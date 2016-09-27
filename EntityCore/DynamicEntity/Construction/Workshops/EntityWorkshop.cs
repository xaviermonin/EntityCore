﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Models = EntityCore.Initialization.Metadata.Models;

namespace EntityCore.DynamicEntity.Construction.Workshops
{
    abstract class EntityWorkshop<TResult> : IEntityWorkshop where TResult : EntityWorkshopResult
    {
        protected EntityFactory Factory { get; private set; }
        private Dictionary<string, TResult> resultsByEntityName;

        public EntityWorkshop(EntityFactory factory)
        {
            this.Factory = factory;
            this.resultsByEntityName = new Dictionary<string, TResult>();
        }

        protected abstract TResult DoWork(Models.Entity entity, TypeBuilder typeBuilder);

        private void AddResults(Models.Entity entity, TResult result)
        {
            resultsByEntityName.Add(entity.Name, result);
        }

        public TResult GetResults(Models.Entity entity)
        {
            return resultsByEntityName[entity.Name];
        }

        public IEnumerable<TResult> GetResults()
        {
            return resultsByEntityName.Values;
        }

        public TResult GetResults(string entityName)
        {
            return resultsByEntityName[entityName];
        }

        public void DoWork(IEnumerable<JobBag> jobsBags)
        {
            foreach (var jb in jobsBags.AsParallel())
                AddResults(jb.Entity, DoWork(jb.Entity, jb.Type));
        }
    }
}
