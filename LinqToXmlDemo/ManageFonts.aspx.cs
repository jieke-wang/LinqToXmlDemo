using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToXML;

namespace LinqToXmlDemo
{
    public partial class ManageFonts : System.Web.UI.Page
    {
        protected string xmlPath
        {
            get { return ViewState["_xmlPath"].ToString(); }
            set { ViewState["_xmlPath"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                xmlPath = Server.MapPath("/CustomizationFonts.xml");
                Populate();
            }
        }

        void Populate()
        {
            XMLFactory<CustomizationFont> factory = new XMLFactory<CustomizationFont>(xmlPath);
            List<CustomizationFont> tests = factory.GetAll();
            lvTest.DataSource = tests;
            lvTest.DataBind();
        }

        protected void lvTest_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            lvTest.EditIndex = -1;
            Populate();
        }

        protected void lvTest_ItemDeleting(object sender, ListViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(lvTest.DataKeys[e.ItemIndex].Value);
            XMLFactory<CustomizationFont> factory = new XMLFactory<CustomizationFont>(xmlPath);
            factory.Delete(id);
            Populate();
        }

        protected void lvTest_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            lvTest.EditIndex = e.NewEditIndex;
            Populate();
        }

        protected void lvTest_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            ListViewDataItem item = lvTest.Items[e.ItemIndex];
            TextBox txtFontName = (TextBox)item.FindControl("txtFontName");
            TextBox txtFontImage = (TextBox)item.FindControl("txtFontImage");
            int id = Convert.ToInt32(lvTest.DataKeys[e.ItemIndex].Value);
            XMLFactory<CustomizationFont> factory = new XMLFactory<CustomizationFont>(xmlPath);

            CustomizationFont objTest = factory.GetByID(id);
            objTest.FontName = txtFontName.Text;
            objTest.FontImage = txtFontImage.Text;
            factory.Update(objTest);

            lvTest.EditIndex = -1;
            Populate();
        }

        protected void brnAdd_Click(object sender, EventArgs e)
        {
            CustomizationFont objTest = new CustomizationFont();
            objTest.FontName = txtFontName.Text;
            objTest.FontImage = txtFontImage.Text;

            XMLFactory<CustomizationFont> factory = new XMLFactory<CustomizationFont>(xmlPath);
            factory.Add(objTest);

            Populate();
        }
    }
}