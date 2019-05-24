namespace ShakeSocketController.Views
{
    partial class DeviceListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbDeviceList = new System.Windows.Forms.ListBox();
            this.gbDeviceInfo = new System.Windows.Forms.GroupBox();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.tbDeviceName = new System.Windows.Forms.TextBox();
            this.tbNickName = new System.Windows.Forms.TextBox();
            this.lbIP = new System.Windows.Forms.Label();
            this.lbDeviceName = new System.Windows.Forms.Label();
            this.lbNickName = new System.Windows.Forms.Label();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.gbDeviceInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbDeviceList
            // 
            this.lbDeviceList.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDeviceList.FormattingEnabled = true;
            this.lbDeviceList.ItemHeight = 16;
            this.lbDeviceList.Location = new System.Drawing.Point(12, 12);
            this.lbDeviceList.Name = "lbDeviceList";
            this.lbDeviceList.Size = new System.Drawing.Size(207, 164);
            this.lbDeviceList.TabIndex = 0;
            this.lbDeviceList.SelectedIndexChanged += new System.EventHandler(this.lbDeviceList_SelectedIndexChanged);
            // 
            // gbDeviceInfo
            // 
            this.gbDeviceInfo.Controls.Add(this.tbIP);
            this.gbDeviceInfo.Controls.Add(this.tbDeviceName);
            this.gbDeviceInfo.Controls.Add(this.tbNickName);
            this.gbDeviceInfo.Controls.Add(this.lbIP);
            this.gbDeviceInfo.Controls.Add(this.lbDeviceName);
            this.gbDeviceInfo.Controls.Add(this.lbNickName);
            this.gbDeviceInfo.Location = new System.Drawing.Point(225, 12);
            this.gbDeviceInfo.Name = "gbDeviceInfo";
            this.gbDeviceInfo.Size = new System.Drawing.Size(204, 164);
            this.gbDeviceInfo.TabIndex = 1;
            this.gbDeviceInfo.TabStop = false;
            this.gbDeviceInfo.Text = "设备信息";
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(65, 96);
            this.tbIP.Name = "tbIP";
            this.tbIP.ReadOnly = true;
            this.tbIP.Size = new System.Drawing.Size(133, 21);
            this.tbIP.TabIndex = 1;
            // 
            // tbDeviceName
            // 
            this.tbDeviceName.Location = new System.Drawing.Point(65, 60);
            this.tbDeviceName.Name = "tbDeviceName";
            this.tbDeviceName.ReadOnly = true;
            this.tbDeviceName.Size = new System.Drawing.Size(133, 21);
            this.tbDeviceName.TabIndex = 1;
            // 
            // tbNickName
            // 
            this.tbNickName.Location = new System.Drawing.Point(65, 24);
            this.tbNickName.Name = "tbNickName";
            this.tbNickName.ReadOnly = true;
            this.tbNickName.Size = new System.Drawing.Size(133, 21);
            this.tbNickName.TabIndex = 1;
            // 
            // lbIP
            // 
            this.lbIP.AutoSize = true;
            this.lbIP.Location = new System.Drawing.Point(18, 99);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(41, 12);
            this.lbIP.TabIndex = 0;
            this.lbIP.Text = "IP地址";
            // 
            // lbDeviceName
            // 
            this.lbDeviceName.AutoSize = true;
            this.lbDeviceName.Location = new System.Drawing.Point(18, 63);
            this.lbDeviceName.Name = "lbDeviceName";
            this.lbDeviceName.Size = new System.Drawing.Size(41, 12);
            this.lbDeviceName.TabIndex = 0;
            this.lbDeviceName.Text = "设备名";
            // 
            // lbNickName
            // 
            this.lbNickName.AutoSize = true;
            this.lbNickName.Location = new System.Drawing.Point(6, 27);
            this.lbNickName.Name = "lbNickName";
            this.lbNickName.Size = new System.Drawing.Size(53, 12);
            this.lbNickName.TabIndex = 0;
            this.lbNickName.Text = "设备昵称";
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.Location = new System.Drawing.Point(256, 182);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisConnect.TabIndex = 2;
            this.btnDisConnect.Text = "断开连接";
            this.btnDisConnect.UseVisualStyleBackColor = true;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(348, 182);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // DeviceListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(441, 213);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDisConnect);
            this.Controls.Add(this.gbDeviceInfo);
            this.Controls.Add(this.lbDeviceList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DeviceListForm";
            this.Text = "设备列表";
            this.gbDeviceInfo.ResumeLayout(false);
            this.gbDeviceInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbDeviceList;
        private System.Windows.Forms.GroupBox gbDeviceInfo;
        private System.Windows.Forms.Label lbNickName;
        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.Label lbDeviceName;
        private System.Windows.Forms.TextBox tbNickName;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbDeviceName;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.Button btnClose;
    }
}