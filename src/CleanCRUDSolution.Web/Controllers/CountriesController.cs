using CleanCRUDSolution.Application.Common.Configuration;
using CleanCRUDSolution.Application.Features.Countries;
using CleanCRUDSolution.Web.Extensions;
using CleanCRUDSolution.Web.Models.CountryModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CleanCRUDSolution.Web.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {        
        private readonly IBulkUploadCountriesService _bulkUploadCountriesService;
        public CountriesController(IBulkUploadCountriesService bulkUploadCountriesService)
        {
            _bulkUploadCountriesService = bulkUploadCountriesService;
        }

        [HttpGet("UploadFromExcel")]
        public IActionResult Index()
        {
            return View(new UploadCountriesViewModel());
        }

        [HttpPost("UploadFromExcel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UploadCountriesViewModel model,
            IOptionsSnapshot<ExcelSettings> excelOptions,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.FormFile?.Length > excelOptions.Value.MaxSizeInBytes)
            {
                ModelState.AddModelError(nameof(model.FormFile),
                    $"File max size is {excelOptions.Value.MaxFileSizeInMB} MB.");
                return View(model);
            }

            var extension = Path.GetExtension(model.FormFile?.FileName?.ToLower());

            if (!excelOptions.Value.AllowedExtensions.Contains(extension))
            {
                var allowedExtensionsString = string.Join(" ", excelOptions.Value.AllowedExtensions);
                ModelState.AddModelError(nameof(model.FormFile),
                    $"Incorrect extension file. Allowed extensions are: {allowedExtensionsString}.");
                return View(model);
            }

            await using var stream = model.FormFile!.OpenReadStream();
            var result = await _bulkUploadCountriesService.UploadCountriesAsync(stream, ct);

            if (result.IsFailure)
            {
                ModelState.AddApplicationErrors(result.Errors);
                return View(model);
            }

            model.PersistedItemsCount = result.Value!.Items.Count;
            model.ReportOutcome = result.Value?.Outcome.ToString();
            model.ReportOutcomeReason = result.Value!.OutcomeReason;

            return View("Index", model);
        }
    }
}
