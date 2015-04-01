using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DeliveryReactiveLicensing.Resources;
using Infrastructure.JqGrid.Model;
using Infrastructure.Models;
using Licensing.Model.Management;
using Licensing.Repository.Catalog;
using Licensing.Repository.Database;
using Licensing.Repository.Database.Metadata;
using Licensing.Repository.Log;
using Licensing.Repository.Management;
using Licensing.Repository.Shared;
using Microsoft.AspNet.Identity;

namespace DeliveryReactiveLicensing.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        // GET: Default

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(JqGridFilterModel opts)
        {
            using (var repository = new GenericRepository<ViewClientGrid>())
            {
                var result = repository.JqGridFindBy(opts, ViewClientGridJson.Key, ViewClientGridJson.Columns, null
                    , ViewClientGridJson.DynamicToDto);
                return Json(result);
            }
        }


        [HttpPost]
        public ActionResult Upsert(int? id)
        {
            ClientModel model = null;
            try
            {
                using (var repositoryCatalog = new CountryRepository())
                {
                    if (id.HasValue)
                    {
                        var repository = new ClientRepository(repositoryCatalog.DbConn);
                        model = repository.FindViewById(id.Value);
                    }
                    else
                    {
                        model = new ClientModel
                        {
                            ClientId = EntityConstants.NULL_VALUE
                        };
                    }

                    ViewBag.CatCountry = new JavaScriptSerializer().Serialize(repositoryCatalog.FindAll());
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoUpsert(ClientModel model)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return Json(new ResponseMessageModel
                    {
                        HasError = true,
                        Title = ResShared.TITLE_REGISTER_FAILED,
                        Message = ResShared.ERROR_INVALID_MODEL
                    });
                }


                using (var repository = new GenericRepository<Client>())
                {
                    if (model.ClientId == EntityConstants.NULL_VALUE)
                    {
                        var inModel = model.ToDataModel();
                        //inModel.ActivationCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                        inModel.InsDateTime = DateTime.Now;
                        inModel.InsUserId = User.Identity.GetUserId();
                        repository.Add(inModel);
                    }
                    else
                    {
                        var oldModel = repository.FindById(model.ClientId);
                        var inModel = model.UpdateModel(oldModel, User.Identity.GetUserId());

                        repository.Update(inModel);
                    }

                    return Json(new ResponseMessageModel
                    {
                        HasError = false,
                        Title = ResShared.TITLE_REGISTER_SUCCESS,
                        Message = ResShared.INFO_REGISTER_SAVED
                    });
                }

            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, model.ClientId, model.FirstName, model.LastName);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = ResShared.TITLE_REGISTER_FAILED,
                    Message = ResShared.ERROR_UNKOWN
                });
            }
        }

        [HttpPost]
        public ActionResult DoObsolete(int id)
        {
            try
            {
                using (var repository = new GenericRepository<License>())
                {
                    var model = repository.FindById(id);
                    if (model == null)
                    {
                        return Json(new ResponseMessageModel
                        {
                            HasError = false,
                            Title = ResShared.TITLE_OBSOLETE_FAILED,
                            Message = ResShared.ERROR_MODEL_NOTFOUND
                        });
                    }

                    model.IsObsolete = true;
                    repository.Update(model);

                    return Json(new ResponseMessageModel
                    {
                        HasError = false,
                        Title = ResShared.TITLE_OBSOLETE_SUCCESS,
                        Message = ResShared.INFO_REGISTER_SAVED
                    });
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = ResShared.TITLE_OBSOLETE_FAILED,
                    Message = ResShared.ERROR_UNKOWN
                });
            }
        }
    }
}