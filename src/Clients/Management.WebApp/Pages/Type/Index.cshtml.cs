namespace Management.WebApp.Pages.Type
{
    using Management.WebApp.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        readonly ITypeService service;

        public IndexModel(ITypeService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [BindProperty]
        public IEnumerable<string> Types { get; set; }

        public async Task OnGet()
        {
            Types = await service.GetAsync();
        }
    }
}
