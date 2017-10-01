using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InspecthelperGUI
{
    public partial class AppSettings : Form
    {
        string origin_row1, origin_row2;
        string setting_path = $@"{Path.Combine(Path.GetTempPath(), Properties.Settings.Default.SettngsFilePath)}";
        public AppSettings()
        {
            InitializeComponent();
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit;
        }
        private void AppSettings_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            if (!(File.Exists(setting_path)))
            {
                WriteSetting(setting_path, $"#Application Name,Process Name"); //Generate data file if not exists
            }
            DGVRendering(dataGridView1, setting_path);
        }
        private void WriteSetting(string fpath,string content)
        {
            StreamWriter w = new StreamWriter(fpath,true);
            content = $@"{content.Substring(0,content.IndexOf("."))},{content}";
            w.WriteLine(content);     
            w.Close();
            DGVRendering(dataGridView1, setting_path);
        }
        private void DGVRendering(DataGridView DGV, string datasource)
        {
            DGV.DataSource = DataObject.DataTableDefinitionFromTextFile(datasource);
        }
        //Add Button
        private void addButton_Click(object sender, EventArgs e) //Add_fpath is a OpenFileDialog Control, it's not a code!
        {
            Add_fpath.Filter = "Executable File|*.exe";
            if(Add_fpath.ShowDialog() == DialogResult.Cancel)
            {
                //if user doesn't want to add it, just skip it.
            }else
            {
                StreamReader reader = new StreamReader(setting_path);
                var temptxt = reader.ReadToEnd();
                reader.Close();
                var addingString = System.IO.Path.GetFileName(Add_fpath.FileName);
                if (!temptxt.Contains(addingString)) {
                    WriteSetting(setting_path, System.IO.Path.GetFileName(Add_fpath.FileName));
                    button2.Enabled = true;
                }
            }
            
        }
        //Remove Button
        private void removeButton_Click(object sender, EventArgs e)
        {
            RemoveRow(new List<string>(){ "#Application Name","Process Name"});

        }

        private void RemoveRow(List<string> RemoveHeaderTag)
        {
            StreamReader reader = new StreamReader(setting_path);
            try
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];
                //Convert.ToString(selectedRow.Cells["Application Path"].Value for selected row value to string
                string temp_text = reader.ReadToEnd();
                reader.Close();
                var replace = string.Empty;
                foreach(var tag in RemoveHeaderTag)
                {
                    replace += $"{Convert.ToString(selectedRow.Cells[tag].Value)},";
                }
                temp_text = temp_text.Replace(replace.Substring(0, replace.LastIndexOf(",")), "");
                StreamWriter writer = new StreamWriter(setting_path);
                writer.Write(temp_text);
                writer.Close();
                File.WriteAllLines(setting_path, File.ReadAllLines(setting_path).Where(l => !string.IsNullOrWhiteSpace(l)));
                DGVRendering(dataGridView1, setting_path);
                //http://stackoverflow.com/questions/10615561/deleting-line-of-text-from-from-txt-file
                if (dataGridView1.RowCount == 0)
                {
                    button2.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                reader.Close();
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];
                origin_row1 = Convert.ToString(selectedRow.Cells["#Application Name"].Value);
                origin_row2 = Convert.ToString(selectedRow.Cells["Process Name"].Value);
            }
            catch {
                //pass
            }
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            StreamReader reader = new StreamReader(setting_path);
            string temp_text = reader.ReadToEnd();
            reader.Close();
            if (!string.IsNullOrEmpty(origin_row1) && !string.IsNullOrEmpty(origin_row2))
            {
               
                try
                {
                    int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];
                    string current_row_1 = Convert.ToString(selectedRow.Cells["#Application Name"].Value);
                    string current_row_2 = Convert.ToString(selectedRow.Cells["Process Name"].Value);
                    temp_text = temp_text.Replace($@"{origin_row1},{origin_row2}", $@"{current_row_1},{current_row_2}");
                    reader.Close();
                    StreamWriter writer = new StreamWriter(setting_path);
                    writer.Write(temp_text);
                    writer.Close();
                    File.WriteAllLines(setting_path, File.ReadAllLines(setting_path).Where(l => !string.IsNullOrWhiteSpace(l)));
                    DGVRendering(dataGridView1, setting_path);
                }
                catch (Exception ex)
                {
                    reader.Close();
                }
            }
            else
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];
                string current_row_1 = Convert.ToString(selectedRow.Cells["#Application Name"].Value);
                string current_row_2 = Convert.ToString(selectedRow.Cells["Process Name"].Value);
                temp_text += $"\n{current_row_1},{current_row_2}";
                StreamWriter writer = new StreamWriter(setting_path);
                writer.Write(temp_text);
                writer.Close();
                File.WriteAllLines(setting_path, File.ReadAllLines(setting_path).Where(l => !string.IsNullOrWhiteSpace(l)));
                DGVRendering(dataGridView1, setting_path);
            }
           
        }


        private void DGV_onCellEditing()
        {
            StreamReader reader = new StreamReader(setting_path);
            string temp_text = reader.ReadToEnd();
            reader.Close();
            if (!string.IsNullOrEmpty(origin_row1) && !string.IsNullOrEmpty(origin_row2))
            {

                try
                {
                    int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];
                    string current_row_1 = Convert.ToString(selectedRow.Cells["#Application Name"].Value);
                    string current_row_2 = Convert.ToString(selectedRow.Cells["Process Name"].Value);
                    temp_text = temp_text.Replace($@"{origin_row1},{origin_row2}", $@"{current_row_1},{current_row_2}");
                    reader.Close();
                    StreamWriter writer = new StreamWriter(setting_path);
                    writer.Write(temp_text);
                    writer.Close();
                    File.WriteAllLines(setting_path, File.ReadAllLines(setting_path).Where(l => !string.IsNullOrWhiteSpace(l)));
                    DGVRendering(dataGridView1, setting_path);
                }
                catch (Exception ex)
                {
                    reader.Close();
                }
            }
            else
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];
                string current_row_1 = Convert.ToString(selectedRow.Cells["#Application Name"].Value);
                string current_row_2 = Convert.ToString(selectedRow.Cells["Process Name"].Value);
                temp_text += $"\n{current_row_1},{current_row_2}";
                StreamWriter writer = new StreamWriter(setting_path);
                writer.Write(temp_text);
                writer.Close();
                File.WriteAllLines(setting_path, File.ReadAllLines(setting_path).Where(l => !string.IsNullOrWhiteSpace(l)));
                DGVRendering(dataGridView1, setting_path);
            }

        }
    }
}
