﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace seminar
{
    public partial class Machine : Form
    {
        MySqlConnection conn;
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataSet myds; // 資料集 

        public Machine()
        {
            InitializeComponent();

            string dbHost = "localhost";//資料庫位址
            string dbPort = "3306";//資料庫的port
            string dbUser = "root";//資料庫使用者帳號
            string dbPass = "5423";//資料庫使用者密碼
            string dbName = "Seminar";//資料庫名稱

            string connStr = "server=" + dbHost + ";port=" + dbPort + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            conn = new MySqlConnection(connStr);
            command = conn.CreateCommand();

        }

        private void Machine_Load(object sender, EventArgs e)
        {
            conn.Open();
            command.CommandText = "ALTER TABLE 機台資料表 AUTO_INCREMENT = 1;";
            command.ExecuteNonQuery();
            myds = new DataSet();
            adapter = new MySqlDataAdapter("SELECT * FROM 機台資料表;", conn);
            adapter.Fill(myds, "機台資料表");
            dataGridView1.DataSource = myds.Tables["機台資料表"];
            conn.Close();
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)  //dataGridView1每新增一行資料時觸發
        {
            //每新增一行資料時，把上一行的行號自動加入
            dataGridView1[0, e.Row.Index - 1].Value = e.Row.Index;
        }

        private void Machine_FormClosing(object sender, FormClosingEventArgs e)
        {

            int added = 0;
            int deleted = 0;
            int modified = 0;

            foreach (DataRow dr in myds.Tables["機台資料表"].Rows)

            {

                switch (dr.RowState)

                {

                    case DataRowState.Added:

                        added++;

                        break;

                    case DataRowState.Deleted:

                        deleted++;

                        break;

                    case DataRowState.Modified:

                        modified++;

                        break;

                    case DataRowState.Unchanged:

                        break;

                    default:

                        break;

                }

            }

            if (added!=0 || deleted!=0|| modified != 0)
            {
                String result = "";

                result += (added == 0 ? "" : "【新增】 " + added + "筆新資料\n")
                    + (deleted == 0 ? "" : "【刪除】 " + deleted + "筆資料\n")
                    + (modified == 0 ? "" : "【變更】 " + modified + "筆資料\n")
                    + "要儲存嗎？";

                DialogResult dialogResult = MessageBox.Show(result,"要儲存嗎？",MessageBoxButtons.YesNoCancel);

                if (dialogResult == DialogResult.Yes)
                {
                    conn.Open();
                    MySqlCommandBuilder mycb = new MySqlCommandBuilder(adapter);
                    adapter.Update(myds, "機台資料表");
                    conn.Close();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }

            }

        }

        private void dataGridView1_UserAddedRow_1(object sender, DataGridViewRowEventArgs e)
        {
            //每新增一行資料時，把上一行的行號自動加入
            dataGridView1[0, e.Row.Index - 1].Value = e.Row.Index;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //取得雙擊的行數與列數
            int icolumn = e.ColumnIndex;
            int irow = e.RowIndex;
            //若雙擊第0列（序號）時
            if (icolumn == 0)
            {
                dataGridView1.CurrentCell = dataGridView1[icolumn + 1, irow];  //把焦點轉移至下一格
                dataGridView1.BeginEdit(true);  //目前焦點儲存格進入編輯狀態
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommandBuilder mycb = new MySqlCommandBuilder(adapter);
            adapter.Update(myds, "機台資料表");
        }

    }
}
