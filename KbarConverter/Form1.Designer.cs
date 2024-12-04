namespace KbarConverter
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tbxMin = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbxKbarOutputBarTime = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxDataTimeOutputColumnFormat = new System.Windows.Forms.ComboBox();
            this.cbxHeaderFormat = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbxDataType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxFullTimeType = new System.Windows.Forms.ComboBox();
            this.tbxOutputName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxDeliveryDate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxMin
            // 
            this.tbxMin.Location = new System.Drawing.Point(242, 118);
            this.tbxMin.Margin = new System.Windows.Forms.Padding(5);
            this.tbxMin.Name = "tbxMin";
            this.tbxMin.Size = new System.Drawing.Size(139, 36);
            this.tbxMin.TabIndex = 0;
            this.tbxMin.Text = "1";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(546, 38);
            this.btnStart.Margin = new System.Windows.Forms.Padding(5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(263, 200);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "開始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cbxKbarOutputBarTime);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbxDataTimeOutputColumnFormat);
            this.groupBox1.Controls.Add(this.cbxHeaderFormat);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cbxDataType);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbxFullTimeType);
            this.groupBox1.Controls.Add(this.tbxOutputName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbxDeliveryDate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbxMin);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Location = new System.Drawing.Point(34, 34);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(869, 667);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "設定";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(67, 593);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 24);
            this.label10.TabIndex = 16;
            this.label10.Text = "Bar Time";
            // 
            // cbxKbarOutputBarTime
            // 
            this.cbxKbarOutputBarTime.FormattingEnabled = true;
            this.cbxKbarOutputBarTime.Items.AddRange(new object[] {
            "OpenTime",
            "CloseTime"});
            this.cbxKbarOutputBarTime.Location = new System.Drawing.Point(242, 593);
            this.cbxKbarOutputBarTime.Margin = new System.Windows.Forms.Padding(5);
            this.cbxKbarOutputBarTime.Name = "cbxKbarOutputBarTime";
            this.cbxKbarOutputBarTime.Size = new System.Drawing.Size(139, 32);
            this.cbxKbarOutputBarTime.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(67, 523);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 24);
            this.label6.TabIndex = 14;
            this.label6.Text = "日期時間";
            // 
            // cbxDataTimeOutputColumnFormat
            // 
            this.cbxDataTimeOutputColumnFormat.FormattingEnabled = true;
            this.cbxDataTimeOutputColumnFormat.Items.AddRange(new object[] {
            "OneColumn",
            "TwoColumn"});
            this.cbxDataTimeOutputColumnFormat.Location = new System.Drawing.Point(242, 523);
            this.cbxDataTimeOutputColumnFormat.Margin = new System.Windows.Forms.Padding(5);
            this.cbxDataTimeOutputColumnFormat.Name = "cbxDataTimeOutputColumnFormat";
            this.cbxDataTimeOutputColumnFormat.Size = new System.Drawing.Size(139, 32);
            this.cbxDataTimeOutputColumnFormat.TabIndex = 13;
            // 
            // cbxHeaderFormat
            // 
            this.cbxHeaderFormat.FormattingEnabled = true;
            this.cbxHeaderFormat.Items.AddRange(new object[] {
            "None",
            "Time,Open,High,Low,Close,Volume",
            "Date,Time,Open,High,Low,Close,TotalVolume"});
            this.cbxHeaderFormat.Location = new System.Drawing.Point(242, 442);
            this.cbxHeaderFormat.Margin = new System.Windows.Forms.Padding(5);
            this.cbxHeaderFormat.Name = "cbxHeaderFormat";
            this.cbxHeaderFormat.Size = new System.Drawing.Size(565, 32);
            this.cbxHeaderFormat.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(67, 446);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 24);
            this.label9.TabIndex = 11;
            this.label9.Text = "資料Header";
            // 
            // cbxDataType
            // 
            this.cbxDataType.FormattingEnabled = true;
            this.cbxDataType.Items.AddRange(new object[] {
            "Tick",
            "1minK"});
            this.cbxDataType.Location = new System.Drawing.Point(242, 34);
            this.cbxDataType.Margin = new System.Windows.Forms.Padding(5);
            this.cbxDataType.Name = "cbxDataType";
            this.cbxDataType.Size = new System.Drawing.Size(139, 32);
            this.cbxDataType.TabIndex = 10;
            this.cbxDataType.SelectedIndexChanged += new System.EventHandler(this.cbxDataType_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(67, 38);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 24);
            this.label8.TabIndex = 9;
            this.label8.Text = "資料類型";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(67, 366);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 24);
            this.label7.TabIndex = 8;
            this.label7.Text = "早午盤";
            // 
            // cbxFullTimeType
            // 
            this.cbxFullTimeType.FormattingEnabled = true;
            this.cbxFullTimeType.Items.AddRange(new object[] {
            "僅早盤",
            "僅午盤",
            "全時(早+午)"});
            this.cbxFullTimeType.Location = new System.Drawing.Point(242, 362);
            this.cbxFullTimeType.Margin = new System.Windows.Forms.Padding(5);
            this.cbxFullTimeType.Name = "cbxFullTimeType";
            this.cbxFullTimeType.Size = new System.Drawing.Size(139, 32);
            this.cbxFullTimeType.TabIndex = 7;
            // 
            // tbxOutputName
            // 
            this.tbxOutputName.Location = new System.Drawing.Point(242, 282);
            this.tbxOutputName.Margin = new System.Windows.Forms.Padding(5);
            this.tbxOutputName.Name = "tbxOutputName";
            this.tbxOutputName.Size = new System.Drawing.Size(139, 36);
            this.tbxOutputName.TabIndex = 6;
            this.tbxOutputName.Text = "output";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(67, 286);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "輸出檔名";
            // 
            // tbxDeliveryDate
            // 
            this.tbxDeliveryDate.Location = new System.Drawing.Point(242, 192);
            this.tbxDeliveryDate.Margin = new System.Windows.Forms.Padding(5);
            this.tbxDeliveryDate.Name = "tbxDeliveryDate";
            this.tbxDeliveryDate.Size = new System.Drawing.Size(139, 36);
            this.tbxDeliveryDate.TabIndex = 4;
            this.tbxDeliveryDate.Text = "yyyyMM";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 197);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "結算月";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 126);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "轉換幾分K";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 723);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 24);
            this.label4.TabIndex = 7;
            this.label4.Text = "說明";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 768);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(519, 24);
            this.label5.TabIndex = 8;
            this.label5.Text = "1.若結算月輸入yyyyMM則自動判定所有近月資料";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 881);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.groupBox1);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "Form1";
            this.Text = "台指期原始資料轉換程式";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxMin;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbxDeliveryDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxOutputName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbxFullTimeType;
        private System.Windows.Forms.ComboBox cbxDataType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbxHeaderFormat;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbxKbarOutputBarTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbxDataTimeOutputColumnFormat;
    }
}

