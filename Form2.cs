using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CreatedProductDemo
{
    public partial class Form2 : Form
    {
        SqlConnection con;
        SqlDataAdapter da;
        SqlCommandBuilder builder;
        DataSet ds;

        public Form2()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                //write query
                string qry = "select *from categoryDemo";
                //assign query to the adapter---> it will fire query
                da = new SqlDataAdapter(qry, con);
                //created object of the dataset
                ds = new DataSet();
                //fill() will fire the sekect query and load data in the dataset
                //categorydemo is the name given to the table in the datset 
                da.Fill(ds, "categoryDemo");
                cmbCategory.DataSource = ds.Tables["categoryDemo"];
                cmbCategory.DisplayMember = "cname";
                cmbCategory.ValueMember = "cid";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private DataSet GetProduct()
        {
            string qry = "select * from ProductDemo";
            da=new SqlDataAdapter(qry, con);
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            builder = new SqlCommandBuilder(da);
            ds= new DataSet();
            da.Fill(ds, "ProductDemo");
            return ds;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ds= GetProduct();
            //create new record to add record
            DataRow row = ds.Tables["ProductDemo"].NewRow();
            row["pname"]=txtProductName.Text;
            row["price"]=txtProductPrice.Text;
            row["cid"] = cmbCategory.SelectedValue;
            //attach this row in datset table
            ds.Tables["ProductDemo"].Rows.Add(row);
            //update the changes from dataset to DB
            int result = da.Update(ds.Tables["ProductDemo"]);
            if(result>=1)
            {
                MessageBox.Show("Record is inserted");
            }
            
                            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProduct();
                //find the row
                DataRow row = ds.Tables["ProductDemo"].Rows.Find(txtID.Text);
                if(row!=null)
                {
                    row["pname"] = txtProductName.Text;
                    row["price"] = txtProductPrice.Text;
                    row["cid"] = cmbCategory.SelectedValue;
                    //update the changes from Dataset to DB
                    int result = da.Update(ds.Tables["ProductDemo"]);
                    if(result>=1)
                    {
                        MessageBox.Show("Record is Updated");
                    }
                }
                else
                {
                    MessageBox.Show("ID is not matched");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProduct();
                //find the row
                DataRow row = ds.Tables["ProductDemo"].Rows.Find(txtID.Text);
                if(row!=null)
                {
                    //delete the current row from the dataset table
                    row.Delete();
                    //update the changes from Dataset to DB
                    int result = da.Update(ds.Tables["ProductDemo"]);
                    if(result>=1)
                    {
                        MessageBox.Show("Record is Deleted");
                    }
                }
                else
                {
                    MessageBox.Show("ID not matched");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                ds = GetProduct();
                dataGridView1.DataSource = ds.Tables["ProductDemo"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select p.*, c.Cname from ProductDemo p inner join categoryDemo c on p.Cid = c.Cid";
                da = new SqlDataAdapter(qry, con);
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ds = new DataSet();
                da.Fill(ds, "cat");
                //find method can only seach the data if PK is applied in the DataSet table
                DataRow row = ds.Tables["cat"].Rows.Find(txtID.Text);
                if (row != null)
                {
                    txtProductName.Text = row["pname"].ToString();
                    txtProductPrice.Text = row["Price"].ToString();
                    cmbCategory.Text = row["Cname"].ToString();
                }
                else
                {
                    MessageBox.Show("Record not found");
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            {

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtID.Text = dataGridView1.CurrentRow.Cells["pid"].Value.ToString();
            txtProductName.Text = dataGridView1.CurrentRow.Cells["pname"].Value.ToString();
            txtProductPrice.Text = dataGridView1.CurrentRow.Cells["price"].Value.ToString();
        }
    }
}
