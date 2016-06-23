using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using COMP2007_S2016_MidTerm1_200308239.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

/**
        * @authorname: parvati
        * @method: This page will display The Todo list
        
        */

namespace COMP2007_S2016_MidTerm1_200308239
{
    public partial class TodoList : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {

          
            if (!IsPostBack)
            {
                Session["SortColumn"] = "TodoID"; // default sort column
                Session["SortDirection"] = "ASC";
            
                this.GetTodo();
            }
        }

        
        protected void GetTodo()
        {
            // connect to tc
            using (TodoConnection tc = new TodoConnection())
            {
                string SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                // query the todo Table 
                var Todo = (from allTodo in tc.Todos
                                select allTodo);

                // bind the result to the GridView
                TodoGridView.DataSource = Todo.AsQueryable().OrderBy(SortString).ToList();
                TodoGridView.DataBind();
            }
        }
       






        protected void PageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            TodoGridView.PageSize = Convert.ToInt32(PageSizeDropDownList.SelectedValue);

            
            this.GetTodo();

        }

        protected void TodoGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            // store which row was clicked
            int selectedRow = e.RowIndex;

            
            int TodoID = Convert.ToInt32(TodoGridView.DataKeys[selectedRow].Values["TodoID"]);

            
            using (TodoConnection tc = new TodoConnection())
            {
                
                Todo deletedTodo = (from Todo in tc.Todos
                                          where Todo.TodoID == TodoID
                                          select Todo).FirstOrDefault();

               
                tc.Todos.Remove(deletedTodo);

               
                tc.SaveChanges();

                // refresh the grid
                this.GetTodo();
            }

        }

        protected void TodoGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
          
            TodoGridView.PageIndex = e.NewPageIndex;

            // refresh the grid
            this.GetTodo();

        }

        protected void TodoGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session["SortColumn"] = e.SortExpression;

            // Refresh the Grid
            this.GetTodo();

            // toggle the direction
            Session["SortDirection"] = Session["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";

        }

        protected void TodoGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header) // if header row has been clicked
                {
                    LinkButton linkbutton = new LinkButton();

                    for (int index = 0; index < TodoGridView.Columns.Count - 1; index++)
                    {
                        if (TodoGridView.Columns[index].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "ASC")
                            {
                                linkbutton.Text = " <i class='fa fa-caret-up fa-lg'></i>";
                            }
                            else
                            {
                                linkbutton.Text = " <i class='fa fa-caret-down fa-lg'></i>";
                            }

                            e.Row.Cells[index].Controls.Add(linkbutton);
                        }
                    }
                }
            }
        }
    

}
    }
