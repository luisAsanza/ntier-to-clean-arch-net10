using AutoMapper;
using CleanCRUDSolution.Application.Common.Enums;
using CleanCRUDSolution.Application.Features.Countries;
using CleanCRUDSolution.Application.Features.Persons;
using CleanCRUDSolution.Application.Features.Persons.DTOs;
using CleanCRUDSolution.Web.Extensions;
using CleanCRUDSolution.Web.Models.PersonModels;
using CleanCRUDSolution.Web.Models.PersonModels.Data;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;

namespace CleanCRUDSolution.Web.Controllers
{
    /// <summary>
    /// MVC controller responsible for CRUD operations and report generation for Persons.
    /// </summary>
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly IPersonReportService _personReportService;
        private readonly IMapper _mapper;
        private readonly ILogger<PersonsController> _logger;
        private readonly IValidator<PersonDataVM> _personDataValidator;

        public PersonsController(
            IPersonService personService,
            ICountriesService countriesService,
            IPersonReportService personReportService,
            IMapper mapper,
            ILogger<PersonsController> logger,
            IValidator<PersonDataVM> personDataValidator)
        {
            _personService = personService;
            _countriesService = countriesService;
            _personReportService = personReportService;
            _mapper = mapper;
            _logger = logger;
            _personDataValidator = personDataValidator;
        }

        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Index(ViewPersonsViewModel model)
        {
            var result = await _personService.GetFilteredPersonsAsync(model.SearchRequest);

            if (result.IsFailure)
            {
                ModelState.AddApplicationErrors(result.Errors, nameof(ViewPersonsViewModel.SearchRequest));
                return View(model);
            }

            model.Persons = result.Value!;
            return View(model);
        }

        [Route("create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var countries = await _countriesService.GetAllCountriesAsync();
            var model = new CreatePersonViewModel()
            {
                CountryOptions = _mapper.Map<IReadOnlyList<SelectListItem>>(countries)
            };
            return View(model);
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePersonViewModel model)
        {
            var validationResult = await _personDataValidator.ValidateAsync(model.PersonData);
            if (!validationResult.IsValid)
            {
                ModelState.AddFluentValidationErrors(validationResult, nameof(CreatePersonViewModel.PersonData));
            }

            if(!ModelState.IsValid)
            {
                var countries = await _countriesService.GetAllCountriesAsync();
                model.CountryOptions = _mapper.Map<IReadOnlyList<SelectListItem>>(countries);
                return View(model);
            }

            var request = _mapper.Map<PersonAddRequest>(model.PersonData);
            var result = await _personService.AddPersonAsync(request);

            if (result.IsFailure)
            {
                var countries = await _countriesService.GetAllCountriesAsync();
                ModelState.AddApplicationErrors(result.Errors, nameof(CreatePersonViewModel.PersonData));
                model.CountryOptions = _mapper.Map<IReadOnlyList<SelectListItem>>(countries);
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Edit(Guid personId)
        {
            var result = await _personService.GetPersonAsync(personId);

            if(result.IsFailure)
            {
                _logger.LogWarning("Person with Id {PersonId} not found.", personId);
                return RedirectToPage("Error", new { code = 404 });
            }

            var model = new UpdatePersonViewModel()
            {
                PersonData = _mapper.Map<UpdatePersonDataVM>(result.Value!),
                CountryOptions = _mapper.Map<List<SelectListItem>>(await _countriesService.GetAllCountriesAsync())
            };

            return View(model);
        }

        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePersonViewModel model)
        {
            var validationResult = await _personDataValidator.ValidateAsync(model.PersonData);
            if (!validationResult.IsValid)
            {
                ModelState.AddFluentValidationErrors(validationResult, nameof(UpdatePersonViewModel.PersonData));
            }

            if(!ModelState.IsValid)
            {
                model.CountryOptions = _mapper.Map<List<SelectListItem>>(await _countriesService.GetAllCountriesAsync());
                return View(model);
            }

            var request = _mapper.Map<PersonUpdateRequest>(model.PersonData);
            var result = await _personService.UpdatePersonAsync(request);

            if(result.IsFailure)
            {
                ModelState.AddApplicationErrors(result.Errors, nameof(UpdatePersonViewModel.PersonData));
                model.CountryOptions = _mapper.Map<List<SelectListItem>>(await _countriesService.GetAllCountriesAsync());
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("[action]/{personId}")]
        public async Task<IActionResult> Delete(Guid personId)
        {
            var result = await _personService.GetPersonAsync(personId);

            if (result.IsFailure)
            {
                return RedirectToPage("Error", new { code = 404 });
            }

            var model = new DeletePersonViewModel()
            {
                PersonData = _mapper.Map<UpdatePersonDataVM>(result.Value!),
                CountryOptions = _mapper.Map<List<SelectListItem>>(await _countriesService.GetAllCountriesAsync())
            };
            return View(model);
        }

        [HttpPost]
        [Route("[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeletePersonViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CountryOptions = _mapper.Map<List<SelectListItem>>(await _countriesService.GetAllCountriesAsync());
                return View(model);
            }

            var result = await _personService.DeletePersonAsync(model.PersonData.Id ?? Guid.Empty);

            if (result.IsFailure)
            {
                ModelState.AddApplicationErrors(result.Errors, nameof(DeletePersonViewModel.PersonData));
                model.CountryOptions = _mapper.Map<List<SelectListItem>>(await _countriesService.GetAllCountriesAsync());
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsPDF()
        {
            var allPersons = await _personService.GetAllPersonsAsync();

            return new ViewAsPdf(allPersons)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Bottom = 20,
                    Right = 20,
                    Left = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsCSV()
        {
            var result = await _personReportService.GetPersonsReportAsync(ReportOptions.CSV);

            if (result.IsFailure)
            {
                //There is no data to render the report
                return RedirectToPage("Error", new { code = 404 });
            }

            //Use application/octet-stream when we want browser to download the file
            //return File(memoryStream, "application/octet-stream", "Persons.csv");

            //Use text/csv to be more specific, sometimes the browser will try to open it in Excel
            //but most of the time will be downloaded
            return File(result.Value!, "text/csv", "Persons.csv");
        }

        [Route("[action]")]
        public async Task<IActionResult> PersonsExcel()
        {
            var result = await _personReportService.GetPersonsReportAsync(ReportOptions.Excel);

            if (result.IsFailure)
            {
                //There is no data to render the report
                return RedirectToPage("Error", new { code = 404 });
            }

            return File(result.Value!, "application/vnd.openxmlformats-officedocument", "Persons.xlsx");
        }
    }
}
