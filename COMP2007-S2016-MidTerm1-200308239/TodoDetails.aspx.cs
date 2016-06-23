﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using COMP2007_S2016_MidTerm1_200308239.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

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

            // connect to the EF DB
            using (TodoConnection tc = new TodoConnection())
            {
                // populate a student object instance with the StudentID from the URL Parameter
                Todo updatedTodo = (from Todo in tc.Todos
                                          where Todo.TodoID == TodoID
                                          select Todo).FirstOrDefault();

                // map the student properties to the form controls
                if (updatedTodo != null)
                {
                    TodoNameTextBox.Text = updatedTodo.TodoName;
                    TodoNotesTextBox.Text = updatedTodo.TodoNotes;
                   

                }
            }

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
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

                    // get the current student from EF DB
                    newTodo = (from Todo in tc.Todos
                                  where Todo.TodoID == TodoID
                                  select Todo).FirstOrDefault();
                }

                // add form data to the new student record
                newTodo.TodoName = TodoNameTextBox.Text;
                newTodo.TodoNotes = TodoNotesTextBox.Text;
                

                // use LINQ to ADO.NET to add / insert new student into the database

                if (TodoID == 0)
                {
                    tc.Todos.Add(newTodo);
                }


                
                tc.SaveChanges();

                // Redirect back to the updated students page
                Response.Redirect("~/TodoList.aspx");
            }

        }
    }
}