using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using AssetsManagementEG.Context.Context;
using AssetsManagementEG.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace AssetsManagementEG.Repositories.Repositories
{
    public class GenericRepository<T> where T : class
    {
        DBSContext context;
        DbSet<T> dbset;

        public GenericRepository(DBSContext _context)
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

        //get it according to it's id
        public T FindOneForUdpdateOrDelete(int entity) 
        {
            return dbset.Find(entity);
        }

        
    }
}
