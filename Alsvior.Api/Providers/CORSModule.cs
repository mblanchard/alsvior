using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Alsvior.Api.Providers
{
    public class CORSModule : IHttpModule
    {
        public void Dispose() { }

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += delegate
            {
                if (context.Request.HttpMethod == "OPTIONS")
                {
                    var response = context.Response;
                    //response.AddHeader("Access-Control-Allow-Origin", "*");
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
            };
        }
    }
}