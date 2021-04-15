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


namespace HRMAPI.Controllers
{
    [RoutePrefix("api/dept")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DepartmentAPIController : ApiController
    {

        #region Declaration

        private zgphrmEntities db = new zgphrmEntities();
        private IDepartmentRepository DepartmentRepository;

        HttpResponseMessage response = null;
        #endregion

        public DepartmentAPIController()
        {

            this.DepartmentRepository = new DepartmentRepository(new zgphrmEntities());

        }


        #region This function used to show data to index page

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost, Route("GetAll")]
        public HttpResponseMessage GetAllCompnayDetail()
        {
            try
            {
                var dbResult = db.GetAllDepartment().ToList();


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

        #region This function used to get company list

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost, Route("GetAllCompanylist")]
        public HttpResponseMessage GetAllCompany()
        {
            try
            {
                var dbResult = db.GetCompanyListDrp().ToList();


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

        #region GetById ,Fill data after edit by CompanyId
        [HttpPost, Route("EditDeptMasterById/{ID}")]
        public HttpResponseMessage EditCompanyMaster(int ID)
        {

            var result = db.EditDepartmentList_ById(ID).ToList();


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


        #region This function used to save company master data
        [HttpPost, Route("Create")]
        public HttpResponseMessage Create(DepartmentMasterEntity obj)
        {
            response = null;
            var objcon = Regex.Replace(obj.objheader.department, @"\s+", "");
            try
            {

                var Message = "";
                var CompanyTable = new hrmDepartment();
                if (obj != null)
                {

                    var Dupcity = db.hrmDepartments.FirstOrDefault(x => x.department.Replace(" ", "") == objcon);

                    if (Dupcity == null)
                    {
                        CompanyTable.department = obj.objheader.department;
                        CompanyTable.webstatus = obj.objheader.webstatus;
                        CompanyTable.descr = obj.objheader.descr;
                        CompanyTable.position = obj.objheader.position;
                        CompanyTable.astatus = obj.objheader.astatus;
                        DepartmentRepository.InsertCompanyMaster(CompanyTable);
                        DepartmentRepository.SaveCompanyMasterMaster();
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


        #region This function used to Update company master data
        [HttpPost, Route("Update")]
        public HttpResponseMessage update(DepartmentMasterEntity obj)
        {
            response = null;
            var objcon = Regex.Replace(obj.objheader.department, @"\s+", "");
            try
            {
                var Message = "";
                if (obj != null)
                {
                    var Dupcon = db.hrmDepartments.FirstOrDefault(x => (x.department.Replace(" ", "") == objcon) && (obj.objheader.sno != x.sno));

                    if (Dupcon == null)
                    {
                        var CompanyTable = DepartmentRepository.GetCompanyMasterById(obj.objheader.sno);
                        CompanyTable.department = obj.objheader.department;
                        CompanyTable.webstatus = obj.objheader.webstatus;
                        CompanyTable.descr = obj.objheader.descr;
                        CompanyTable.position = obj.objheader.position;
                        CompanyTable.astatus = obj.objheader.astatus;


                        DepartmentRepository.UpdateCompanyMaster(CompanyTable);
                        DepartmentRepository.SaveCompanyMasterMaster();
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

                hrmBranch updatejobRelease = db.hrmBranches.First(x => (x.sno == id));
                updatejobRelease.bstatus = "Deactive";
                db.SaveChanges();


            }


            catch (Exception ex)
            {



            }
            return response;
        }

    }
}
