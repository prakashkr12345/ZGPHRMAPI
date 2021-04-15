using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HRMDataModel;
using HRMRepository.Enitities;
using HRMRepository.Repository;
using HRMRepository.IRepository;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Http.Cors;
//using System.Web.Cors;



namespace HRMAPI.Controllers
{
    [RoutePrefix("api/company")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CompanyMasterController : ApiController
    {
        #region Declaration

        private zgphrmEntities db = new zgphrmEntities();
        private ICompanyMasterRepository CompanyMasterRepository;
      
        HttpResponseMessage response = null;
        #endregion

        public CompanyMasterController()
        {

            this.CompanyMasterRepository = new CompanyMasterRepository(new zgphrmEntities());

        }

        #region This function used to show data to index page

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost, Route("GetAll")]
        public HttpResponseMessage GetAllCompnayDetail()
        {
            try
            {
                var dbResult = db.GetAllCompanyList().ToList();
                

                if (dbResult != null)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, dbResult);
                }
                else
                {
                    response = Request.CreateResponse<string>(HttpStatusCode.NotFound, "Failed");
                }
                return response;
            }
            catch (Exception ex)
            {
                
                response = Request.CreateResponse<string>(HttpStatusCode.NotFound, "Failed");

            }

            return response;

        }

        #endregion


        #region This function used to save company master data
        [HttpPost, Route("Create")]
        public HttpResponseMessage Create(CompanyMasterEntities obj)
        {
            response = null;
            var objcon = Regex.Replace(obj.objheader.name, @"\s+", "");
            try
            {

                var Message = "";
                var CompanyTable = new Companylist();
                if (obj != null)
                {

                    var Dupcity = db.Companylists.FirstOrDefault(x => x.name.Replace(" ", "") == objcon);

                    if (Dupcity == null)
                    {
                        CompanyTable.cid = obj.objheader.cid;
                        CompanyTable.code = obj.objheader.code;
                        CompanyTable.name = obj.objheader.name;
                        CompanyTable.status = obj.objheader.status  ;
                        CompanyTable.createdon = DateTime.Now.ToString();
                        CompanyMasterRepository.InsertCompanyMaster(CompanyTable);
                        CompanyMasterRepository.SaveCompanyMasterMaster();
                        Message = "1";
                        response = Request.CreateResponse<string>(HttpStatusCode.OK, Message);

                    }

                    else
                    {
                        Message = "2";
                        response = Request.CreateResponse<string>(HttpStatusCode.OK, Message);
                    }



                }
            }

            catch (Exception ex)
            {
                //_objErrorHelper.APIMethodName = "Create";
                //_objErrorHelper.APIControllerName = "CityAPIController";
                //ErrorLogger.LogError(ex, _objErrorHelper);
                response = Request.CreateResponse<string>(HttpStatusCode.NotFound, "Failed");


            }
            return response;


        }
        #endregion

        #region GetById ,Fill data after edit by CompanyId
        [HttpPost, Route("EditCompanyMasterById/{ID}")]
        public HttpResponseMessage EditCompanyMaster(int ID)
        {

            var result = db.EditCompanyList_ByID(ID).Select(x => new EditCompanyList_ByID_Result
            {
                cid = x.cid,
                code = x.code,
                name = x.name,
                status = x.status,
              

            }).ToList();

            if (result != null)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                response = Request.CreateResponse<string>(HttpStatusCode.NotFound, "Failed");
            }
            return response;
        }
        #endregion


        #region This function used to Update company master data
        [HttpPost, Route("Update")]
        public HttpResponseMessage update(CompanyMasterEntities obj)
        {
            response = null;
            var objcon = Regex.Replace(obj.objheader.name, @"\s+", "");
            try
            {
                var Message = "";
                if (obj != null)
                {
                    var Dupcon = db.Companylists.FirstOrDefault(x => (x.name.Replace(" ", "") == objcon) && (obj.objheader.Id != x.Id));

                    if (Dupcon == null)
                    {
                        var CityTable = CompanyMasterRepository.GetCompanyMasterById(obj.objheader.Id);
                       // CityTable.cid = obj.objheader.cid;
                        CityTable.code = obj.objheader.code;
                        CityTable.name = obj.objheader.name;
                        CityTable.status = obj.objheader.status;

                        CompanyMasterRepository.UpdateCompanyMaster(CityTable);
                        CompanyMasterRepository.SaveCompanyMasterMaster();
                        Message = "1";
                        response = Request.CreateResponse<string>(HttpStatusCode.OK, Message);

                    }

                    else
                    {
                        Message = "2";

                        response = Request.CreateResponse<string>(HttpStatusCode.OK, Message);
                    }


                }
            }

            catch (Exception ex)
            {
                //_objErrorHelper.APIMethodName = "Update";
                //_objErrorHelper.APIControllerName = "CityAPIController";
                //ErrorLogger.LogError(ex, _objErrorHelper);
                response = Request.CreateResponse<string>(HttpStatusCode.NotFound, "Failed");


            }
            return response;


        }
        #endregion



        [HttpPost, Route("Delete/{ID}")]
        public HttpResponseMessage Delete(int id)
        {
            response = null;
            try
            {

                Companylist updatejobRelease = db.Companylists.First(x => (x.Id == id));              
                updatejobRelease.status = "Deactive";
                db.SaveChanges();

 
            }


            catch (Exception ex)
            {
               


            }
            return response;
        }


      


    }
}
