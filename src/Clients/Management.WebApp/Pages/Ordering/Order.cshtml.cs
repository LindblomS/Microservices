namespace Management.WebApp.Pages.Ordering
{
    using global::Ordering.Contracts.Requests;
    using Management.WebApp.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class OrderModel : PageModel
    {
        readonly OrderingService service;

        public OrderModel(OrderingService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [BindProperty]
        public GetOrder.Order Order { get; set; }

        public async Task OnGet(string id)
        {
            Order = await service.GetAsync(id);
        }
    }
}
