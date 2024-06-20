using File_Manager.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace File_Manager
{
    public partial class Form1 : Form
    {
        private ListView fileListView;
        private ComboBox driveComboBox;

        private string currentPath; // Змінна для зберігання поточного шляху
        private List<string> File_path = new List<string>();
        private string copiedPath;
        private string cutPath;
        private bool isCutOperation;
        private ListViewColumnSorter lvwColumnSorter;
        bool info=false;
        string path_for_create { set; get; }

        public Form1(string initialPath = null, DriveInfo selectedDrive = null)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            groupBox3.Hide();
            // Відключаємо кнопку максимізації
            this.MaximizeBox = false;
            // Assuming listView1 is the name of the ListView control in your designer
            fileListView = listView1;
            lvwColumnSorter = new ListViewColumnSorter();
            fileListView.ListViewItemSorter = lvwColumnSorter;
            // Configure ListView properties
            fileListView.View = View.Details;
            fileListView.FullRowSelect = true;
            fileListView.GridLines = true;

            // Add columns if not already added in designer
            if (fileListView.Columns.Count == 0)
            {
                fileListView.Columns.Add("Name", 150);
                fileListView.Columns.Add("Size", 70);
                fileListView.Columns.Add("Type", 100);
                fileListView.Columns.Add("Date Modified", 150);
            }

            // Attach event handlers
            fileListView.Click += ListView_Info;
            fileListView.DoubleClick += ListView_DoubleClick;
            fileListView.AfterLabelEdit += fileListView_AfterLabelEdit;
            fileListView.ColumnClick += ListView_ColumnClick;
            fileListView.MouseDown += FileListView_MouseDown; // Attach MouseDown event

            // Initialize ImageList for file icons
            ImageList imageList = new ImageList();
            imageList.Images.Add("folder", Properties.Resources.folder); // Assuming you have a folder_icon resource
            imageList.Images.Add("file", Properties.Resources.new_document__1_); // Assuming you have a file_icon resource
            fileListView.SmallImageList = imageList;

            // Assuming driveComboBox is intentionally declared in the code-behind
            driveComboBox = comboBox2;
            driveComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            driveComboBox.SelectedIndexChanged += DriveComboBox_SelectedIndexChanged;
            this.Controls.Add(driveComboBox);

            // Populate ComboBox with available drives
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    driveComboBox.Items.Add(drive);
                }
            }

            // Set the selected drive if provided
            if (selectedDrive != null)
            {
                driveComboBox.SelectedItem = selectedDrive;
            }

            if (!string.IsNullOrEmpty(initialPath))
            {
                LoadFiles(initialPath);
            }
            else
            {
                // Load files for the default selected drive
                if (driveComboBox.Items.Count > 0)
                {
                    driveComboBox.SelectedIndex = 0;
                    DriveInfo defaultDrive = (DriveInfo)driveComboBox.SelectedItem;
                    LoadFiles(defaultDrive.Name);
                }
            }

            // Attach KeyDown event handler for the form
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            // Enable KeyPreview to ensure form captures key events
            this.KeyPreview = true;
        }
        private void FileListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Control)
            {
                ListViewItem item = fileListView.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    string itemName = item.Text;
                    string fullPath = Path.Combine(currentPath, itemName);

                    if (Directory.Exists(fullPath))
                    {
                        DriveInfo selectedDrive = (DriveInfo)driveComboBox.SelectedItem;
                        Form1 newForm = new Form1(fullPath, selectedDrive); // Pass the full path and selected drive to the new form
                        newForm.Show(); // Show the new form
                    }
                }
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopySelectedItem();
                e.SuppressKeyPress = true; // Prevent the 'ding' sound
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                PasteCopiedItem();
                e.SuppressKeyPress = true; // Prevent the 'ding' sound
            }
            else if(e.Control && e.KeyCode == Keys.X)
            {
                CutSelectedItem();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.Delete)
            {
                DeliteItem();
                e.SuppressKeyPress = true;
            }
        }
        private void CopySelectedItem()
        {
            if (fileListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = fileListView.SelectedItems[0];
                string itemName = selectedItem.Text;
                copiedPath = Path.Combine(currentPath, itemName);
                isCutOperation=false;
                cutPath=null;
               // MessageBox.Show($"Copied: {copiedPath}", "Copy", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No item selected to copy.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void CutSelectedItem() {
            if (fileListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = fileListView.SelectedItems[0];
                string itemName = selectedItem.Text;
                cutPath = Path.Combine(currentPath, itemName);
                isCutOperation = true;
                copiedPath = null;
                //MessageBox.Show($"Cut: {cutPath}", "Cut", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No item selected to cut.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void PasteCopiedItem()
        {
            if (string.IsNullOrEmpty(copiedPath) && string.IsNullOrEmpty(cutPath))
            {
                MessageBox.Show("No item copied or cut.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sourcePath = isCutOperation ? cutPath : copiedPath;
                string destinationPath = Path.Combine(currentPath, Path.GetFileName(sourcePath));

                if (Directory.Exists(sourcePath))
                {
                    // If the item is a directory, move or copy the directory
                    if (isCutOperation)
                    {
                        if (IsSameDrive(sourcePath, destinationPath))
                        {
                            Directory.Move(sourcePath, destinationPath);
                        }
                        else
                        {
                            CopyDirectory(sourcePath, destinationPath);
                            Directory.Delete(sourcePath, true);
                        }
                    }
                    else
                    {
                        CopyDirectory(sourcePath, destinationPath);
                    }
                }
                else if (File.Exists(sourcePath))
                {
                    // If the item is a file, move or copy the file
                    if (isCutOperation)
                    {
                        if (IsSameDrive(sourcePath, destinationPath))
                        {
                            File.Move(sourcePath, destinationPath);
                        }
                        else
                        {
                            File.Copy(sourcePath, destinationPath);
                            File.Delete(sourcePath);
                        }
                    }
                    else
                    {
                        File.Copy(sourcePath, destinationPath);
                    }
                }
                else
                {
                    MessageBox.Show("The copied or cut item no longer exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

               // MessageBox.Show($"Pasted to: {destinationPath}", "Paste", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadFiles(currentPath); // Refresh the ListView

                // Only clear cutPath and reset isCutOperation if it was a cut operation
                if (isCutOperation)
                {
                    cutPath = null;
                    isCutOperation = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error pasting item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsSameDrive(string path1, string path2)
        {
            return Path.GetPathRoot(path1).Equals(Path.GetPathRoot(path2), StringComparison.OrdinalIgnoreCase);
        }
        private void ListView_Info(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = fileListView.SelectedItems[0];
                string itemName = selectedItem.Text;
                string fullPath = Path.Combine(currentPath, itemName);

                // Update the textBox1 with the full path
                textBox1.Text = fullPath;
                string fileExtension = Path.GetExtension(fullPath);

                // Determine file type and update UI accordingly
                switch (fileExtension.ToLower())
                {
                    // Images
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                        pictureBox1.Image = Properties.Resources.png__1_; // Replace with your image resource
                        break;

                    // Documents
                    case ".txt":
                    case ".doc":
                    case ".docx":
                        pictureBox1.Image = Properties.Resources.google_docs__1_; // Replace with your document icon resource
                        break;

                    // PDF
                    case ".pdf":
                        pictureBox1.Image = Properties.Resources.pdf__1___1_; // Replace with your PDF icon resource
                        break;

                    // Folders
                    case "":
                        pictureBox1.Image = Properties.Resources.folder__2_; // Replace with your folder icon resource
                        break;

                    // Default (unknown file type)
                    default:
                        pictureBox1.Image = Properties.Resources.new_document__2_; // Clear PictureBox if file type is unknown
                        break;
                }

                // Display full path in label3
                label3.Text = itemName;
                if (fileExtension.ToLower() != "")
                {
                    label7.Text = $"Type: {fileExtension.ToLower()}";
                }
                else
                {
                    label7.Text = $"Type: folder";
                }
                label8.Text = $"Full Path: {fullPath}";

                // Display last modification date in label9
                if (File.Exists(fullPath))
                {
                    DateTime lastModified = File.GetLastWriteTime(fullPath);
                    label9.Text = $"Last update: {lastModified}";
                }
                else if (Directory.Exists(fullPath))
                {
                    DateTime lastModified = Directory.GetLastWriteTime(fullPath);
                    label9.Text = $"Last update: {lastModified}";
                }
                else
                {
                    label9.Text = "Last update: N/A";
                }
            }
        }


        // Event handler for ListView double-click
        private void ListView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (fileListView.SelectedItems.Count > 0)
                {
                    ListViewItem selectedItem = fileListView.SelectedItems[0];
                    string itemName = selectedItem.Text;
                    string fullPath = Path.Combine(currentPath, itemName);

                    // Check if it's a folder
                    if (Directory.Exists(fullPath))
                    {
                        LoadFiles(fullPath); // Load contents of the selected folder
                        currentPath = fullPath; // Update the current path
                        File_path.Add(currentPath);
                    }
                    else if (File.Exists(fullPath))
                    {
                        // If it's a file, open it
                        System.Diagnostics.Process.Start(fullPath);
                    }
                    else
                    {
                        // If it's neither a folder nor a file, show an error message
                        MessageBox.Show("The selected item is neither a file nor a directory.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }


        // Event handler for drive selection change
        private void DriveComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriveInfo selectedDrive = (DriveInfo)driveComboBox.SelectedItem;
            LoadFiles(selectedDrive.Name);
            currentPath = selectedDrive.Name; // Оновлюємо поточний шлях
            textBox1.Text= selectedDrive.Name;
            // Відображаємо загальний розмір диска в гігабайтах
            label6.Text = $"Всього : {Math.Round((double)selectedDrive.TotalSize / Math.Pow(2, 30), 2)} GB";
            label4.Text = $"Вільно: {Math.Round((double)selectedDrive.TotalFreeSpace / Math.Pow(2, 30), 2)} GB";

            progressBar1.Maximum = (int)(selectedDrive.TotalSize / Math.Pow(2, 20)); 
            progressBar1.Value = (int)((selectedDrive.TotalSize - selectedDrive.TotalFreeSpace) / Math.Pow(2, 20)); 

        }

        // Function to load files into ListView from selected drive or directory
        private void LoadFiles(string path)
        {
            try
            {
                currentPath = path; // Update the current path
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                DirectoryInfo[] directories = directoryInfo.GetDirectories();
                FileInfo[] files = directoryInfo.GetFiles();
                path_for_create = currentPath;
                fileListView.Items.Clear();

                foreach (DirectoryInfo directory in directories)
                {
                    ListViewItem item = new ListViewItem(directory.Name);
                    item.SubItems.Add("");
                    item.SubItems.Add("Folder");
                    item.SubItems.Add(directory.LastWriteTime.ToString());
                    item.ImageKey = "folder";
                    fileListView.Items.Add(item);
                }

                foreach (FileInfo file in files)
                {
                    ListViewItem item = new ListViewItem(file.Name);
                    item.SubItems.Add(file.Length.ToString());
                    item.SubItems.Add(file.Extension);
                    item.SubItems.Add(file.LastWriteTime.ToString());
                    item.ImageKey = "file";
                    fileListView.Items.Add(item);
                }

                textBox1.Text = path; // Update the textBox1 with the new path
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            currentPath = textBox1.Text;
            if (string.IsNullOrEmpty(currentPath))
            {
                return;
            }

            // Перевіряємо, чи є наявний символ '\'
            bool pathEndsWithBackslash = currentPath.EndsWith("\\");

            // Отримуємо останній індекс символу '\'
            int lastIndex = currentPath.LastIndexOf('\\');

            // Перевіряємо, чи currentPath є кореневим шляхом 'C:\'
            if (lastIndex == 2 && currentPath.Length == 3 && currentPath[1] == ':')
            {
                //currentPath += "\\"; // Додаємо символ '\', щоб відобразити кореневий шлях 'C:\'
            }
            else if (lastIndex > 0)
            {
                currentPath = currentPath.Substring(0, lastIndex);
            }

            // Якщо після видалення останнього символу не залишилося '\', додаємо його
            if (lastIndex == 2)
            {
                currentPath += "\\";
            }

            // Перевіряємо, чи в кінці є подвійний символ '\', і замінюємо його на одиничний '\'
            if (currentPath.EndsWith("\\\\"))
            {
                currentPath = currentPath.Substring(0, currentPath.Length - 1);
            }

            // Оновлюємо textBox1 і завантажуємо файли
            textBox1.Text = currentPath;

            LoadFiles(currentPath);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (File_path.Count > 0)
            {
                // Отримуємо перший елемент зі списку File_path

                string firstPath = File_path[0].ToString();

                // Оновлюємо textBox1 з отриманим першим шляхом
                textBox1.Text = firstPath;

                // Видаляємо перший елемент зі списку File_path
                File_path.RemoveAt(0);

                // Завантажуємо файли за новим шляхом (першим елементом з File_path)
                LoadFiles(firstPath);
            }
            else
            {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadFiles(currentPath);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (fileListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = fileListView.SelectedItems[0];
                fileListView.LabelEdit = true; // Enable editing mode
                selectedItem.BeginEdit(); // Begin edit mode on the selected item
            }
        }

        private void fileListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                ListViewItem selectedItem = fileListView.Items[e.Item];
                string oldName = selectedItem.Text;
                string newName = e.Label;
                string oldFullPath = Path.Combine(currentPath, oldName);
                string newFullPath = Path.Combine(currentPath, newName);

                try
                {
                    if (File.Exists(oldFullPath))
                    {
                        File.Move(oldFullPath, newFullPath);
                    }
                    else if (Directory.Exists(oldFullPath))
                    {
                        Directory.Move(oldFullPath, newFullPath);
                    }

                    // Update the ListViewItem's text with the new label
                    selectedItem.Text = newName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error renaming item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.CancelEdit = true; // Cancel the edit operation if there's an error
                }
            }
            fileListView.LabelEdit = false; // Disable label editing after editing is complete
        }

        private void button5_Click(object sender, EventArgs e)
        {
            currentPath = textBox1.Text;
            if (string.IsNullOrEmpty(currentPath))
            {
                return;
            }

            currentPath = currentPath.Substring(0, 3);

            textBox1.Text = currentPath;

            LoadFiles(currentPath);
        }
        private void DeliteItem()
        {
            if (fileListView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = fileListView.SelectedItems[0];
                string itemName = selectedItem.Text;
                string fullPath = Path.Combine(currentPath, itemName);

                // Confirm deletion
                DialogResult result = MessageBox.Show($"Are you sure you want to delete {itemName}?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (Directory.Exists(fullPath))
                        {
                            Directory.Delete(fullPath, true);
                        }
                        else if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                        else
                        {
                            MessageBox.Show("Selected item does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        fileListView.Items.Remove(selectedItem);
                        MessageBox.Show($"{itemName} has been deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting item: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No item selected to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            DeliteItem();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            CopySelectedItem();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            PasteCopiedItem();
        }

        // Utility method to copy directories
        private void CopyDirectory(string sourceDir, string destinationDir)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(tempPath, false);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destinationDir, subdir.Name);
                CopyDirectory(subdir.FullName, tempPath);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CutSelectedItem();
        }

        private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options
            fileListView.Sort();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Createcs createcs = new Createcs(path_for_create); 
            createcs.ShowDialog();
            LoadFiles(currentPath);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (info == false)
            {
                groupBox3.Show();
                info = true;
            }
            else
            {
                groupBox3.Hide();
                info = false;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Author form = new Author();
            form.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            // Створюємо новий екземпляр ColorDialog
            ColorDialog colorDialog = new ColorDialog();
            System.Drawing.Color color = groupBox3.BackColor;
            // Показуємо діалогове вікно і перевіряємо, чи користувач натиснув ОК
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Встановлюємо вибраний колір як фон для головної форми
                this.BackColor = colorDialog.Color;
                groupBox4.BackColor = color;
                groupBox3.BackColor = color;
                groupBox2.BackColor = color;
                groupBox1.BackColor = color;

            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                groupBox4.BackColor = colorDialog.Color;
                groupBox3.BackColor = colorDialog.Color; 
                groupBox2.BackColor = colorDialog.Color; 
                groupBox1.BackColor = colorDialog.Color; 


            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            // Створюємо новий екземпляр FontDialog
            FontDialog fontDialog = new FontDialog();

            // Показуємо діалогове вікно і перевіряємо, чи користувач натиснув ОК
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                // Встановлюємо вибраний шрифт для всієї форми та її контролів
                SetFont(this, fontDialog.Font);
            }
        }
        private void SetFont(Control control, Font font)
        {
            control.Font = font;
            foreach (Control childControl in control.Controls)
            {
                SetFont(childControl, font);
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }

}