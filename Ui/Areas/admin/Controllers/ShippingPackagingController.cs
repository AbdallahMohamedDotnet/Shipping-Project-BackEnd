using BL.Contract;
using BL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ui.Helpers;

namespace Ui.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class ShippingPackagingController : Controller
    {
        private readonly IPackgingTypes _packagingTypes;

        public ShippingPackagingController(IPackgingTypes packagingTypes)
        {
            _packagingTypes = packagingTypes;
        }

        public IActionResult Index()
        {
            var data = _packagingTypes.GetAll();
            return View(data);
        }

        public IActionResult Edit(Guid? Id)
        {
            TempData["MessageType"] = null;
            var data = new DTOShipingPackging();
            if (Id != null)
            {
                data = _packagingTypes.GetById((Guid)Id);
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DTOShipingPackging data)
        {
            TempData["MessageType"] = null;
            if (!ModelState.IsValid)
                return View("Edit", data);
            try
            {
                if (data.Id == Guid.Empty)
                    _packagingTypes.Add(data);
                else
                    _packagingTypes.Update(data);
                TempData["MessageType"] = MessageTypes.SaveSucess;
            }
            catch (Exception ex)
            {
                TempData["MessageType"] = MessageTypes.SaveFailed;
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid Id)
        {
            TempData["MessageType"] = null;
            try
            {
                _packagingTypes.ChangeStatus(Id, 0);
                TempData["MessageType"] = MessageTypes.DeleteSuccess;
            }
            catch (Exception ex)
            {
                TempData["MessageType"] = MessageTypes.DeleteFailed;
            }

            return RedirectToAction("Index");
        }
    }
}