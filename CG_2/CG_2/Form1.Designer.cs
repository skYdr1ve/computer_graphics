namespace CG_2
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dpiDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pixelFormatDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComressionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageInfoBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.imageInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageInfoBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(363, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Get Info";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dpiDataGridViewTextBoxColumn,
            this.pixelFormatDataGridViewTextBoxColumn,
            this.ComressionType});
            this.dataGridView1.DataSource = this.imageInfoBindingSource1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(776, 397);
            this.dataGridView1.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ImageName";
            this.dataGridViewTextBoxColumn1.HeaderText = "ImageName";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 200;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ImageSize";
            this.dataGridViewTextBoxColumn2.HeaderText = "ImageSize";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dpiDataGridViewTextBoxColumn
            // 
            this.dpiDataGridViewTextBoxColumn.DataPropertyName = "Dpi";
            this.dpiDataGridViewTextBoxColumn.HeaderText = "Dpi";
            this.dpiDataGridViewTextBoxColumn.Name = "dpiDataGridViewTextBoxColumn";
            // 
            // pixelFormatDataGridViewTextBoxColumn
            // 
            this.pixelFormatDataGridViewTextBoxColumn.DataPropertyName = "PixelFormat";
            this.pixelFormatDataGridViewTextBoxColumn.HeaderText = "PixelFormat";
            this.pixelFormatDataGridViewTextBoxColumn.Name = "pixelFormatDataGridViewTextBoxColumn";
            // 
            // ComressionType
            // 
            this.ComressionType.DataPropertyName = "ComressionType";
            this.ComressionType.HeaderText = "ComressionType";
            this.ComressionType.Name = "ComressionType";
            this.ComressionType.Width = 150;
            // 
            // imageInfoBindingSource1
            // 
            this.imageInfoBindingSource1.DataSource = typeof(CG_2.ImageInfo);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // imageInfoBindingSource
            // 
            this.imageInfoBindingSource.DataSource = typeof(CG_2.ImageInfo);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 415);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "CG_2";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageInfoBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageInfoBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource imageInfoBindingSource;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.DataGridViewTextBoxColumn imagenameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imagesizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource imageInfoBindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dpiDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pixelFormatDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComressionType;
        private System.Windows.Forms.Label label1;
    }
}

