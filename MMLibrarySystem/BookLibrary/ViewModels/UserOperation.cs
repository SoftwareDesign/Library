using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace BookLibrary.ViewModels
{
    /// <summary>
    /// Represents an user operation which will be shown on the view.
    /// </summary>
    public class UserOperation
    {
        public UserOperation(string label, string action)
        {
            Label = label;
            Action = action;
        }

        public UserOperation(string label, string action, string controller)
            : this(label, action)
        {
            Controller = controller;
        }

        public UserOperation(string label, string action, string controller, string confirmation)
            : this(label, action, controller)
        {
            Confirmation = confirmation;
        }

        public UserOperation(string label, string action, string controller, AjaxOptions ajaxOptions)
            : this(label, action, controller)
        {
            AjaxOptions = ajaxOptions;
        }

        public string Label { get; set; }

        public string Action { get; set; }

        public string Controller { get; set; }

        public string Confirmation { get; set; }

        public RouteValueDictionary RouteValues { get; set; }

        public AjaxOptions AjaxOptions { get; set; }
    }
}