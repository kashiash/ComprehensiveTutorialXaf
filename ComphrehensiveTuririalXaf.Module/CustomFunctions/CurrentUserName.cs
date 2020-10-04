using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComprehensiveTutorialXaf.Module.CustomFunctions
{
    public class CurrentUserName : ICustomFunctionOperatorBrowsable
    {
        public FunctionCategory Category
        {
            get
            {
                return FunctionCategory.Text;
            }
        }
        public string Description
        {
            get
            {
                return "CurrentUserName()\r\nPodaje nazwę zalogowanego uzytkownika";
            }
        }
        public bool IsValidOperandCount(int count)
        {
            return true;
        }
        public bool IsValidOperandType(int operandIndex, int operandCount, Type type)
        {
            return true;
        }
        public int MaxOperandCount
        {
            get
            {
                return 0;
            }
        }
        public int MinOperandCount
        {
            get
            {
                return 0;
            }
        }
        public object Evaluate(params object[] operands)
        {

            var user = (SecurityUserBase)SecuritySystem.CurrentUser;
            if (user != null)
            {
                return $"{user.UserName}";
            }

            return string.Empty;

        }
        public string Name
        {
            get
            {
                return "CurrentUserName";
            }
        }
        public Type ResultType(params Type[] operands)
        {
            return typeof(string);
        }
    }
}
