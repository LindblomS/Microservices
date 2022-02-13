namespace Management.WebApp.Pages.Brand
{
    using Management.WebApp.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        readonly IBrandService service;

        public IndexModel(IBrandService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [BindProperty]
        public IEnumerable<string> Brands { get; set; }

        public async Task OnGet()
        {
            Brands = await service.GetAsync();
        }
    }
}
