using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 银行管理系统WPF.Utilities
{
    //定义增删改查的公共方法
    public interface IRepository<T> where T : class, new()
    {
        bool Add(T model);
        bool Update(T model);
        bool Delete(int primaryKey);
        List<T> GetList(Expressionable<T> exp);
        List<T> GetListByPage(Expressionable<T> exp, int page, int pageSize, ref int totalCount);


    }
}
