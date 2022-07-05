using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLOG.Core
{
    public static class DOCSTATUS
    {
        public const int INACTIVE = 0;
        public const int NEW = 1;
        public const int REVISE = 2;
        public const int POST = 8;
        public const int DELETE = 9;        
        public const int PROCESS = 10;
        public const int APPROVE = 31;
        public const int DISAPPROVE = 33;
        public const int CANCEL = 44;
        public const int VOID = 55;
        public const int CLOSE = 80;
        public const int OPEN = 20;
        public const int SUBMIT = 21;
        public const int CONFIRM = 200;
        public const int EXPIRE = 408;
        public const int COMPLETE = 210;

        public static string Caption(int stat, string customLabel = "")
        {
            string result = "";

            string label = customLabel;
            switch (stat)
            {
                case DOCSTATUS.INACTIVE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Inactive" : label);
                        break;
                    }
                case DOCSTATUS.NEW:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "New" : label);
                        break;
                    }
                case DOCSTATUS.REVISE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Revise" : label);
                        break;
                    }
                case DOCSTATUS.POST:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Posted" : label);
                        break;
                    }                
                case DOCSTATUS.DELETE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Deleted" : label);
                        break;
                    }
                case DOCSTATUS.APPROVE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Approved" : label);
                        break;
                    }
                case DOCSTATUS.DISAPPROVE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "Disapproved" : label);
                        break;
                    }
                case DOCSTATUS.VOID:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "VOID" : label);
                        break;
                    }
                case DOCSTATUS.CLOSE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "CLOSED" : label);
                        break;
                    }
                case DOCSTATUS.OPEN:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "OPEN" : label);
                        break;
                    }
                case DOCSTATUS.SUBMIT:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "SUBMITTED" : label);
                        break;
                    }
                case DOCSTATUS.CANCEL:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "CANCELED" : label);
                        break;
                    }
                case DOCSTATUS.CONFIRM:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "CONFIRMED" : label);
                        break;
                    }
                case DOCSTATUS.EXPIRE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "EXPIRED" : label);
                        break;
                    }
                case DOCSTATUS.COMPLETE:
                    {
                        result = (string.IsNullOrEmpty(customLabel) ? "COMPLETED" : label);
                        break;
                    }
                default: break;
            }

            return result;
        }
    }

    public static class TRX_MODULE
    {
        public const int TRX_PAYABLE = 1;
        public const int TRX_RECEIVABLE = 2;
        public const int TRX_CHECKBOOK = 3;
        public const int TRX_GENERAL_JOURNAL = 4;
        public const int TRX_ASSET = 5;
        public const int TRX_INVENTORY = 6;
        public const int TRX_PAYMENT = 7;
        public const int TRX_RECEIPT = 8;
        public const int TRX_APPLY_PAYABLE = 9;
        public const int TRX_APPLY_RECEIPT = 10;
        public const int TRX_BANK_RECONCILE = 11;
        public const int TRX_SHIPPING = 12;
        
        /*CONTAINER RENT 13-19 */
        public const int TRX_CONTAINER_RENT = 13;
        public const int TRX_CONTAINER_RENTAL_REQUEST = 14;
        public const int TRX_DELIVERY_ORDER_CONFIRM = 15;

        /*SALES 20-29 */

        public const int TRX_SALES = 20;
        public const int TRX_SALES_QUOTATION = 21;
        public const int TRX_SALES_ORDER = 22;
        public const int TRX_SALES_NEGO = 23;

        public const int TRX_DEPOSIT = 200;

        public static Dictionary<int, string> GetAll()
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();

            dict.Add(TRX_SHIPPING, "SHIPPING");
            dict.Add(TRX_CONTAINER_RENT, "CONTAINER RENT");

            //dict.Add(TRX_PAYABLE, "PAYABLE");
            //dict.Add(TRX_RECEIVABLE, "RECEIVABLE");
            //dict.Add(TRX_DEPOSIT, "DEPOSIT");
            //dict.Add(TRX_CHECKBOOK, "CHECKBOOK");

            //dict.Add(TRX_CONTAINER_RENTAL_REQUEST, "CONTAINER RENTAL REQUEST");
            //dict.Add(TRX_DELIVERY_ORDER_CONFIRM, "DELIVERY ORDER CONFIRM");
            //dict.Add(TRX_SALES_ORDER, "SALES ORDER");
            //dict.Add(TRX_SALES_QUOTATION, "SALES QUOTATION");

            return dict;
        }
    }

    public static class DOCUMENTTYPE
    {
        public const string INVOICE = "INVOICE";
        public const string DEBIT_NOTE = "DEBIT NOTE";
        public const string CREDIT_NOTE = "CREDIT NOTE";

        public const string DEPOSIT_DEMURRAGE = "DEMURRAGE";
        public const string CONTAINER_GUARANTEE = "CONTAINER GUARANTEE";
        public const string DETENTION = "DETENTION";
        

        public const string CHECKBOOK_IN = "IN";
        public const string CHECKBOOK_OUT = "OUT";

        public const string CONTAINER_RENTAL_REQUEST = "CONTAINER RENTAL REQUEST";
        public const string DELIVERY_ORDER_CONFIRM = "DELIVERY ORDER CONFIRM";

        public const string ADVANCE = "ADVANCE";
        public const string RECEIPT = "RECEIPT";
        public const string PAYMENT = "PAYMENT";

        public const string DEPOSIT_SETTLEMENT = "DEPOSIT SETTLEMENT";

    }

    public static class ROUNDING_TYPE
    {
        public const int UP = 0;
        public const int DOWN = 1;
        public const int COMMON = 2;
    }
    public static class CALC
    {
        /// <summary>
        /// Get Functional Amount
        /// </summary>
        /// <param name="isMultiply"></param>
        /// <param name="originating"></param>
        /// <param name="exchangeRate"></param>
        /// <returns></returns>
        public static async Task<decimal> FunctionalAmountAsync(bool isMultiply = true, decimal originating = 0, decimal exchangeRate = 0)
        {
            decimal val = 0;

            if(originating > 0 && exchangeRate > 0)
            {
                if (isMultiply)
                {
                    val = (originating * exchangeRate);
                }
                else
                {
                    val = (originating / exchangeRate);
                }
            }            
            
            return val;

        }

        /// <summary>
        /// Get Functional Amount
        /// </summary>
        /// <param name="isMultiply"></param>
        /// <param name="originating"></param>
        /// <param name="exchangeRate"></param>
        /// <returns></returns>
        public static decimal FunctionalAmount(bool isMultiply = true, decimal originating = 0, decimal exchangeRate = 0, FLOG_BE.Model.Companies.Entities.TaxSchedule taxProfile = null)
        {
            decimal val = 0;

            if (originating > 0 && exchangeRate > 0)
            {
                if (isMultiply)
                {
                    val = (originating * exchangeRate);
                }
                else
                {
                    val = (originating / exchangeRate);
                }

                if(taxProfile != null)
                {
                    if(taxProfile.RoundingLimitAmount > 0)
                    {
                        if (taxProfile.RoundingType == ROUNDING_TYPE.UP)
                        {
                            val = Math.Ceiling(val / taxProfile.RoundingLimitAmount) * taxProfile.RoundingLimitAmount;
                        }
                        else if (taxProfile.RoundingType == ROUNDING_TYPE.DOWN)
                        {
                            val = Math.Floor(val / taxProfile.RoundingLimitAmount) * taxProfile.RoundingLimitAmount;
                        }
                        else
                        {
                            val = Math.Round(val / taxProfile.RoundingLimitAmount, MidpointRounding.AwayFromZero) * taxProfile.RoundingLimitAmount;
                        }
                    }                    
                }
            }

            return val;

        }

    }

    public static class DATE_FORMAT
    {
        public const string YMD_TIME = "yyyy-MM-dd HH:mm:ss";
        public const string YMD = "yyyy-MM-dd";
    }
}
