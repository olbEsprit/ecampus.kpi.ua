﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Site.EIR.IrGroup
{
    public partial class Default : Core.SitePage
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            AutoLogin();
            if (SessionId != null)
            {
                var client = new Campus.SDK.Client();
                var url = Campus.SDK.Client.BuildUrl("IrGroup", "GetAllPrivateIrGroups", new { SessionId });
                var result = client.Get(url);
                
                var inner = JsonConvert.DeserializeObject(result.Data.ToString());

                if (inner != null)
                {
                    var items = (inner as IEnumerable<Object>);
                    var groups = items.Cast<JObject>().ToList();

                    foreach (var group in groups)
                    {
                        IrGroupListRendering(group);
                    }
                }

            }
            else
            {
                var mainDiv = new HtmlGenericControl("div");
                CreateErrorMessage(mainDiv);
                LinkContainer.Controls.Add(mainDiv);
            }
        }

        public void AutoLogin()
        {
            //TODO debuging staff - delete!!!!!!!
            var client = new Campus.SDK.Client();
            client.Authenticate("123", "123");
            SessionId = client.SessionId;
        }

        private void IrGroupListRendering(JObject group)
        {
            var groupLink = new LinkButton();
            var mainDiv = new HtmlGenericControl("div");
            var name = new HtmlGenericControl("h5");
            var description = new HtmlGenericControl("p");
            

            groupLink.PostBackUrl = Request.Url.AbsolutePath;
            groupLink.Attributes.Add("class", "irGroupLink");
            groupLink.Attributes.Add("irGroupId", group["IrGroupId"].ToString());

            mainDiv.Attributes.Add("id", "mainBlock");
            mainDiv.Attributes.Add("class", ".form-inline");

            name.Attributes.Add("id", "irGroupName");
            name.Attributes.Add("class", "text-primary");
          

            name.InnerText = group["Name"].ToString();
            description.InnerText = group["Description"].ToString();

            mainDiv.Controls.Add(name);
            mainDiv.Controls.Add(description);
            groupLink.Controls.Add(mainDiv);

            LinkContainer.Controls.Add(groupLink);
        }

        protected void NewGroup_Click(object sender, EventArgs e)
        {
            if (SessionId != null)
            {
                Session["group_level"] = "private";
                Response.Redirect("NewIrGroup.aspx");
            }
            else
            {
                HtmlGenericControl mainDiv = new HtmlGenericControl("div");
                CreateErrorMessage(mainDiv);
                LinkContainer.Controls.Add(mainDiv);
            }

        }


    }
    
}