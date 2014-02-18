using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using We7.CMS.Common.PF;
using Thinkment.Data;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Accounts
{
    [Serializable]
    [Helper("We7.AccountHelper")]
    public partial class AccountLocalHelper : BaseHelper, IAccountHelper
    {
        public static readonly string AccountSessionKey = "We7.Session.Account.Key";

        public Account GetAccountByLoginName(string loginName)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "LoginName", loginName);
            if (Assistant.Count<Account>(c) > 0)
            {
                List<Account> accounts = Assistant.List<Account>(c, null);
                return accounts[0];
            }
            return null;
        }

        public string[] Login(string username, string password)
        {
            string[] results = { "false", ""};
            Account act = GetAccountByLoginName(username);

            if (act == null)
            {
                results[0] = "false";
                results[1] = "用户名或密码不正确!";
                return results;
            }
            if (!IsValidPassword(act, password))
            {
                results[0] = "false";
                results[1] = "用户名或密码不正确!";
                return results;
            }
            if (GeneralConfigs.GetConfig().UserRegisterMode == "email" && act.EmailValidate != 1)
            {
                results[0] = "false";
                results[1] = "该用户尚未通过Email验证!";
                return results;
            }
            if (GeneralConfigs.GetConfig().UserRegisterMode == "manual" && act.State != 1)
            {
                results[0] = "false";
                results[1] = "该用户尚未通过人工审核!";
                return results;
            }
            if (act.Overdue < DateTime.Today)
            {
                results[0] = "false";
                results[1] = "您的会员使用日期已经终止!";
                return results;
            }

            results[0] = "true";
            results[1] = act.ID;
            Security.SetAccountID(act.ID);
            return results;
        }

        public bool IsValidPassword(Account act, string password)
        {
            if (act == null)
                return false;
            if (act.PasswordHashed == (int)We7.CMS.Common.Enum.TypeOfPasswordHashed.webEncrypt)
                password = Security.Encrypt(password);
            return string.Compare(password, act.Password, false) == 0;
        }


        public List<string> GetObjectsByPermission(string accountID, string permission)
        {
            throw new NotImplementedException();
        }
    }

}
