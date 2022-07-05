using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG.Core.Finance
{
    public static class EXC_RATE_TYPE
    {
        public const int PURCHASING = 1;
        public const int SALES = 2;
        public const int FINANCIAL = 3;
        public const int TAX = 4;

        /// <summary>
        /// Used to get label of related contant
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="customLabel"></param>
        /// <returns></returns>
        public static string Caption(int stat, string customLabel = "")
        {
            string result = "";

            string label = customLabel;
            switch (stat)
            {
                case EXC_RATE_TYPE.PURCHASING:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "PURCHASING" : label);
                        
                        break;
                    }
                case EXC_RATE_TYPE.SALES:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "SALES" : label);
                        break;
                    }
                case EXC_RATE_TYPE.FINANCIAL:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "FINANCIAL" : label);
                        break;
                    }
                case EXC_RATE_TYPE.TAX:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "TAX" : label);
                        break;
                    }
                default: break;
            }

            return result;
        }

    }

    public static class GLMOD
    {
        public const int PURCHASING = 10;
        public const int SALES = 20;
        public const int INVENTORY = 30;
        public const int RECEIVABLE = 40;
        public const int FIXEDASSET = 50;
        public const int FINANCIAL = 60;
        public const int TAX = 70;

        /// <summary>
        /// Used to get label of related contant
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="customLabel"></param>
        /// <returns></returns>
        public static string Caption(int stat, string customLabel = "")
        {
            string result = "";

            string label = customLabel;
            switch (stat)
            {
                case GLMOD.PURCHASING:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "PURCHASING" : label);
                        break;
                    }
                case GLMOD.SALES:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "SALES" : label);
                        break;
                    }
                case GLMOD.INVENTORY:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "INVENTORY" : label);
                        break;
                    }
                case GLMOD.RECEIVABLE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "RECEIVABLE" : label);
                        break;
                    }
                case GLMOD.FIXEDASSET:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "FIXED ASSET" : label);
                        break;
                    }
                case GLMOD.FINANCIAL:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "FINANCIAL" : label);
                        break;
                    }
                case GLMOD.TAX:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "TAX" : label);
                        break;
                    }
                default: break;
            }

            return result;
        }

    }

    public static class CALC_METHOD
    {
        public const int MULTIPLY = 1;
        public const int DIVISION = 2;

        /// <summary>
        /// Used to get label of related contant
        /// </summary>
        /// <param name="stat"></param>
        /// <param name="customLabel"></param>
        /// <returns></returns>
        public static string Caption(int stat, string customLabel = "")
        {
            string result = "";

            string label = customLabel;
            switch (stat)
            {
                case CALC_METHOD.MULTIPLY:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "MULTIPLY" : label);
                        break;
                    }
                case CALC_METHOD.DIVISION:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "DIVISION" : label);
                        break;
                    }
                default: break;
            }

            return result;
        }

    }

    public static class STATEMENT_TYPE
    {
        public const int BALANCE_SHEET = 1;
        public const int PROFIT_LOSS = 2;
        public const int CASHFLOW = 2;

    }

    public static class POSTING_PARAM
    {
        public const int AP_ADVANCE_PAYMENT = 100;
        public const int AP_WRITEOFF = 101;
        public const int AP_DISCOUNT = 102;

        public const int AR_ADVANCE_RECEIPT = 150;
        public const int AR_WRITEOFF = 151;
        public const int AR_DISCOUNT = 152;

        public const int GL_RETAIN_EARNING_CURRENT = 200;
        public const int GL_RETAIN_EARNING_PREVIOUS = 201;

        public const int FIN_ADVANCE_RECEIPT = 300;
        public const int FIN_ADVANCE_PAYMENT = 301;

    }
}
