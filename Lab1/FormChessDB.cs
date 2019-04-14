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

namespace Lab1
{
    public partial class FormChessDB : Form
    {
        SqlConnection connection;
        SqlDataAdapter adapterTournament; 
        SqlDataAdapter adapterTournamentType; 
        DataSet dataSet;
        SqlCommandBuilder builder;
        string connectionString = "Data Source = DESKTOP-SMI6GU7;" + "Initial Catalog = Chess Tournament;" + "Integrated Security = true";

        public FormChessDB()
        {
            
            connection = new SqlConnection(connectionString);
            dataSet = new DataSet();
            InitializeComponent();
            LoadData();
        }


        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                adapterTournament.SelectCommand.Connection = connection;
                builder = new SqlCommandBuilder(adapterTournament);
                adapterTournament.Update(dataSet, "Tournament");
                dataSet.Tables["Tournament"].Clear();
                adapterTournament.Fill(dataSet, "Tournament");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadData()
        {
            adapterTournament = new SqlDataAdapter("SELECT * FROM Tournament", connection);
            adapterTournament.Fill(dataSet, "Tournament");

            adapterTournamentType = new SqlDataAdapter("SELECT * FROM TournamentType", connection);
            adapterTournamentType.Fill(dataSet, "TournamentType");

            DataRelation relation = new DataRelation("FK__Tournament__TTID", dataSet.Tables["TournamentType"].Columns["TTID"], dataSet.Tables["Tournament"].Columns["TTID"]);
            dataSet.Relations.Add(relation);

            BindingSource bindingSourceParent = new BindingSource(dataSet, "TournamentType");
            BindingSource bindingSourceChild = new BindingSource(bindingSourceParent, "FK__Tournament__TTID");

            dataGridView1.DataSource = bindingSourceParent;
            dataGridView2.DataSource = bindingSourceChild;
        }

       
    }
    
}
