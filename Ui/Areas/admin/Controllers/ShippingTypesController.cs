using BL.Contracts;
using BL.DTOConfiguration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Ui.Helpers;
namespace Ui.Areas.admin.Controllers
{
    [Area("admin")]
    
    public class ShippingTypesController : Controller
    {
        private readonly IShippingType IShippingTypes;
        public ShippingTypesController(IShippingType shipingTypes)
        {
            this.IShippingTypes = shipingTypes;
        }
        public IActionResult Index()
        {
            var data = IShippingTypes.GetAll();
            return View(data);
        }

        public IActionResult Edit(Guid? Id)
        {
            TempData["MessageType"] = null;
            var data = new DTOShippingType();
            if (Id != null)
            {
                data = IShippingTypes.GetById((Guid)Id);
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DTOShippingType data)
        {
            TempData["MessageType"] = null;
            if (!ModelState.IsValid)
                return View("Edit", data);
            try
            {
                if (data.Id == Guid.Empty)
                    IShippingTypes.Add(data);
                else
                    IShippingTypes.Update(data);
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
                IShippingTypes.ChangeStatus(Id, 0);
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
