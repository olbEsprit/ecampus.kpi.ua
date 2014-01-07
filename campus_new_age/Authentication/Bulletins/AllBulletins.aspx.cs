﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using campus_new_age.Authentication;

namespace campus_new_age.Authentication.Bulletins
{
    public partial class AllBulletins : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserData"] != null)
            {
                Dictionary<string, object> answer = SameCore.GetData("http://api.ecampus.kpi.ua/BulletinBoard/GetActual?sessionId="+Session["UserData"]);
                ArrayList Bulletins;

                if (answer != null) {
                    Bulletins = (ArrayList)answer["Data"];
                    BulletinsRendering(Bulletins);
                }
            }
            else
            {
                HtmlGenericControl mainDiv = new HtmlGenericControl("div");
                mainDiv.Attributes.Add("id", "mainBlock");
                BulletinsContainer.Controls.Add(mainDiv);
                SameCore.CreateErrorMessage(mainDiv);
            }

        }

        private void BulletinsRendering(ArrayList Bulletins) {
            
            for (int i = 0; i < Bulletins.Count; i++)
            {
                Dictionary<string, object> kvBulletin = (Dictionary<string, object>)Bulletins[i];
                BulletinsContainer.Controls.Add(BulletinBlockRendering(kvBulletin));
            }
        }

        private HtmlGenericControl BulletinBlockRendering(Dictionary<string, object> kvBulletin) {
            
            HtmlGenericControl bulletinDiv = new HtmlGenericControl("div");
            bulletinDiv.Attributes.Add("class", "inf_des");

            HtmlGenericControl dateSpan = new HtmlGenericControl("span");
            dateSpan.Attributes.Add("class","date");
            dateSpan.InnerText = kvBulletin["DateCreate"].ToString();

            HtmlGenericControl subject = new HtmlGenericControl("h4");
            subject.Attributes.Add("class", "text-primary");
            subject.InnerText = kvBulletin["Subject"].ToString();

            HtmlGenericControl readLink = new HtmlGenericControl("a");
            readLink.Attributes.Add("class", "showText");
            readLink.InnerText = "[читати...]";

            HtmlGenericControl text = new HtmlGenericControl("p");
            text.Attributes.Add("class", "text-success");
            text.InnerText = kvBulletin["Text"].ToString();

            HtmlGenericControl publisher = new HtmlGenericControl("span");
            publisher.Attributes.Add("class","poster");
            HtmlGenericControl publisherName = new HtmlGenericControl("a");
            publisherName.InnerText = kvBulletin["CreatorUserFullName"] == null ? "Анонім" : kvBulletin["CreatorUserFullName"].ToString();
            publisher.Controls.Add(publisherName);

            bulletinDiv.Controls.Add(dateSpan);
            bulletinDiv.Controls.Add(subject);
            bulletinDiv.Controls.Add(readLink);
            bulletinDiv.Controls.Add(text);
            bulletinDiv.Controls.Add(publisher);

            return bulletinDiv;
        }

        protected void ModearatorMode_Click(object sender, EventArgs e)
        {
            Response.Redirect("MyBulletins.aspx");
        }
    }
}