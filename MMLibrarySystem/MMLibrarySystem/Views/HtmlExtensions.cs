using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;

using MMLibrarySystem.ViewModels;

namespace MMLibrarySystem.Views
{
    /// <summary>
    /// Defines extension methods for HTML.
    /// </summary>
    public static class HtmlExtensions
    {
        public static MvcHtmlString UserOperation(this HtmlHelper helper, UserOperation operation)
        {
            if (operation == null)
            {
                return MvcHtmlString.Create(string.Empty);
            }

            if (string.IsNullOrEmpty(operation.Action))
            {
                return MvcHtmlString.Create(operation.Label);
            }
            
            if (operation.AjaxOptions != null)
            {
                throw new InvalidOperationException("Please use AjaxUserOperation for Ajax Operations.");
            }

            var htmlAttributes = CreateConfirmation(operation.Confirmation);

            return helper.ActionLink(
                operation.Label,
                operation.Action,
                operation.Controller,
                operation.RouteValues,
                htmlAttributes);
        }

        public static MvcHtmlString AjaxUserOperation(this AjaxHelper helper, UserOperation operation)
        {
            if (operation == null)
            {
                return MvcHtmlString.Create(string.Empty);
            }
            
            if (string.IsNullOrEmpty(operation.Action))
            {
                return MvcHtmlString.Create(operation.Label);
            }
            
            if (operation.AjaxOptions != null)
            {
                throw new InvalidOperationException("Please use UserOperation for Non-Ajax Operations.");
            }

            var htmlAttributes = CreateConfirmation(operation.Confirmation);

            return helper.ActionLink(
                operation.Label,
                operation.Action,
                operation.Controller,
                operation.RouteValues,
                operation.AjaxOptions,
                htmlAttributes);
        }

        private static IDictionary<string, object> CreateConfirmation(string confirmation)
        {
            if (string.IsNullOrEmpty(confirmation))
            {
                return null;
            }

            var onClickHandler = string.Format("return confirm('{0}')", confirmation);
            var htmlAttributes = new Dictionary<string, object> { { "onclick", onClickHandler } };
            return htmlAttributes;
        }
    }
}