using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQL_1
{
    public partial class Form1 : Form
    {
        int id = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool s = CM(2);
            if (s)
            {
                MessageBox.Show("登入成功");
            }
            else
            {
                MessageBox.Show("登入失敗");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            id = 0;

            showtable();
        }

        void showtable()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|Database1.mdf;Integrated Security=True";
            cn.Open();

            if (cn.State == ConnectionState.Open)
            {
                string sql = $"select*from member";

                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(sql, cn);

                da.Fill(ds, "member");

                dataGridView1.DataSource = ds.Tables["member"];

                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("標楷體", 20);
                dataGridView1.DefaultCellStyle.Font = new Font("標楷體", 18);

                dataGridView1.Columns["id"].HeaderText = "序號";
                dataGridView1.Columns["m_id"].HeaderText = "帳號";
                dataGridView1.Columns["m_password"].HeaderText = "密碼";
                dataGridView1.Columns["m_name"].HeaderText = "暱稱";
                dataGridView1.Columns["m_phone"].HeaderText = "電話";
                dataGridView1.Columns["id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns["m_id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns["m_password"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns["m_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns["m_phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.Rows[0].Selected = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (text_name.Text != "")
            {
                if (!CM(1))
                {
                    SqlConnection cn = new SqlConnection();
                    cn.ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|Database1.mdf;Integrated Security=True";
                    cn.Open();

                    string sql = "insert into member(m_id,m_password,m_name) values(@m_id,@m_password,@m_name)";

                    SqlCommand cmd = new SqlCommand(sql, cn);

                    cmd.Parameters.Add(new SqlParameter("@m_id", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@m_id"].Value = text_admin.Text;

                    cmd.Parameters.Add(new SqlParameter("@m_password", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@m_password"].Value = text_password.Text;

                    cmd.Parameters.Add(new SqlParameter("@m_name", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@m_name"].Value = text_name.Text;

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("新增成功");
                        showtable();
                    }

                    cn.Close();
                }
                else
                {
                    MessageBox.Show("帳號已存在");
                }
            }
            else
            {
                MessageBox.Show("請輸入名稱");
            }
        }
        private bool CM(int s)
        {
            bool flag = false;
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|Database1.mdf;Integrated Security=True";
            cn.Open();
            if (cn.State == ConnectionState.Open)
            {
                string sql = "";
                if (s == 1)
                {
                    sql = "select*from member where m_id=@m_id";
                }
                else if (s == 2)
                {
                    sql = "select*from member where m_id=@m_id or m_phone=@m_phone and m_password=@m_password";
                }

                SqlCommand cmd = new SqlCommand(sql, cn);

                cmd.Parameters.Add(new SqlParameter("@m_id", SqlDbType.NVarChar, 50));
                cmd.Parameters["@m_id"].Value = text_admin.Text;
                
                cmd.Parameters.Add(new SqlParameter("@m_password", SqlDbType.NVarChar, 50));
                cmd.Parameters["@m_password"].Value = text_password.Text;
                
                cmd.Parameters.Add(new SqlParameter("@m_name", SqlDbType.NVarChar, 50));
                cmd.Parameters["@m_name"].Value = text_name.Text;

                cmd.Parameters.Add(new SqlParameter("@m_phone", SqlDbType.NVarChar, 50));
                cmd.Parameters["@m_phone"].Value = text_phone.Text;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (s == 2) { id = Convert.ToInt32(dr["id"]); }
                    text_name.Text = dr["m_name"].ToString();
                    flag = true;
                }

            }
            cn.Close();
            return flag;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|Database1.mdf;Integrated Security=True";
            cn.Open();

            string sql = "update member set m_password=@m_password,m_name=@m_name,m_phone=@m_phone where id=@id";

            SqlCommand cmd = new SqlCommand(sql, cn);

            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, 50));
            cmd.Parameters["@id"].Value = id;

            cmd.Parameters.Add(new SqlParameter("@m_password", SqlDbType.NVarChar, 50));
            cmd.Parameters["@m_password"].Value = text_password.Text;

            cmd.Parameters.Add(new SqlParameter("@m_name", SqlDbType.NVarChar, 50));
            cmd.Parameters["@m_name"].Value = text_name.Text;
            
            cmd.Parameters.Add(new SqlParameter("@m_phone", SqlDbType.NVarChar, 50));
            cmd.Parameters["@m_phone"].Value = text_phone.Text;

            int reslt = cmd.ExecuteNonQuery();

            if (reslt>0)
            {
                MessageBox.Show("修改成功");
                showtable();
            }

            cn.Close();
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1 && e.ColumnIndex == -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                id = Convert.ToInt32(row.Cells["id"].Value);

                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|Database1.mdf;Integrated Security=True";
                cn.Open();

                string sql = "delete from member where id=@id";

                SqlCommand cmd = new SqlCommand(sql, cn);

                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, 50));
                cmd.Parameters["@id"].Value = id;

                int reslt = cmd.ExecuteNonQuery();

                if (reslt > 0)
                {
                    MessageBox.Show("刪除成功");
                }
                else
                {
                    MessageBox.Show("刪除失敗");
                }

                cn.Close();
                showtable();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult yn = MessageBox.Show("是否要刪除","警告", MessageBoxButtons.OKCancel,MessageBoxIcon.Error);

            if (yn == DialogResult.OK)
            {
                if (e.RowIndex > -1 && e.ColumnIndex == -1)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    id = Convert.ToInt32(row.Cells["id"].Value);

                    SqlConnection cn = new SqlConnection();
                    cn.ConnectionString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|Database1.mdf;Integrated Security=True";
                    cn.Open();

                    string sql = "delete from member where id=@id";

                    SqlCommand cmd = new SqlCommand(sql, cn);

                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@id"].Value = id;

                    int reslt = cmd.ExecuteNonQuery();

                    if (reslt > 0)
                    {
                        MessageBox.Show("刪除成功");
                    }
                    else
                    {
                        MessageBox.Show("刪除失敗");
                    }

                    cn.Close();
                    showtable();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
