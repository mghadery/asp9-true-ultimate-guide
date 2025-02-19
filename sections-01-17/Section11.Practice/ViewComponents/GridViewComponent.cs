using Microsoft.AspNetCore.Mvc;

namespace Section11.Practice.ViewComponents
{
    public class GridViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string title, int x)
        {
            ViewBag.Title = title + " " + x;
            return View();
        }
    }
}
