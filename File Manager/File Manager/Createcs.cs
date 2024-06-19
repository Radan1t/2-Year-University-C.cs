using System;
using System.Collections.Generic;
using System.IO; // For file operations
using System.Windows.Forms;

namespace File_Manager
{
    public partial class Createcs : Form
    {
        public Createcs(string path_for_create)
        {
            InitializeComponent();
            label3.Text = path_for_create;

            // Create a list of file types with their extensions
            var fileTypes = new List<FileType>
            {
                new FileType { Name = "Папка", Extension = "Folder" },
                new FileType { Name = "Ярлык", Extension = ".lnk" },
                new FileType { Name = "Microsoft Access Database", Extension = ".mdb" },
                new FileType { Name = "Точковий рисунок", Extension = ".bmp" },
                new FileType { Name = "Microsoft Word Document", Extension = ".docx" },
                new FileType { Name = "Microsoft Access Database", Extension = ".accdb" },
                new FileType { Name = "Microsoft Project Document", Extension = ".mpp" },
                new FileType { Name = "Microsoft PowerPoint Presentation", Extension = ".pptx" },
                new FileType { Name = "Microsoft Publisher Document", Extension = ".pub" },
                new FileType { Name = "Архив WinRAR", Extension = ".rar" },
                new FileType { Name = "Rich Text Format", Extension = ".rtf" },
                new FileType { Name = "Текстовий документ", Extension = ".txt" },
                new FileType { Name = "Microsoft Excel Worksheet", Extension = ".xlsx" },
                new FileType { Name = "Архив ZIP - WinRAR", Extension = ".zip" }
            };

            // Set the ComboBox properties
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "Extension";
            comboBox1.DataSource = fileTypes;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the text from textBox1
            string fileName = textBox1.Text;

            // Get the selected extension from comboBox1
            string selectedExtension = comboBox1.SelectedValue.ToString();

            // Determine if a folder is being created
            bool isFolder = selectedExtension == "Folder";

            // Combine the text and extension to form the file name
            string fullFileName = isFolder ? fileName : fileName + selectedExtension;

            // Get the path where the file should be created
            string path = label3.Text;

            // Combine the path and file name to get the full path
            string fullPath = Path.Combine(path, fullFileName);

            try
            {
                if (isFolder)
                {
                    // Ensure unique folder name
                    fullPath = GetUniquePath(fullPath, isDirectory: true);

                    // Create the directory
                    Directory.CreateDirectory(fullPath);
                }
                else
                {
                    // Ensure unique file name
                    fullPath = GetUniquePath(fullPath, isDirectory: false);

                    // Create the file
                    using (FileStream fs = File.Create(fullPath))
                    {
                        // Optionally write something to the file
                        // byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                        // fs.Write(info, 0, info.Length);
                    }
                }

                MessageBox.Show("File created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while creating the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private string GetUniquePath(string path, bool isDirectory)
        {
            string directory = Path.GetDirectoryName(path);
            string fileName = isDirectory ? Path.GetFileName(path) : Path.GetFileNameWithoutExtension(path);
            string extension = isDirectory ? "" : Path.GetExtension(path);

            int count = 1;
            string uniquePath = path;

            while (isDirectory ? Directory.Exists(uniquePath) : File.Exists(uniquePath))
            {
                uniquePath = Path.Combine(directory, $"{fileName} ({count}){extension}");
                count++;
            }

            return uniquePath;
        }
    }

    // Define a class to represent file types
    public class FileType
    {
        public string Name { get; set; }
        public string Extension { get; set; }
    }
}
