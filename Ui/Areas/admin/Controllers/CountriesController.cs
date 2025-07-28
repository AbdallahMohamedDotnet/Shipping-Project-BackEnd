using BL.Contracts;
using BL.DTOConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ui.Helpers;

namespace Ui.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountry _countryService;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(ICountry countryService, ILogger<CountriesController> logger)
        {
            _countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            try
            {
                var data = _countryService.GetAll();
                _logger.LogInformation("Retrieved {Count} countries", data?.Count ?? 0);
                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving countries list");
                TempData["MessageType"] = MessageTypes.SaveFailed;
                return View(new List<DTOCountry>());
            }
        }

        [Route("admin/Countries/Edit/{id?}")]
        public IActionResult Edit(Guid? id)
        {
            TempData["MessageType"] = null;

            try
            {
                var data = new DTOCountry();

                if (id.HasValue && id.Value != Guid.Empty)
                {
                    data = _countryService.GetById(id.Value);
                    if (data == null)
                    {
                        _logger.LogWarning("Country with ID {Id} not found", id.Value);
                        TempData["MessageType"] = MessageTypes.SaveFailed;
                        return RedirectToAction("Index");
                    }
                    _logger.LogInformation("Editing country with ID {Id}", id.Value);
                }
                else
                {
                    _logger.LogInformation("Creating new country");
                }

                return View(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading country for editing. ID: {Id}", id);
                TempData["MessageType"] = MessageTypes.SaveFailed;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Route("admin/Countries/Save")]
        [ValidateAntiForgeryToken]
        public IActionResult Save(DTOCountry data)
        {
            _logger.LogInformation("Save action called with data: Arabic={Arabic}, English={English}, Id={Id}",
                data?.CountryAname, data?.CountryEname, data?.Id);

            TempData["MessageType"] = null;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model validation failed for country save operation");
                LogModelErrors();
                return View("Edit", data);
            }

            try
            {
                bool success = false;

                if (data.Id == Guid.Empty)
                {
                    _logger.LogInformation("Creating new country: {EnglishName}", data.CountryEname);
                    success = _countryService.Add(data);

                    if (success)
                    {
                        _logger.LogInformation("Successfully created new country: {EnglishName}", data.CountryEname);
                        TempData["MessageType"] = MessageTypes.SaveSucess;
                        TempData["SuccessMessage"] = "Country created successfully!";
                    }
                    else
                    {
                        _logger.LogWarning("Failed to create new country: {EnglishName}", data.CountryEname);
                        TempData["MessageType"] = MessageTypes.SaveFailed;
                        ModelState.AddModelError("", "Failed to save country. Please try again.");
                        return View("Edit", data);
                    }
                }
                else
                {
                    _logger.LogInformation("Updating country: {Id} - {EnglishName}", data.Id, data.CountryEname);
                    success = _countryService.Update(data);

                    if (success)
                    {
                        _logger.LogInformation("Successfully updated country: {Id}", data.Id);
                        TempData["MessageType"] = MessageTypes.SaveSucess;
                        TempData["SuccessMessage"] = "Country updated successfully!";
                    }
                    else
                    {
                        _logger.LogWarning("Failed to update country: {Id}", data.Id);
                        TempData["MessageType"] = MessageTypes.SaveFailed;
                        ModelState.AddModelError("", "Failed to update country. Please try again.");
                        return View("Edit", data);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving country: {Id} - {EnglishName}", data.Id, data.CountryEname);
                TempData["MessageType"] = MessageTypes.SaveFailed;
                ModelState.AddModelError("", $"An error occurred while saving: {ex.Message}");
                return View("Edit", data);
            }

            return RedirectToAction("Index");
        }

        [Route("admin/Countries/Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            TempData["MessageType"] = null;

            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid ID provided for deletion");
                TempData["MessageType"] = MessageTypes.DeleteFailed;
                return RedirectToAction("Index");
            }

            try
            {
                bool success = _countryService.ChangeStatus(id, 0);

                if (success)
                {
                    _logger.LogInformation("Successfully deleted country: {Id}", id);
                    TempData["MessageType"] = MessageTypes.DeleteSuccess;
                }
                else
                {
                    _logger.LogWarning("Failed to delete country: {Id}", id);
                    TempData["MessageType"] = MessageTypes.DeleteFailed;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting country: {Id}", id);
                TempData["MessageType"] = MessageTypes.DeleteFailed;
            }

            return RedirectToAction("Index");
        }

        // Add Create action for clarity
        [Route("admin/Countries/Create")]
        public IActionResult Create()
        {
            _logger.LogInformation("Creating new country");
            return View("Edit", new DTOCountry());
        }

        // Add a test action to debug issues
        [HttpGet]
        [Route("admin/Countries/Test")]
        public IActionResult Test()
        {
            var testData = new DTOCountry
            {
                Id = Guid.Empty,
                CountryAname = "تست عربي",
                CountryEname = "Test English",
                CreatedDate = DateTime.Now,
                CreatedBy = Guid.NewGuid(),
                CurrentState = 1
            };

            _logger.LogInformation("Test data created: {@TestData}", testData);
            return Json(new { success = true, data = testData, message = "Test successful" });
        }

        #region Helper Methods

        /// <summary>
        /// Logs model validation errors for debugging
        /// </summary>
        private void LogModelErrors()
        {
            foreach (var modelError in ModelState)
            {
                foreach (var error in modelError.Value.Errors)
                {
                    _logger.LogWarning("Model validation error for {Field}: {Error}",
                        modelError.Key, error.ErrorMessage);
                }
            }
        }

        #endregion
    }
}
