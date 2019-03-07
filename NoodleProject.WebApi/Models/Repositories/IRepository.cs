using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoodleProject.WebApi.Models.Repositories
{
    public interface IRepository<T, TKey>
    {
        T GetOneById(TKey id);
        ICollection<T> getAll();
        T CreateOne(T parameters);
        T UpdateOne(T parameters);
        void DeleteOneById(TKey id);
    }
}
