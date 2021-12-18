using MediaIndoo_TVBox.Banco.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaIndoo_TVBox.Banco.Repositorios
{
    public class BaseRepositorio<TEntity> : IBaseRepositorio<TEntity> where TEntity : class
    {

        protected readonly BancoContext DbContext;


        public BaseRepositorio()
        {
            DbContext = new BancoContext();
        }

        public void Add(TEntity entity)
        {

            var result = DbContext.Set<TEntity>().Add(entity);

            DbContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
            DbContext.SaveChanges();
        }
        public TEntity GetById(int id)
        {
            try
            {
                var result = DbContext.Set<TEntity>().Find(id);
                return result;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;
            }

        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().ToList();
        }

        public void Delete(TEntity entity)
        {
            DbContext.Remove(entity);
            DbContext.SaveChanges();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}