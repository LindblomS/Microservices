namespace Management.WebApp.Pages.Catalog
{
    using Management.WebApp.Models;
    using Management.WebApp.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class UpdateModel : PageModel
    {
        readonly ICatalogService service;
        readonly ITypeService typeService;
        readonly IBrandService brandService;

        public UpdateModel(ICatalogService service, ITypeService typeService, IBrandService brandService)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.typeService = typeService ?? throw new ArgumentNullException(nameof(typeService));
            this.brandService = brandService ?? throw new ArgumentNullException(nameof(brandService));
        }

        [BindProperty]
        public UpdateCatalogItem Item { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> Types { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> Brands { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var item = await service.GetAsync(id);

            Item = new UpdateCatalogItem 
            { 
                Id = item.Id,
                AvailableStock = item.AvailableStock,
                Brand = item.Brand,
                Description = item.Description,
                Name = item.Name,
                Price = item.Price,
                Type = item.Type
            };

            return await OnGetAsync();
        }

        private async Task<IActionResult> OnGetAsync()
        {
            Types = (await typeService.GetAsync()).Select(x => new SelectListItem { Text = x, Value = x });
            Brands = (await brandService.GetAsync()).Select(x => new SelectListItem { Text = x, Value = x });
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return await OnGetAsync();

            await service.UpdateAsync(Item);
            return RedirectToPage("Index");
        }
    }
}
