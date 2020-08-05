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

namespace Car_repair
{
    public partial class Form1 : Form
    {

        SqlConnection sqlConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='|DataDirectory|CarRepair.mdf';Integrated Security=True";
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Maria\source\repos\Car repair\Car repair\CarRepair.mdf;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            Load_data();
            comboBox_data();
        }


        private void Load_data()
        {
            dataGridView.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();

            string query = "SELECT Model,VIN,SPARES.Cost,WORKS.Cost,MATERIALS.Cost, ISNULL(WORKS.Cost,0)+ISNULL(SPARES.Cost,0)+ISNULL(MATERIALS.Cost,0) FROM REPAIR LEFT JOIN CARS ON REPAIR.Car=CARS.CarID LEFT JOIN DEFECTS ON REPAIR.Defect=DEFECTS.DefectID LEFT JOIN SPARES ON REPAIR.Spare=SPARES.SpareID LEFT JOIN WORKS ON REPAIR.Work=WORKS.WorkID LEFT JOIN MATERIALS ON REPAIR.Material=MATERIALS.MaterialID;";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (reader.Read())
            {
                data.Add(new string[6]);
                for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    if (reader[i].ToString() == string.Empty)
                    {
                        data[data.Count - 1][i] = " - ";
                    }
                    else
                    {
                        data[data.Count - 1][i] = reader[i].ToString();
                    }
                }
            }
            reader.Close();

            foreach (string[] s in data)
            {
                dataGridView.Rows.Add(s);
            }

            string query2 = "SELECT Model,DefectName,WorkType,Cost FROM REPAIR LEFT JOIN CARS ON REPAIR.Car=CARS.CarID LEFT JOIN DEFECTS ON REPAIR.Defect=DEFECTS.DefectID LEFT JOIN WORKS ON REPAIR.Work=WORKS.WorkID;";
            SqlCommand command2 = new SqlCommand(query2, sqlConnection);
            SqlDataReader reader2 = command2.ExecuteReader();
            List<string[]> data2 = new List<string[]>();
            while (reader2.Read())
            {
                data2.Add(new string[6]);
                for (int i = 0; i < dataGridView2.ColumnCount; i++)
                {
                    if (reader2[i].ToString() == string.Empty)
                    {
                        data2[data2.Count - 1][i] =" - ";
                    }
                    else
                    {
                        data2[data2.Count - 1][i] = reader2[i].ToString();
                    }
                }
                    
                    
            }
            reader2.Close();
            foreach (string[] s in data2)
            {
                dataGridView2.Rows.Add(s);
            }


            string query3 = "SELECT Model,SpareName, Cost FROM REPAIR, CARS, SPARES WHERE REPAIR.Car=CARS.CarID AND REPAIR.Spare=SPARES.SpareID and REPAIR.Spare IS NOT NULL;";
            SqlCommand command3 = new SqlCommand(query3, sqlConnection);
            SqlDataReader reader3 = command3.ExecuteReader();
            List<string[]> data3 = new List<string[]>();
            while (reader3.Read())
            {
                data3.Add(new string[6]);
                for (int i = 0; i < dataGridView3.ColumnCount; i++)
                {
                    if (reader3[i].ToString() == string.Empty)
                    {
                        data3[data3.Count - 1][i] = " - ";
                    }
                    else
                    {
                        data3[data3.Count - 1][i] = reader3[i].ToString();
                    }
                }
            }
            reader3.Close();

            foreach (string[] s in data3)
            {
                dataGridView3.Rows.Add(s);
            }
        }

        private void Сlear()
        {
            dataGridView.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
        }

        private void comboBox_data()
        {
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

           
            string[] model_list ={"ГАЗ 33023", "ГАЗ 3221", "ГАЗ Садко Next", "ГАЗель Next", "ГАЗель Next Микроавтобус", "ГАЗель Некст Ситилайн", "ГАЗон Next" };
            DataTable model_table = new DataTable();
            model_table.Columns.Add("Models");
            foreach (var item in model_list)
            {
                model_table.Rows.Add(item);
            }
            comboBox1.DataSource = model_table;
            comboBox1.DisplayMember = "Models";
            comboBox1.ValueMember = "Models";
            comboBox1.SelectedIndex = -1;


            SqlCommand select_defect = new SqlCommand("SELECT DefectID, DefectName FROM DEFECTS;", sqlConnection);
            DataTable defect_table = new DataTable();
            SqlDataAdapter adapter3 = new SqlDataAdapter(select_defect);
            adapter3.Fill(defect_table);
            comboBox3.DataSource = defect_table;
            comboBox3.DisplayMember = "DefectName";
            comboBox3.ValueMember = "DefectID";
            comboBox3.SelectedIndex = -1;

            SqlCommand select_spare = new SqlCommand("SELECT SpareID,SpareName FROM SPARES;", sqlConnection);
            DataTable spare_table = new DataTable();
            SqlDataAdapter adapter4 = new SqlDataAdapter(select_spare);
            adapter4.Fill(spare_table);
            DataRow dataRow4 = spare_table.NewRow();
            dataRow4["SpareID"] = 0;
            spare_table.Rows.InsertAt(dataRow4, 0);
            comboBox4.DataSource = spare_table;
            comboBox4.DisplayMember = "SpareName";
            comboBox4.ValueMember = "SpareID";
            comboBox4.SelectedIndex = -1;

            SqlCommand select_material = new SqlCommand("SELECT MaterialID, MaterialName FROM MATERIALS;", sqlConnection);
            DataTable material_table = new DataTable();
            SqlDataAdapter adapter5 = new SqlDataAdapter(select_material);
            adapter5.Fill(material_table);
            DataRow dataRow5 = material_table.NewRow();
            dataRow5["MaterialID"] = 0;
            material_table.Rows.InsertAt(dataRow5, 0);
            comboBox5.DataSource = material_table;
            comboBox5.DisplayMember = "MaterialName";
            comboBox5.ValueMember = "MaterialID";
            comboBox5.SelectedIndex = -1;

            SqlCommand select_work = new SqlCommand("SELECT WorkID,WorkType FROM WORKS;", sqlConnection);
            DataTable work_table = new DataTable();
            SqlDataAdapter adapter6 = new SqlDataAdapter(select_work);
            adapter6.Fill(work_table);
            comboBox6.DataSource = work_table;
            comboBox6.DisplayMember = "WorkType";
            comboBox6.ValueMember = "WorkID";
            comboBox6.SelectedIndex = -1;
        }

        private void Add_Click(object sender, EventArgs e)
        {
            string VIN = textBox1.Text;
            string model = comboBox1.SelectedValue.ToString();
            int defect_ID = Convert.ToInt32(comboBox3.SelectedValue);
            int spare_ID = Convert.ToInt32(comboBox4.SelectedValue);
            int material_ID = Convert.ToInt32(comboBox5.SelectedValue);
            int work_ID = Convert.ToInt32(comboBox6.SelectedValue);

            if ((VIN == string.Empty) || string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(comboBox3.Text) || string.IsNullOrEmpty(comboBox6.Text))
            {
                MessageBox.Show("Заполните данные");
                return;
            }


            int id = check_car(model, VIN);
            if (id == 0)
            {
                string query = "INSERT INTO CARS(Model, VIN) OUTPUT Inserted.CarID VALUES(N'" + model +"','"+ VIN +"');";
                SqlCommand comand = new SqlCommand(query, sqlConnection);
                id = (Int32)comand.ExecuteScalar(); 
            }
            
            string query2 = "INSERT INTO REPAIR(Car, Defect,Spare,Work,Material) VALUES("+id+","+defect_ID+","+spare_ID+","+work_ID+","+material_ID+");";
            string query3 = "INSERT INTO REPAIR(Car, Defect,Spare,Work,Material) VALUES(" + id + "," + defect_ID + ",NULL," + work_ID + ",NULL);";
            string query4 = "INSERT INTO REPAIR(Car, Defect,Spare,Work,Material) VALUES(" + id + "," + defect_ID + ",NULL," + work_ID + "," + material_ID + ");";
            string query5 = "INSERT INTO REPAIR(Car, Defect,Spare,Work,Material) VALUES(" + id + "," + defect_ID + "," + spare_ID + "," + work_ID + ",NULL);";
            if (material_ID!=0 && spare_ID!=0)
            {
                SqlCommand command2 = new SqlCommand(query2, sqlConnection);
                command2.ExecuteNonQuery();
            }
            else
            {
                if (spare_ID == 0 && material_ID == 0)
                {
                     SqlCommand command2 = new SqlCommand(query3, sqlConnection);
                     command2.ExecuteNonQuery();
                }
                else
                {
                    if(spare_ID == 0)
                    {
                        SqlCommand command2 = new SqlCommand(query4, sqlConnection);
                        command2.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand command2 = new SqlCommand(query5, sqlConnection);
                        command2.ExecuteNonQuery();
                    }
                }
                
            }
            MessageBox.Show("Случай добавлен ");
            Load_data();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (!Char.IsDigit(c) && c != 8 && (c <= 64 || c >= 91) && (c <= 96 || c >= 123) && c==17)
            {
                e.Handled = true;
            }
        }


        private int check_car(string model,string VIN)
        {
            int id;
            SqlCommand cm = new SqlCommand("SELECT CarID FROM CARS WHERE Model=N'" + model + "' AND VIN='" + VIN + "';", sqlConnection);
            SqlDataReader r = cm.ExecuteReader();
                    r.Read();
                    if (!r.HasRows)
                    {
                        id = 0;
                     }
                     else
                    {
                        id = Convert.ToInt32(r[0].ToString());
                    }
                r.Close();
            return id;
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            string model = dataGridView.CurrentRow.Cells["Модель"].Value.ToString();
            string VIN = dataGridView.CurrentRow.Cells["ВИН"].Value.ToString();
            if (MessageBox.Show("Вы точно жеалаете удалить автомобиль-"+model +", VIN-"+ VIN +" из списка?", "DataGridView", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SqlCommand cm = new SqlCommand("SELECT CarID FROM CARS WHERE Model=N'" + model + "' AND VIN='" + VIN + "';", sqlConnection);
                SqlDataReader r = cm.ExecuteReader();
                r.Read();
                int id = Convert.ToInt32(r[0].ToString());
                r.Close();

                string query = "DELETE FROM REPAIR WHERE Car=" + id + ";" + "DELETE FROM CARS WHERE CarID=" + id + "";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.ExecuteNonQuery();

                Load_data();
            }
        }
    }
}
