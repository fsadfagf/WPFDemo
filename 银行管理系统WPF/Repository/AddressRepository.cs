using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_BankCustomerSystem.Models;
using WPF_BankCustomerSystem.Models.DTO;
using 银行管理系统WPF.Utilities;

namespace 银行管理系统WPF.Repository
{
    
    public class AddressRepository : IRepository<AddressInfo>
    {
        public bool Add(AddressInfo model)
        {
            return SqlSugarHelper.Db.Insertable(model).ExecuteCommand() > 0;
        }
        public bool Delete(int primaryKey)
        {
            var model = SqlSugarHelper.Db.Queryable<AddressInfo>().InSingle(primaryKey);
            if (model == null) return false;

            model.Status = 1;
            return SqlSugarHelper.Db.Updateable(model).ExecuteCommand() > 0;
        }

        public List<AddressInfo> GetList(Expressionable<AddressInfo> exp)
        {
            return SqlSugarHelper.Db.Queryable<AddressInfo>().Where(exp.ToExpression()).ToList();
        }

        public List<AddressInfo> GetListByPage(Expressionable<AddressInfo> exp, int page, int pageSize, ref int totalCount)
        {
            return SqlSugarHelper.Db.Queryable<AddressInfo>().Where(exp.ToExpression()).ToPageList(page, pageSize, ref totalCount);
        }

        public bool Update(AddressInfo model)
        {
            return SqlSugarHelper.Db.Updateable(model).ExecuteCommand() > 0;
        }
       
        public List<AddressInfo> GetAll()
        {
            return SqlSugarHelper.Db.Queryable<AddressInfo>()
                .Where(it => it.Status == 0)
                .ToList();
        }
      
        public List<ViewAddressInfo> GetListView(Expressionable<ViewAddressInfo> exp)
        {
            return SqlSugarHelper.Db.Queryable<ViewAddressInfo>().Where(exp.ToExpression()).ToList();
        }

        public List<ViewAddressInfo> GetListByPageView(Expressionable<ViewAddressInfo> exp, int page, int pageSize, ref int totalCount)
        {
            return SqlSugarHelper.Db.Queryable<ViewAddressInfo>().Where(exp.ToExpression()).ToPageList(page, pageSize, ref totalCount);
        }
    }
}
