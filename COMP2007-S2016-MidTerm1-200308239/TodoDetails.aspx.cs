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
        * @method: This page will display detailed Todo list
        
        */

namespace COMP2007_S2016_MidTerm1_200308239
{
    public partial class TodoDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetTodo();
            }
        }

        protected void GetTodo()
        {
            // populate teh form with existing data from the database
            int TodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

            // connect to the tc DB
            using (TodoConnection tc = new TodoConnection())
            {
                // populate a todo object instance with the TodoID
                Todo updatedTodo = (from Todo in tc.Todos
                                          where Todo.TodoID == TodoID
                                          select Todo).FirstOrDefault();

                // maps todo properties  the form controls
                if (updatedTodo != null)
                {
                    TodoNameTextBox.Text = updatedTodo.TodoName;
                    TodoNotesTextBox.Text = updatedTodo.TodoNotes;
                   

                }
            }

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // Redirect back to Todo page
            Response.Redirect("~/TodoList.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            using (TodoConnection tc = new TodoConnection())
            {
               
                Todo newTodo = new Todo();

                int TodoID = 0;

                if (Request.QueryString.Count > 0) 
                {
                    // get the id from the URL
                    TodoID = Convert.ToInt32(Request.QueryString["TodoID"]);

                    // get the current Todo from tc Database
                    newTodo = (from Todo in tc.Todos
                                  where Todo.TodoID == TodoID
                                  select Todo).FirstOrDefault();
                }

                // add new Todo record
                newTodo.TodoName = TodoNameTextBox.Text;
                newTodo.TodoNotes = TodoNotesTextBox.Text;
                

                // use LINQ to ADO.NET to add / insert new Todo List into the database

                if (TodoID == 0)
                {
                    tc.Todos.Add(newTodo);
                }


                
                tc.SaveChanges();

                // Redirect back to the updated Todo page
                Response.Redirect("~/TodoList.aspx");
            }

        }
    }
}