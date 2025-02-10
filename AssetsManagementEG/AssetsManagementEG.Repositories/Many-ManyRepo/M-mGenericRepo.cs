using AssetsManagementEG.Context.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetsManagementEG.Repositories.Many_ManyRepo
{
    public class MGenericRepo<T> where T : class
    {
        DBSContext context;
        DbSet<T> dbset;

        public MGenericRepo(DBSContext _context)
        {
            context = _context;
            dbset = context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return dbset;
        }
        public bool Create(T Entity)
        {
            dbset.Add(Entity);
            context.SaveChanges();
            return true;
        }
        public void Update(T Entity)
        {
            dbset.Update(Entity);
            context.SaveChanges();
        }
        public void Delete(T Entity)
        {
            dbset.Remove(Entity);
            context.SaveChanges();
        }
        public T FindOneForUdpdateOrDelete(int entity)
        {
            return dbset.Find(entity);
        }
    }
}
