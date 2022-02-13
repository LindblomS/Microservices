namespace Management.WebApp.Pages.Catalog
{
    using global::Catalog.Contracts.Queries;
    using Management.WebApp.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        readonly ICatalogService service;

        public IndexModel(ICatalogService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [BindProperty]
        public IEnumerable<Item> Items { get; private set; }

        public async Task OnGet()
        {
            Items = await service.GetAsync();
        }
    }
}
