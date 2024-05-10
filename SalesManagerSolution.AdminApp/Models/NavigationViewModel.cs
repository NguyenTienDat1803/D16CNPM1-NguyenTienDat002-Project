using Microsoft.AspNetCore.Mvc.Rendering;

namespace SalesManagerSolution.AdminApp.Models
{
    public class NavigationViewModel
    {
        public List<SelectListItem> Languages { get; set; } = default!;

        public string CurrentLanguageId { get; set; } = default!;

        public string ReturnUrl { set; get; } = default!;
    }
}
