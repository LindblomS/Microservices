namespace Management.WebApp.Pages.Catalog
{
    using Management.WebApp.Models;
    using Management.WebApp.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Linq;

    public class CreateModel : PageModel
    {
        readonly ICatalogService service;
        readonly ITypeService typeService;
        readonly IBrandService brandService;

        public CreateModel(ICatalogService service, ITypeService typeService, IBrandService brandService)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.typeService = typeService ?? throw new ArgumentNullException(nameof(typeService));
            this.brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
        }

        [BindProperty]
        public CreateCatalogItem Item { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> Types { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> Brands { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Types = (await typeService.GetAsync()).Select(x => new SelectListItem { Text = x, Value = x });
            Brands = (await brandService.GetAsync()).Select(x => new SelectListItem { Text = x, Value = x });
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return await OnGet();

            await service.CreateAsync(Item);
            return RedirectToPage("Index");
        }
    }
}
