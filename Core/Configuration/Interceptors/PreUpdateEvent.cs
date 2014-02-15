using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using EFConvention.Interceptors;

namespace Core.Configuration.Interceptors
{
    public class PreUpdateEvent : IPreUpdateEventListener
    {
        public void OnUpdate(DbEntityEntry entityEntry)
        {
            if (entityEntry.Entity is Entity)
                entityEntry.Cast<Entity>().Entity.Modified = DateTime.Now;
        }
    }
}
