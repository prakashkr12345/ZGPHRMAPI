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
    [RoutePrefix("api/designation")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DesignationAPIController : ApiController
    {
        #region Declaration

        private zgphrmEntities db = new zgphrmEntities();
        private IDesignationRepository DesignationRepository;

        HttpResponseMessage response = null;
        #endregion

        public DesignationAPIController()
        {

            this.DesignationRepository = new DesignationRepository(new zgphrmEntities());

        }


        #region This function used to show data to index page

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [HttpPost, Route("GetAll")]
        public HttpResponseMessage GetAllCompnayDetail()
        {
            try
            {
                var dbResult = db.GetAllDesignation().ToList();


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
    }
}
