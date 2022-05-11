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
            this.listBoxDeviceList = new System.Windows.Forms.ListBox();
            this.gbDeviceInfo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDeviceDelete = new System.Windows.Forms.Button();
            this.btnDeviceModify = new System.Windows.Forms.Button();
            this.labelDeviceState = new System.Windows.Forms.Label();
            this.tbClientVersion = new System.Windows.Forms.TextBox();
            this.labelClientVersion = new System.Windows.Forms.Label();
            this.labelUserName = new System.Windows.Forms.Label();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.tbDeviceID = new System.Windows.Forms.TextBox();
            this.labelIP = new System.Windows.Forms.Label();
            this.labelNickName = new System.Windows.Forms.Label();
            this.tbDeviceName = new System.Windows.Forms.TextBox();
            this.tbNickName = new System.Windows.Forms.TextBox();
            this.labelDeviceID = new System.Windows.Forms.Label();
            this.labelDeviceName = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.cbAutoConnect = new System.Windows.Forms.CheckBox();
            this.cbDisabled = new System.Windows.Forms.CheckBox();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.gbDeviceInfo.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxDeviceList
            // 
            this.listBoxDeviceList.FormattingEnabled = true;
            this.listBoxDeviceList.IntegralHeight = false;
            this.listBoxDeviceList.ItemHeight = 12;
            this.listBoxDeviceList.Location = new System.Drawing.Point(9, 9);
            this.listBoxDeviceList.Name = "listBoxDeviceList";
            this.listBoxDeviceList.Size = new System.Drawing.Size(180, 152);
            this.listBoxDeviceList.TabIndex = 0;
            this.listBoxDeviceList.SelectedIndexChanged += new System.EventHandler(this.lbDeviceList_SelectedIndexChanged);
            // 
            // gbDeviceInfo
            // 
            this.gbDeviceInfo.AutoSize = true;
            this.gbDeviceInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbDeviceInfo.Controls.Add(this.tableLayoutPanel2);
            this.gbDeviceInfo.Location = new System.Drawing.Point(198, 9);
            this.gbDeviceInfo.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.gbDeviceInfo.Name = "gbDeviceInfo";
            this.gbDeviceInfo.Padding = new System.Windows.Forms.Padding(6);
            this.gbDeviceInfo.Size = new System.Drawing.Size(249, 267);
            this.gbDeviceInfo.TabIndex = 1;
            this.gbDeviceInfo.TabStop = false;
            this.gbDeviceInfo.Text = "设备连接";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.labelDeviceState, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.tbClientVersion, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.labelClientVersion, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.labelUserName, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.tbIP, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.tbDeviceID, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelIP, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.labelNickName, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbDeviceName, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbNickName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelDeviceID, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelDeviceName, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbUserName, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.cbAutoConnect, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.cbDisabled, 1, 7);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 20);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 9;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(237, 241);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.btnDeviceDelete, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnDeviceModify, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(37, 212);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(162, 26);
            this.tableLayoutPanel3.TabIndex = 15;
            // 
            // btnDeviceDelete
            // 
            this.btnDeviceDelete.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnDeviceDelete.Location = new System.Drawing.Point(84, 3);
            this.btnDeviceDelete.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.btnDeviceDelete.Name = "btnDeviceDelete";
            this.btnDeviceDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceDelete.TabIndex = 1;
            this.btnDeviceDelete.Text = "删除";
            this.btnDeviceDelete.UseVisualStyleBackColor = true;
            this.btnDeviceDelete.Click += new System.EventHandler(this.btnDeviceDelete_Click);
            // 
            // btnDeviceModify
            // 
            this.btnDeviceModify.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDeviceModify.Location = new System.Drawing.Point(3, 3);
            this.btnDeviceModify.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.btnDeviceModify.Name = "btnDeviceModify";
            this.btnDeviceModify.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceModify.TabIndex = 0;
            this.btnDeviceModify.Text = "保存修改";
            this.btnDeviceModify.UseVisualStyleBackColor = true;
            this.btnDeviceModify.Click += new System.EventHandler(this.btnDeviceModify_Click);
            // 
            // labelDeviceState
            // 
            this.labelDeviceState.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDeviceState.AutoSize = true;
            this.labelDeviceState.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelDeviceState.Location = new System.Drawing.Point(15, 178);
            this.labelDeviceState.Name = "labelDeviceState";
            this.tableLayoutPanel2.SetRowSpan(this.labelDeviceState, 2);
            this.labelDeviceState.Size = new System.Drawing.Size(41, 12);
            this.labelDeviceState.TabIndex = 12;
            this.labelDeviceState.Text = "已连接";
            // 
            // tbClientVersion
            // 
            this.tbClientVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbClientVersion.Location = new System.Drawing.Point(62, 138);
            this.tbClientVersion.MaxLength = 512;
            this.tbClientVersion.Name = "tbClientVersion";
            this.tbClientVersion.ReadOnly = true;
            this.tbClientVersion.Size = new System.Drawing.Size(172, 21);
            this.tbClientVersion.TabIndex = 11;
            this.tbClientVersion.WordWrap = false;
            // 
            // labelClientVersion
            // 
            this.labelClientVersion.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelClientVersion.AutoSize = true;
            this.labelClientVersion.Location = new System.Drawing.Point(9, 142);
            this.labelClientVersion.Name = "labelClientVersion";
            this.labelClientVersion.Size = new System.Drawing.Size(47, 12);
            this.labelClientVersion.TabIndex = 10;
            this.labelClientVersion.Text = "APP版本";
            // 
            // labelUserName
            // 
            this.labelUserName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(15, 88);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(41, 12);
            this.labelUserName.TabIndex = 6;
            this.labelUserName.Text = "用户名";
            // 
            // tbIP
            // 
            this.tbIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIP.Location = new System.Drawing.Point(62, 111);
            this.tbIP.MaxLength = 512;
            this.tbIP.Name = "tbIP";
            this.tbIP.ReadOnly = true;
            this.tbIP.Size = new System.Drawing.Size(172, 21);
            this.tbIP.TabIndex = 9;
            this.tbIP.WordWrap = false;
            // 
            // tbDeviceID
            // 
            this.tbDeviceID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDeviceID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbDeviceID.Location = new System.Drawing.Point(62, 30);
            this.tbDeviceID.MaxLength = 512;
            this.tbDeviceID.Name = "tbDeviceID";
            this.tbDeviceID.ReadOnly = true;
            this.tbDeviceID.Size = new System.Drawing.Size(172, 21);
            this.tbDeviceID.TabIndex = 3;
            this.tbDeviceID.WordWrap = false;
            // 
            // labelIP
            // 
            this.labelIP.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(15, 115);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(41, 12);
            this.labelIP.TabIndex = 8;
            this.labelIP.Text = "IP地址";
            // 
            // labelNickName
            // 
            this.labelNickName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelNickName.AutoSize = true;
            this.labelNickName.Location = new System.Drawing.Point(3, 7);
            this.labelNickName.Name = "labelNickName";
            this.labelNickName.Size = new System.Drawing.Size(53, 12);
            this.labelNickName.TabIndex = 0;
            this.labelNickName.Text = "设备昵称";
            // 
            // tbDeviceName
            // 
            this.tbDeviceName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDeviceName.Location = new System.Drawing.Point(62, 57);
            this.tbDeviceName.MaxLength = 512;
            this.tbDeviceName.Name = "tbDeviceName";
            this.tbDeviceName.ReadOnly = true;
            this.tbDeviceName.Size = new System.Drawing.Size(172, 21);
            this.tbDeviceName.TabIndex = 5;
            this.tbDeviceName.WordWrap = false;
            // 
            // tbNickName
            // 
            this.tbNickName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbNickName.Location = new System.Drawing.Point(62, 3);
            this.tbNickName.MaxLength = 512;
            this.tbNickName.Name = "tbNickName";
            this.tbNickName.Size = new System.Drawing.Size(172, 21);
            this.tbNickName.TabIndex = 1;
            this.tbNickName.WordWrap = false;
            // 
            // labelDeviceID
            // 
            this.labelDeviceID.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDeviceID.AutoSize = true;
            this.labelDeviceID.Location = new System.Drawing.Point(15, 34);
            this.labelDeviceID.Name = "labelDeviceID";
            this.labelDeviceID.Size = new System.Drawing.Size(41, 12);
            this.labelDeviceID.TabIndex = 2;
            this.labelDeviceID.Text = "设备ID";
            // 
            // labelDeviceName
            // 
            this.labelDeviceName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDeviceName.AutoSize = true;
            this.labelDeviceName.Location = new System.Drawing.Point(15, 61);
            this.labelDeviceName.Name = "labelDeviceName";
            this.labelDeviceName.Size = new System.Drawing.Size(41, 12);
            this.labelDeviceName.TabIndex = 4;
            this.labelDeviceName.Text = "设备名";
            // 
            // tbUserName
            // 
            this.tbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUserName.Location = new System.Drawing.Point(62, 84);
            this.tbUserName.MaxLength = 512;
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.ReadOnly = true;
            this.tbUserName.Size = new System.Drawing.Size(172, 21);
            this.tbUserName.TabIndex = 7;
            this.tbUserName.WordWrap = false;
            // 
            // cbAutoConnect
            // 
            this.cbAutoConnect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbAutoConnect.AutoSize = true;
            this.cbAutoConnect.Location = new System.Drawing.Point(62, 165);
            this.cbAutoConnect.Name = "cbAutoConnect";
            this.cbAutoConnect.Size = new System.Drawing.Size(72, 16);
            this.cbAutoConnect.TabIndex = 13;
            this.cbAutoConnect.Text = "自动连接";
            this.cbAutoConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbAutoConnect.UseVisualStyleBackColor = true;
            // 
            // cbDisabled
            // 
            this.cbDisabled.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbDisabled.AutoSize = true;
            this.cbDisabled.Location = new System.Drawing.Point(62, 187);
            this.cbDisabled.Name = "cbDisabled";
            this.cbDisabled.Size = new System.Drawing.Size(48, 16);
            this.cbDisabled.TabIndex = 14;
            this.cbDisabled.Text = "禁用";
            this.cbDisabled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cbDisabled.UseVisualStyleBackColor = true;
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnDisConnect.Location = new System.Drawing.Point(0, 3);
            this.btnDisConnect.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisConnect.TabIndex = 0;
            this.btnDisConnect.Text = "断开连接";
            this.btnDisConnect.UseVisualStyleBackColor = true;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(81, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.gbDeviceInfo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.listBoxDeviceList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(464, 341);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnDisConnect, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnClose, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(299, 294);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(156, 26);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // DeviceListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(464, 341);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeviceListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置连接";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeviceListForm_FormClosed);
            this.Shown += new System.EventHandler(this.DeviceListForm_Shown);
            this.gbDeviceInfo.ResumeLayout(false);
            this.gbDeviceInfo.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxDeviceList;
        private System.Windows.Forms.GroupBox gbDeviceInfo;
        private System.Windows.Forms.Label labelNickName;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Label labelDeviceName;
        private System.Windows.Forms.TextBox tbNickName;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbDeviceName;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelDeviceID;
        private System.Windows.Forms.TextBox tbDeviceID;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label labelClientVersion;
        private System.Windows.Forms.TextBox tbClientVersion;
        private System.Windows.Forms.Label labelDeviceState;
        private System.Windows.Forms.CheckBox cbAutoConnect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnDeviceDelete;
        private System.Windows.Forms.Button btnDeviceModify;
        private System.Windows.Forms.CheckBox cbDisabled;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}