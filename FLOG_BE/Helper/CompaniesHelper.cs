using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Mediator;
using FLOG_BE.Model.Central;
using FLOG_BE.Model.Companies;
using FLOG_BE.Model.Companies.Entities;
using FLOG_BE.Features.Companies.AccountSegment.PostAccountSegment;
using FLOG.Core;

namespace FLOG_BE.Helper
{
    public static class Unit
    {

        public static async Task<ValidationResult> Validate(FlogContext centralContext, CompanyContext companyContext, Request request)
        {
            var newSegments = request.Body.Where(x => x.Length > 0).OrderBy(x => x.SegmentNo).ToList();
            var company = centralContext.Companies.FirstOrDefault(x => x.CompanyId == request.Initiator.CompanyId);
            if (company != null)
            {
                //check duplicate segment no
                if (newSegments.GroupBy(x => x.SegmentNo)
                    .Select(group => new { k = group.Key, c = group.Count() })
                    .Any(x => x.c > 1))
                {
                    return ValidationResult.ValidationError($"Duplicate Segment No!");
                }

                if (company.CoaTotalLength < newSegments.Sum(x => x.Length))
                {
                    return ValidationResult.ValidationError($"Maximum length is {company.CoaTotalLength}");
                }

                int prev = 0; bool isAllowed = true;
                newSegments.ForEach(x => { if (x.SegmentNo - prev != 1) isAllowed = false; prev = x.SegmentNo; });
                if (!isAllowed)
                {
                    return ValidationResult.ValidationError($"SegmentNo should be in sequence!");
                }

                return ValidationResult.Ok();
            }
            else
                return ValidationResult.ValidationError("Company not found!");
        }

    }

    public static class ActionRight
    {
        public const int BUTTON_NEW = 1;
        public const int BUTTON_EDIT = 2;
        public const int BUTTON_POSTING = 3;
        public const int BUTTON_DELETE = 4;
        public const int BUTTON_VOID = 5;
        public const int BUTTON_APPROVE = 6;
        public const int BUTTON_DISAPPROVE = 7;
        public const int BUTTON_PRINT = 8;
        public const int BUTTON_CLOSE = 9;
        public const int BUTTON_VOUCHER = 10;

        public static string Apply(FlogContext centralContext, int buttonState, string securityId, string formId)
        {
            string button = "";

            var formAccess = centralContext.SecurityRoleAccesses.Where(x => x.FormId.Equals(formId) && x.SecurityRoleId.Equals(securityId)).ToList();

            switch (buttonState)
            {
                case BUTTON_NEW:
                    {
                        button = "ACTIONNEW";

                        if (formAccess != null)
                        {
                            button = (formAccess.ElementAt(0).AllowNew ? "ACTIONNEW" : "");
                        }

                        break;
                    }
                case BUTTON_EDIT:
                    {
                        button = "ACTIONEDIT";

                        if (formAccess != null)
                        {
                            button = (formAccess.ElementAt(0).AllowEdit ? "ACTIONEDIT" : "");
                        }

                        break;
                    }
                case BUTTON_DELETE:
                    {
                        button = "ACTIONDELETE";

                        if (formAccess != null)
                        {
                            button = (formAccess.ElementAt(0).AllowDelete ? "ACTIONDELETE" : "");
                        }

                        break;
                    }
                case BUTTON_VOID:
                    {
                        button = "ACTIONVOID";

                        break;
                    }
                case BUTTON_POSTING:
                    {
                        button = "ACTIONPOSTING";

                        if (formAccess != null)
                        {
                            button = (formAccess.ElementAt(0).AllowPost ? "ACTIONPOSTING" : "");
                        }

                        break;
                    }
                case BUTTON_PRINT:
                    {
                        button = "ACTIONPRINT";

                        if (formAccess != null)
                        {
                            button = (formAccess.ElementAt(0).AllowPrint ? "ACTIONPRINT" : "");
                        }

                        break;
                    }
                case BUTTON_APPROVE:
                    {
                        button = "ACTIONAPPROVE";

                        break;
                    }
                case BUTTON_DISAPPROVE:
                    {
                        button = "ACTIONDISAPPROVE";

                        break;
                    }
                case BUTTON_CLOSE:
                    {
                        button = "ACTIONCLOSE";

                        break;
                    }
                case BUTTON_VOUCHER:
                    {
                        button = "ACTIONVOUCHER";

                        break;
                    }
                default: break;
            }

            return button;
        }
    }

    public class ActionButton
    {
        public ActionButton()
        {

        }

        public string Key { get; set; }
        public string EventKey { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public bool IsGridButton { get; set; }
    }

    public static class ImageBlob
    {
        public static string ToImageUrl(byte[] logoImageData)
        {
            string imageBase64Data = string.Empty;
            string imageDataURL = string.Empty;

            if (logoImageData != null && logoImageData.Length > 0)
            {
                imageBase64Data = Convert.ToBase64String(logoImageData);
                imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }

            return imageDataURL;
        }
    }
}
