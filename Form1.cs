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
    public partial class Form1 : Form

    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader reader;

        public Form1()
        {
            InitializeComponent();
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<CategoryDemo> list = new List<CategoryDemo>();
                string qry = "select * from categoryDemo";
                cmd = new SqlCommand(qry, con);
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CategoryDemo cd = new CategoryDemo();
                        cd.Cid = Convert.ToInt32(reader["cid"]);
                        cd.Cname = reader["cname"].ToString();
                        list.Add(cd);
                    }
                }
                // display dname & on selection of dname we need did
                cmbCategory.DataSource = list;
                cmbCategory.DisplayMember = "Cname";
                cmbCategory.ValueMember = "Cid";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "insert into ProductDemo values(@name,@price,@cid)";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", txtProductName.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(txtProductPrice.Text));
                cmd.Parameters.AddWithValue("@cid", Convert.ToInt32(cmbCategory.SelectedValue));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record is  inserted");
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "select p.*, c.cname from ProductDemo p inner join category c on c.cid = p.cid where p.pid=@id";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        txtProductName.Text = reader["pname"].ToString();
                        txtProductPrice.Text = reader["price"].ToString();
                        cmbCategory.Text = reader["cname"].ToString();

                    }
                }
                else
                {
                    MessageBox.Show("Record not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "update ProductDemo set pname=@name,price=@price,cid=@cid where pid=@id";
                cmd = new SqlCommand(qry, con);
                // assign value to each parameter
                cmd.Parameters.AddWithValue("@name", txtProductName.Text);
                cmd.Parameters.AddWithValue("@price", Convert.ToDouble(txtProductPrice.Text));
                cmd.Parameters.AddWithValue("@cid", Convert.ToInt32(cmbCategory.SelectedValue));
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtID.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record is Updated");
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
           
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string qry = "delete from ProductDemo where pid=@pid";
                cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@pid", Convert.ToInt32(txtID.Text));
                con.Open();
                int result = cmd.ExecuteNonQuery();
                if (result >= 1)
                {
                    MessageBox.Show("Record deleted");
                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            

        }
        private void GetAllProducts()
        {
            string qry = "select p.*, c.cname from ProductDemo p inner join category c on c.cid = p.cid";
            cmd = new SqlCommand(qry, con);
            con.Open();
            reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            con.Close();

        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            try
            {
                GetAllProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    }



   
    

    




        
    

