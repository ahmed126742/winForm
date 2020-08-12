using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Windows.Documents;
using System.Data.Entity.Core.Common.EntitySql;
using System.IO;

namespace Itagency
{
    public partial class Form1 : Form
    {

        Employee emp = new Employee();
        Department dep = new Department();
        CompanyContxtEntities db = new CompanyContxtEntities();

        public Form1()
        {
            InitializeComponent();
            fillComboBOx();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            emp.Name = Name.Text;
            emp.age = int.Parse(age.Text);
            emp.Salary = Decimal.Parse(Salary.Text);
            emp.departmentId = (int)comboBox1.SelectedIndex;
            db.Employees.Add(emp);
            db.SaveChanges();
            Name.Text = "";
            age.Text ="";
            Salary.Text="";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dep.Name = DepName.Text;
            db.Departments.Add(dep);
            db.SaveChanges();
            DepName.Text = "";
            fillComboBOx();


        }

        private void  fillComboBOx()
        {
            //comboBox1.Items.Clear();
            var data = db.Departments.ToList();
            DataTable table = new DataTable();
            table.Columns.Add("id");
            table.Columns.Add("DepName");
            foreach (var item in data)
            {
                table.Rows.Add(item.departmentId, item.Name);
            }
            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "DepName";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.Employees
                .Join(db.Departments,
                      p => p.departmentId,
                      c => c.departmentId,
                      (p, c) => new {
                          Name = p.Name,
                          age = p.age,
                          Salary = p.Salary,
                          Department = c.Name
                      }
                      ).ToArray();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            StringBuilder CsvFile = new StringBuilder();
            CsvFile.AppendLine("Name , Age , Salary ,Department");
            var _Data = db.Employees
                .Join(db.Departments,
                      p => p.departmentId,
                      c => c.departmentId,
                      (p, c) => new {
                          Name = p.Name,
                          age = p.age,
                          Salary = p.Salary,
                          Department = c.Name
                      }
                      ).ToArray();
            foreach (var item in _Data)
            {
                CsvFile.AppendLine(item.Name+","+item.age+","+item.Salary+","+item.Department);
            }
            string Path = "E:\\DataBackup.csv"; // StATIC nName just right now 
            File.AppendAllText(Path, CsvFile.ToString());
        }
    }
}
