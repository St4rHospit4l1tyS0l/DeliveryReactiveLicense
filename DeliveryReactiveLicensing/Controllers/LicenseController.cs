﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DeliveryReactiveLicensing.Resources;
using DeliveryReactiveLicensing.Service.License;
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
    public class LicenseController : Controller
    {
        // GET: Default

        readonly ILicenseService _licenseService = new LicenseService();

        public ActionResult Index(int clId)
        {
            using (var repositoryClient = new ClientRepository())
            {
                var infoClient = repositoryClient.GetClientInfoById(clId);
                ViewBag.ClientInfo = infoClient;
                ViewBag.ClId = clId;
            }
            return View();
        }

        public ActionResult List(JqGridFilterModel opts, int clId)
        {
            using (var repository = new GenericRepository<ViewLicenseGrid>())
            {
                var result = repository.JqGridFindBy(opts, ViewLicenseGridJson.Key, ViewLicenseGridJson.Columns, 
                    (e => e.IsObsolete == false && e.ClientId == clId)
                    , ViewLicenseGridJson.DynamicToDto);
                return Json(result);
            }
        }


        [HttpPost]
        public ActionResult Upsert(int? id, int clId)
        {
            LicenseModel model = null;
            try
            {
                using (var repositoryClient = new ClientRepository())
                {
                    var infoClient = repositoryClient.GetClientInfoById(clId);
                    ViewBag.ClientInfo = infoClient;

                    if (id.HasValue)
                    {
                        var repository = new LicenseRepository(repositoryClient.DbConn);
                        model = repository.FindViewById(id.Value);
                    }
                    else
                    {
                        model = new LicenseModel
                        {
                            ClientId = clId,
                            IsActive = true,
                            LicenseId = EntityConstants.NULL_VALUE
                        };
                    }
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
        public ActionResult DoUpsert(LicenseModel model)
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


                using (var repository = new GenericRepository<License>())
                {
                    if (model.LicenseId == EntityConstants.NULL_VALUE)
                    {
                        var inModel = model.ToDataModel();
                        inModel.ActivationCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                        inModel.InsDateTime = DateTime.Now;
                        inModel.InsUserId = User.Identity.GetUserId();
                        repository.Add(inModel);
                    }
                    else
                    {
                        var oldModel = repository.FindById(model.LicenseId);
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
                SharedLogger.LogError(ex, model.LicenseId, model.ClientId, model.Name);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = ResShared.TITLE_REGISTER_FAILED,
                    Message = ResShared.ERROR_UNKOWN
                });
            }
        }

        [HttpGet]
        public ActionResult DownloadCode(int id)
        {
            try
            {
                var message = _licenseService.GetInfoForActivationCode(id);
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(message[0].Replace("<br/>", string.Empty)));
                return File(ms, "text/plain", message[1]);

            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
                //
            }

            return Json("Archivo de activación incorrecto", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DoPayment(int id)
        {
            try
            {
                using (var repository = new GenericRepository<LicensePeriod>())
                {
                    var model = repository.FindById(id);
                    if (model == null)
                    {
                        return Json(new ResponseMessageModel
                        {
                            HasError = true,
                            Title = "Confirmación del pago",
                            Message = "No se encontró el registro para confirmar el pago. <br/> Por favor actualice la información e intente de nuevo la operación."
                        });
                    }

                    model.IsPayed = true;
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
                            HasError = true,
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


        [HttpPost]
        public async Task<ActionResult> SendEmail(int id)
        {
            try
            {
                var message = _licenseService.GetInfoForActivationCode(id);

                if (message == null || message.Length != 4 || String.IsNullOrEmpty(message[3]))
                {
                    return Json(new ResponseMessageModel
                    {
                        HasError = true,
                        Title = ResShared.TITLE_LICENCE,
                        Message = "No fue posible enviar el correo electrónico, revise que el proveedor esté activo o que el correo electrónico del destinatario sea correcto"
                    });
                }

                await new EmailService().SendAsync(new IdentityMessage
                {
                    Destination = message[3],
                    Subject = "Código para la activación del sistema Advanced Delivery Management",
                    Body = _licenseService.GenerateBody(message)
                });

                return Json(new ResponseMessageModel
                {
                    HasError = false,
                    Title = ResShared.TITLE_LICENCE,
                    Message = "El correo electrónico ha sido enviado de forma exitosa"
                });
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

        [HttpPost]
        public ActionResult Period(int id, int clId)
        {
            try
            {
                ViewBag.StartDate = DateTime.Today.ToString(SharedConstants.DATE_FORMAT);
                using (var repository = new PeriodRepository())
                {
                    var infoLicenseInfo = repository.GetLicenseInfoByIdAndClientId(id, clId);
                    ViewBag.LicenseInfo = infoLicenseInfo;

                    var serializer = new JavaScriptSerializer();
                    var repositoryPeriod = new PeriodRepository(repository.DbConn);
                    ViewBag.PeriodLic = serializer.Serialize(repositoryPeriod.GetPeriodsByLicenseId(id));

                    ViewBag.LstPeriodTypes = serializer.Serialize(new CatPeriodRepository().FindAll());
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, id);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DoPeriod(LicensePeriodsModel model)
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

                var serializer = new JavaScriptSerializer();
                var lstPeriods = serializer.Deserialize<List<PeriodModel>>(model.LstPeriodsTx);

                if (lstPeriods.Count == 0)
                {
                    return Json(new ResponseMessageModel
                    {
                        HasError = true,
                        Title = ResShared.TITLE_REGISTER_FAILED,
                        Message = "No existen periodos para agregar a la licencia"
                    });
                }

                var lstPeriodsResponse = new List<PeriodResponseModel>();
                var now = DateTime.Now;
                var userId = User.Identity.GetUserId();
                using (var repository = new GenericRepository<LicensePeriod>())
                {
                    foreach (var period in lstPeriods)
                    {
                        var periodToIns = period.ToModel(model.LicenseId);
                        periodToIns.InsDateTime = now;
                        periodToIns.InsUserId = userId;
                        periodToIns.IsObsolete = false;
                        periodToIns.IsPayed = false;

                        repository.Add(periodToIns);
                    
                        lstPeriodsResponse.Add(new PeriodResponseModel
                        {
                            LicensePeriodBeforeId = period.LicensePeriodId,
                            LicensePeriodId = periodToIns.LicensePeriodId,
                            InsDateTime = periodToIns.InsDateTime.ToString(SharedConstants.DATE_FORMAT),
                            InsUserName = User.Identity.GetUserName()
                        });
                    }


                    return Json(new ResponseMessageModel
                    {
                        HasError = false,
                        Title = ResShared.TITLE_REGISTER_SUCCESS,
                        Message = String.Format("Se agregaron {0} periodo(s) a la licencia", lstPeriods.Count),
                        Data = serializer.Serialize(lstPeriodsResponse) 
                    });
                }
            }
            catch (Exception ex)
            {
                SharedLogger.LogError(ex, model.LicenseId, model.ClientId);
                return Json(new ResponseMessageModel
                {
                    HasError = true,
                    Title = ResShared.TITLE_REGISTER_FAILED,
                    Message = ResShared.ERROR_UNKOWN
                });
            }
        }


        [HttpPost]
        public ActionResult DoDeletePeriod(int id)
        {
            try
            {
                using (var repository = new GenericRepository<LicensePeriod>())
                {
                    var model = repository.FindById(id);
                    if (model == null)
                    {
                        return Json(new ResponseMessageModel
                        {
                            HasError = true,
                            Title = ResShared.TITLE_OBSOLETE_FAILED,
                            Message = ResShared.ERROR_MODEL_NOTFOUND
                        });
                    }

                    model.IsObsolete = true;
                    model.DelDateTime = DateTime.Now;
                    model.DelUserId = User.Identity.GetUserId();
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
