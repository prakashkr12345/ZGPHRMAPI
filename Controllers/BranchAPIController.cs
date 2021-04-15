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
    [RoutePrefix("api/branch")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BranchAPIController : ApiController
    {
        #region Declaration

        private zgphrmEntities db = new zgphrmEntities();
        private IBranchRepository BranchRepository;

        HttpResponseMessage response = null;
        #endregion

        public BranchAPIController()
        {

            this.BranchRepository = new BranchRepository(new zgphrmEntities());

        }


        #region This function used to show data to index page

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost, Route("GetAll")]
        public HttpResponseMessage GetAllCompnayDetail()
        {
            try
            {
                var dbResult = db.GetAllBranch().ToList();


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
        [HttpPost, Route("EditBranchMasterById/{ID}")]
        public HttpResponseMessage EditCompanyMaster(int ID)
        {

            var result = db.EditBranchList_ById(ID).ToList();
           

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
        public HttpResponseMessage Create(BranchMasterEntities obj)
        {
            response = null;
            var objcon = Regex.Replace(obj.objheader.branchname, @"\s+", "");
            try
            {

                var Message = "";
                var CompanyTable = new hrmBranch();
                if (obj != null)
                {

                    var Dupcity = db.hrmBranches.FirstOrDefault(x => x.branchname.Replace(" ", "") == objcon);

                    if (Dupcity == null)
                    {
                        CompanyTable.cid = obj.objheader.cid;
                        CompanyTable.code = obj.objheader.code;
                        CompanyTable.branchname = obj.objheader.branchname;
                        CompanyTable.address = obj.objheader.address;
                        CompanyTable.district = obj.objheader.district;
                        CompanyTable.state = obj.objheader.state;
                        CompanyTable.pincode = obj.objheader.pincode;
                        CompanyTable.mobileno = obj.objheader.mobileno;
                        CompanyTable.landline = obj.objheader.landline;
                        CompanyTable.email = obj.objheader.email;
                        CompanyTable.bstatus = obj.objheader.bstatus;
                        CompanyTable.createdon = DateTime.Now.ToString();
                        BranchRepository.InsertCompanyMaster(CompanyTable);
                        BranchRepository.SaveCompanyMasterMaster();
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
        public HttpResponseMessage update(BranchMasterEntities obj)
        {
            response = null;
            var objcon = Regex.Replace(obj.objheader.branchname, @"\s+", "");
            try
            {
                var Message = "";
                if (obj != null)
                {
                    var Dupcon = db.hrmBranches.FirstOrDefault(x => (x.branchname.Replace(" ", "") == objcon) && (obj.objheader.sno != x.sno));

                    if (Dupcon == null)
                    {
                        var CompanyTable = BranchRepository.GetCompanyMasterById(obj.objheader.sno);
                        CompanyTable.cid = obj.objheader.cid;
                        CompanyTable.code = obj.objheader.code;
                        CompanyTable.branchname = obj.objheader.branchname;
                        CompanyTable.address = obj.objheader.address;
                        CompanyTable.district = obj.objheader.district;
                        CompanyTable.state = obj.objheader.state;
                        CompanyTable.pincode = obj.objheader.pincode;
                        CompanyTable.mobileno = obj.objheader.mobileno;
                        CompanyTable.landline = obj.objheader.landline;
                        CompanyTable.email = obj.objheader.email;
                        CompanyTable.bstatus = obj.objheader.bstatus;
                        CompanyTable.createdon = DateTime.Now.ToString();
                        BranchRepository.InsertCompanyMaster(CompanyTable);
                        BranchRepository.SaveCompanyMasterMaster();

                        BranchRepository.UpdateCompanyMaster(CompanyTable);
                        BranchRepository.SaveCompanyMasterMaster();
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
