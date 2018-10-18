﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using litclassicbot.Classes;


namespace litclassicbot
{
    public partial class Particles : System.Web.UI.Page
    {
        private int currentParticleID = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            // 1. Cookie записывается верно при первом открытии страницы (когда cookie-файл не создан)
            // 2. Cookie записывается верно при смене значения checkbox, когда, опять же, cookie-файл ещё не создан
            // 3. Cookie сохраняется при обновлении страницы один раз после ситуации в п.2, далее обнуляется
            // 4. Cookie обнуляется при обновлении страницы после п.1.

            // если у пользователя разрешены cookie - использовать их
            // если не разрешены - использовать сеанс

            if (Request.Cookies["litclassic-cookie"] != null)
            {
                if (Server.HtmlEncode(Request.Cookies["litclassic-cookie"]["theme-type-0"]) == "true")
                    CheckBoxThemeType0.Checked = true;
                else CheckBoxThemeType0.Checked = false;
                if (Server.HtmlEncode(Request.Cookies["litclassic-cookie"]["theme-type-1"]) == "true")
                    CheckBoxThemeType1.Checked = true;
                else CheckBoxThemeType1.Checked = false;
                if (Server.HtmlEncode(Request.Cookies["litclassic-cookie"]["theme-type-2"]) == "true")
                    CheckBoxThemeType2.Checked = true;
                else CheckBoxThemeType2.Checked = false;

                Response.Cookies["litclassic-cookie"].Expires = DateTime.Now.AddYears(3);
            }
            else
            {
                CheckBoxThemeType0.Checked = true;

                Response.Cookies["litclassic-cookie"]["theme-type-0"] = "true";
                Response.Cookies["litclassic-cookie"]["theme-type-1"] = "false";
                Response.Cookies["litclassic-cookie"]["theme-type-2"] = "false";
                Response.Cookies["litclassic-cookie"].Expires = DateTime.Now.AddYears(3);
            }

            Response.Cookies["litclassic-cookie-user-info"]["last-visit"] = DateTime.Now.ToString();          
            Response.Cookies["litclassic-cookie-user-info"].Expires = DateTime.Now.AddYears(3);

            ShowRandomParticle();
        }


        private void ShowRandomParticle()
        {
            CheckCheckBoxes();

            BotDBConnect currentConnection = new BotDBConnect();

            currentConnection.SetSQLConnectionToAzureDBLitClassicBooks();

            List<string> listGetRandomParticle = new List<string>();
            listGetRandomParticle = currentConnection.GetRandomParticle("web");
            string particle = listGetRandomParticle[0];
            string title = listGetRandomParticle[1];
            int indeLastLine = Convert.ToInt32(listGetRandomParticle[2]);
            // для отправки сообщения об ошибке
            currentParticleID = Convert.ToInt32(listGetRandomParticle[3]);
            int bookID = Convert.ToInt32(listGetRandomParticle[4]);
            // создаёт "обёрточный" класс для всего содержания "частицы"
            string beginParticle = "<div class=\"label-particle-line\"><p>";
            string endParticle = "</p></div>";
            // замена символов новой строки на тег, выполняющий это в html
            particle = beginParticle + particle.Replace("\n\r", "</p><p>") + endParticle;
            particle = particle.Replace("$$strong-open$$", "<strong>");
            particle = particle.Replace("$$emphasis-open$$", "<emphasis>");
            particle = particle.Replace("$$strong-close$$", "</strong>");
            particle = particle.Replace("$$emphasis-close$$", "</emphasis>");
            // создаёт "обёрточный" класс для всего содержания сведения о "частице"
            string beginTitle = "<div class=\"label-particle-title\">";
            string endTitle = "</div>";
            // замена символов новой строки на тег, выполняющий это в html
            title = beginTitle + title.Replace("\n\r", "<br>") + endTitle;
            LabelParticleLine.Text = particle;
            //randomParticalButtonText = partical;
            LabelParticleTitle.Text = title;
        }
        private void ShowRandomPoemParticle()
        {
            BotDBConnect currentConnection = new BotDBConnect();

            currentConnection.SetSQLConnectionToAzureDBLitClassicBooks();

            List<string> listGetRandomParticle = new List<string>();
            listGetRandomParticle = currentConnection.GetRandomPoemParticle("web");
            string particle = listGetRandomParticle[0];
            string title = listGetRandomParticle[1];
            int indeLastLine = Convert.ToInt32(listGetRandomParticle[2]);
            // для отправки сообщения об ошибке
            currentParticleID = Convert.ToInt32(listGetRandomParticle[3]);
            int bookID = Convert.ToInt32(listGetRandomParticle[4]);
            // создаёт "обёрточный" класс для всего содержания "частицы"
            string beginParticle = "<div class=\"label-poem-particle-line\"><p>";
            string endParticle = "</p></div>";
            // замена символов новой строки на тег, выполняющий это в html
            particle = beginParticle + particle.Replace("\n\r", "</p><p>") + endParticle;
            particle = particle.Replace("$$strong-open$$", "<strong>");
            particle = particle.Replace("$$emphasis-open$$", "<emphasis>");
            particle = particle.Replace("$$strong-close$$", "</strong>");
            particle = particle.Replace("$$emphasis-close$$", "</emphasis>");
            // создаёт "обёрточный" класс для всего содержания сведения о "частице"
            string beginTitle = "<div class=\"label-particle-title\">";
            string endTitle = "</div>";
            // замена символов новой строки на тег, выполняющий это в html
            title = beginTitle + title.Replace("\n\r", "<br>") + endTitle;
            LabelPoemParticleLine.Text = particle;
            LabelParticleTitle.Text = title;
        }
        private void ReportParticle()
        {
            BotDBConnect currentConnection = new BotDBConnect();

            currentConnection.SetSQLConnectionToAzureDBLitClassicBooks();
            currentConnection.WriteNewParticleReportByParticleID(currentParticleID.ToString());
        }
        private void CheckCheckBoxes()
        {
            // нельзя оставлять все чекбоксы пустыми
            if ((CheckBoxThemeType0.Checked == false)
                && (CheckBoxThemeType1.Checked == false)
                && (CheckBoxThemeType2.Checked == false))
                CheckBoxThemeType0.Checked = true;
        }



        protected void CheckBoxThemeType0_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void CheckBoxThemeType1_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void CheckBoxThemeType2_CheckedChanged(object sender, EventArgs e)
        {

        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    ReportPartical();
        //}
    }
}