﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace campus_new_age.Authentication.Bulletins
{
    public partial class MyBulletins : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserData"] != null)
            {
                Dictionary<string, object> answer = SameCore.GetData("http://api.ecampus.kpi.ua/BulletinBoard/GetMyBulletins?sessionId=" + Session["UserData"]);
                ArrayList Bulletins;

                if (answer != null)
                {
                    Bulletins = (ArrayList)answer["Data"];
                    BulletinsRendering(Bulletins);
                }
            }
            else
            {
                HtmlGenericControl mainDiv = new HtmlGenericControl("div");
                mainDiv.Attributes.Add("id", "mainBlock");
                MyBulletinsContainer.Controls.Add(mainDiv);
                SameCore.CreateErrorMessage(mainDiv);
            }

        }

        private void BulletinsRendering(ArrayList Bulletins)
        {

            for (int i = 0; i < Bulletins.Count; i++)
            {
                Dictionary<string, object> kvBulletin = (Dictionary<string, object>)Bulletins[i];
                MyBulletinsContainer.Controls.Add(BulletinBlockRendering(kvBulletin));
            }
        }

        private HtmlGenericControl BulletinBlockRendering(Dictionary<string, object> kvBulletin)
        {

            HtmlGenericControl bulletinDiv = new HtmlGenericControl("div");
            bulletinDiv.Attributes.Add("class", "inf_des");

            HtmlGenericControl dateSpan = new HtmlGenericControl("span");
            dateSpan.Attributes.Add("class", "date");
            dateSpan.InnerText = kvBulletin["DateCreate"].ToString();

            HtmlGenericControl subject = new HtmlGenericControl("h4");
            subject.Attributes.Add("class", "text-danger");
            subject.InnerText = kvBulletin["Subject"].ToString();

            HtmlGenericControl readLink = new HtmlGenericControl("a");
            readLink.Attributes.Add("class", "showText");
            readLink.InnerText = "[редагувати...]";

            HtmlGenericControl setDiv = new HtmlGenericControl("div");
            setDiv.Attributes.Add("class", "set_des");
            HtmlGenericControl spanSubj = new HtmlGenericControl("span");
            spanSubj.Attributes.Add("class", "text-primary");
            spanSubj.InnerText = "Тема ";
            TextBox setSubj = new TextBox();
            setSubj.Attributes.Add("class", "form-control");
            setSubj.Text = kvBulletin["Subject"].ToString();
            HtmlGenericControl spanText = new HtmlGenericControl("span");
            spanText.Attributes.Add("class", "text-primary");
            spanText.InnerText = "Текст ";
            TextBox setText = new TextBox();
            setText.Attributes.Add("class", "form-control");
            setText.Text = kvBulletin["Text"].ToString();
            Button saveChanges = new Button();
            saveChanges.Text = "Зберегти зміни";
            saveChanges.CssClass = "btn btn-success btn-sm";
            setDiv.Controls.Add(spanSubj);
            setDiv.Controls.Add(setSubj);
            setDiv.Controls.Add(spanText);
            setDiv.Controls.Add(setText);
            setDiv.Controls.Add(saveChanges);

            HtmlGenericControl publisher = new HtmlGenericControl("span");
            publisher.Attributes.Add("class", "poster");
            HtmlGenericControl publisherName = new HtmlGenericControl("a");
            publisherName.InnerText = " ";
            publisher.Controls.Add(publisherName);

            bulletinDiv.Controls.Add(dateSpan);
            bulletinDiv.Controls.Add(subject);
            bulletinDiv.Controls.Add(readLink);
            bulletinDiv.Controls.Add(setDiv);
            bulletinDiv.Controls.Add(publisher);

            return bulletinDiv;
        }
    }
}