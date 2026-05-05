using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace NRAdmanWebApplicationNet10.Helpers
{
    public static class ToastHelper
    {
        public static void Success(ITempDataDictionary tempData, string message)
        {
            tempData["ToastType"] = "success";
            tempData["ToastMessage"] = message;
        }

        public static void Error(ITempDataDictionary tempData, string message)
        {
            tempData["ToastType"] = "error";
            tempData["ToastMessage"] = message;
        }

        public static void Warning(ITempDataDictionary tempData, string message)
        {
            tempData["ToastType"] = "warning";
            tempData["ToastMessage"] = message;
        }

        public static void Info(ITempDataDictionary tempData, string message)
        {
            tempData["ToastType"] = "info";
            tempData["ToastMessage"] = message;
        }
    }
}
