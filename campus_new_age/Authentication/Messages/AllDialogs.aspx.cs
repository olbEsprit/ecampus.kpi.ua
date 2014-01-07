﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace campus_new_age.Authentication
{
    public partial class AllDialogs : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {

            Dictionary<string, object> answer = GetData("http://localhost:49945//message/GetUserConversations?sessionId=" + Session["UserData"].ToString());
            ArrayList Data;

            if (answer != null)
            {

                Data = (ArrayList)answer["Data"];

                for (int i = 0; i < Data.Count; i++)
                {
                    Dictionary<string, object> kvData = (Dictionary<string, object>)Data[i];
                    LinkButtonsRendering(kvData);
                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }

        }

        protected void messageLink_Click(object sender, EventArgs e)
        {

            LinkButton activeLink = sender as LinkButton;

            if (activeLink != null)
            {
                Session["GroupId"] = activeLink.Attributes["cId"];
                Session["imgDiv"] = activeLink.Controls[0].Controls[0];
                Session["Subject"] = activeLink.Attributes["subj"];
                Response.Redirect("Messages.aspx");
            }

        }

        public Dictionary<string, object> GetData(string request)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;

                var json = client.DownloadString(request);
                var serializer = new JavaScriptSerializer();
                Dictionary<string, object> respDictionary = serializer.Deserialize<Dictionary<string, object>>(json);

                return respDictionary;

            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public void LinkButtonsRendering(Dictionary<string, object> Data) {

            ArrayList Users = (ArrayList)Data["Users"];

            LinkButton messageLink = new LinkButton();
            HtmlGenericControl mainDiv = new HtmlGenericControl("div");
            HtmlGenericControl imgDiv = new HtmlGenericControl("div");
            HtmlGenericControl infoDiv = new HtmlGenericControl("div");
            HtmlGenericControl subject = new HtmlGenericControl("h5");
            HtmlGenericControl last = new HtmlGenericControl("p");
            HtmlGenericControl date = new HtmlGenericControl("p");


            for (int j = 0; j < Users.Count || j > 4; j++)
            {
                Dictionary<string, object> kvUsers = (Dictionary<string, object>)Users[j];
                Image ownImg = new Image();

                ownImg.ImageUrl = kvUsers["Photo"].ToString();
                imgDiv.Controls.Add(ownImg);
            }

            messageLink.PostBackUrl = Request.Url.AbsolutePath.ToString();
            messageLink.Attributes.Add("class", "messageLink");
            messageLink.Attributes.Add("cId", Data["GroupId"].ToString());
            messageLink.Attributes.Add("subj",Data["Subject"].ToString());
            messageLink.Click += messageLink_Click;

            mainDiv.Attributes.Add("id", "mainBlock");
            mainDiv.Attributes.Add("class", ".form-inline");

            imgDiv.Attributes.Add("id", "imgBlock");
            //imgDiv.Attributes.Add("class", "col-md2");

            infoDiv.Attributes.Add("id", "infoBlock");
            //infoDiv.Attributes.Add("class", "col-md5");

            subject.Attributes.Add("id", "subject");
            subject.Attributes.Add("class", "text-primary");
            last.Attributes.Add("id", "last");
            last.Attributes.Add("class", "text-success");
            date.Attributes.Add("id", "date");
            date.Attributes.Add("class", "text-warning");

            subject.InnerText = Data["Subject"].ToString();
            last.InnerText = Data["LastMessageText"].ToString();
            date.InnerText = Data["LastMessageDate"].ToString();

            infoDiv.Controls.Add(subject);
            infoDiv.Controls.Add(last);
            infoDiv.Controls.Add(date);
            mainDiv.Controls.Add(imgDiv);
            mainDiv.Controls.Add(infoDiv);
            messageLink.Controls.Add(mainDiv);

            LinkContainer.Controls.Add(messageLink);

        }


    }
}