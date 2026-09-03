using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_BankCustomerSystem.Models.DTO;
using 银行管理系统WPF.Models;
using 银行管理系统WPF.Utilities;

namespace 银行管理系统WPF.Repository
{
    public class CustomerRepository: IRepository<CustomerInfo>
    {
        public bool Add(CustomerInfo model)
        {
            return SqlSugarHelper.Db.Insertable(model).ExecuteCommand() > 0;
        }

        public bool Delete(int primaryKey)
        {
            var model = SqlSugarHelper.Db.Queryable<CustomerInfo>().InSingle(primaryKey);
            model.Status = 1;
            return SqlSugarHelper.Db.Updateable(model).ExecuteCommand() > 0;
        }

        public List<CustomerInfo> GetList(Expressionable<CustomerInfo> exp)
        {
            return SqlSugarHelper.Db.Queryable<CustomerInfo>().Where(exp.ToExpression()).ToList();
        }

        public List<CustomerInfo> GetListByPage(Expressionable<CustomerInfo> exp, int page, int pageSize, ref int totalCount)
        {
            return SqlSugarHelper.Db.Queryable<CustomerInfo>().Where(exp.ToExpression()).ToPageList(page, pageSize, ref totalCount);
        }

        public List<ViewCustomerInfo> GetList(Expressionable<ViewCustomerInfo> exp)
        {
            return SqlSugarHelper.Db.Queryable<ViewCustomerInfo>().Where(exp.ToExpression()).ToList();
        }

        public List<ViewCustomerInfo> GetListByPage(Expressionable<ViewCustomerInfo> exp, int page, int pageSize, ref int totalCount)
        {
            return SqlSugarHelper.Db.Queryable<ViewCustomerInfo>().Where(exp.ToExpression()).ToPageList(page, pageSize, ref totalCount);
        }

        public bool Update(CustomerInfo model)
        {
            return SqlSugarHelper.Db.Updateable(model).ExecuteCommand() > 0;
        }

    }
}
