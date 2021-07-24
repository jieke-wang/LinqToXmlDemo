using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqToXML;

namespace LinqToXmlDemo
{
    public partial class Demo : System.Web.UI.Page
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
                xmlPath = Server.MapPath("/test.xml");
                Populate();
            }
        }

        void Populate()
        {
            XMLFactory<Test> factory = new XMLFactory<Test>(xmlPath);
            List<Test> tests = factory.GetAll();
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
            XMLFactory<Test> factory = new XMLFactory<Test>(xmlPath);
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
            TextBox txtName = (TextBox)item.FindControl("txtName");
            TextBox txtAge = (TextBox)item.FindControl("txtAge");
            int id = Convert.ToInt32(lvTest.DataKeys[e.ItemIndex].Value);
            XMLFactory<Test> factory = new XMLFactory<Test>(xmlPath);

            Test objTest = factory.GetByID(id);
            objTest.Name = txtName.Text;
            objTest.Age = Convert.ToInt32(txtAge.Text);
            factory.Update(objTest);

            lvTest.EditIndex = -1;
            Populate();
        }

        protected void brnAdd_Click(object sender, EventArgs e)
        {
            Test objTest = new Test();
            objTest.Name = txtName.Text;

            try
            {
                objTest.Age = Convert.ToInt32(txtAge.Text);
            }
            catch
            {
            }

            XMLFactory<Test> factory = new XMLFactory<Test>(xmlPath);
            factory.Add(objTest);

            Populate();
        }
    }
}