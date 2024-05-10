using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesManagerSolution.AdminApp.Models;

namespace eShopSolution.AdminApp.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {

        public NavigationViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = new List<LanguageVm>()
            {
                new LanguageVm()
                {
                    Id = "vi",
                    IsDefault = true,
                    Name ="VietNam"
                },
                new LanguageVm()
                {
                    Id = "en",
                    IsDefault = false,
                    Name ="English"
                },
            };

            var currentLanguageId = "vi";

            var items = languages.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = currentLanguageId == null ? x.IsDefault : currentLanguageId == x.Id.ToString()
            });

            var navigationVm = new NavigationViewModel()
            {
                CurrentLanguageId = currentLanguageId,
                Languages = items.ToList()
            };

            return View("Default", navigationVm);
        }
    }
}