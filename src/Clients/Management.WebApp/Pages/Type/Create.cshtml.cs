namespace Management.WebApp.Pages.Type
{
    using Management.WebApp.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.ComponentModel.DataAnnotations;

    public class CreateModel : PageModel
    {
        readonly ITypeService service;

        public CreateModel(ITypeService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [BindProperty]
        [Required, MinLength(1), MaxLength(250)]
        public string Type { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            await service.CreateAsync(Type);
            return RedirectToPage("Index");
        }
    }
}
