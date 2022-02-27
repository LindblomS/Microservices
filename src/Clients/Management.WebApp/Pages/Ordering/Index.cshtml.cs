namespace Management.WebApp.Pages.Ordering
{
    using global::Ordering.Contracts.Requests;
    using Management.WebApp.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class IndexModel : PageModel
    {
        readonly OrderingService service;

        public IndexModel(OrderingService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [BindProperty]
        public IEnumerable<GetOrders.Order> Orders { get; set; }

        public async Task OnGet()
        {
            Orders = await service.GetAsync();
        }
    }
}
