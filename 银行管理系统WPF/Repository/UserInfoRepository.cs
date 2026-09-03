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
    public class UserInfoRepository : IRepository<UserInfo>
    {
        public bool Add(UserInfo model)
        {
            return SqlSugarHelper.Db.Insertable(model).ExecuteCommand()>0;
        }

        public bool Delete(int primaryKey)
        {
            var model = SqlSugarHelper.Db.Queryable<UserInfo>().InSingle(primaryKey);
            model.Status = 1;
            return SqlSugarHelper.Db.Updateable(model).ExecuteCommand()>0;
        }

        public List<UserInfo> GetList(Expressionable<UserInfo> exp)
        {
            return SqlSugarHelper.Db.Queryable<UserInfo>().Where(exp.ToExpression()).ToList();
        }

        public List<UserInfo> GetListByPage(Expressionable<UserInfo> exp, int page, int pageSize, ref int totalCount)
        {
            return SqlSugarHelper.Db.Queryable<UserInfo>().Where(exp.ToExpression()).ToPageList(page, pageSize, ref totalCount);
        }

        public List<ViewUserInfo> GetList(Expressionable<ViewUserInfo> exp)
        {
            return SqlSugarHelper.Db.Queryable<ViewUserInfo>().Where(exp.ToExpression()).ToList();
        }

        public List<ViewUserInfo> GetListByPage(Expressionable<ViewUserInfo> exp, int page, int pageSize, ref int totalCount)
        {
            return SqlSugarHelper.Db.Queryable<ViewUserInfo>().Where(exp.ToExpression()).ToPageList(page, pageSize, ref totalCount);
        }

        public bool Update(UserInfo model)
        {
            return SqlSugarHelper.Db.Updateable(model).ExecuteCommand() > 0;
        }
    }
}
