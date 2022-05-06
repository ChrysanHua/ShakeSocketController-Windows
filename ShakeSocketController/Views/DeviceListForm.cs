using ShakeSocketController.Controller;
using ShakeSocketController.Model;
using ShakeSocketController.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShakeSocketController.Views
{
    /// <summary>
    /// 配置连接窗口
    /// </summary>
    public partial class DeviceListForm : Form
    {
        private readonly TransactionController _controller;
        private List<DeviceInfo> curDeviceList;

        public DeviceListForm(TransactionController controller)
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            listBoxDeviceList.Dock = DockStyle.Fill;
            this.PerformLayout();
            this.Icon = Icon.FromHandle(Resources.CtrlON128.GetHicon());

            _controller = controller;

            // TODO: 全局列表不能用现在这个IP字典，IP作为Key存在重复而被覆盖的可能性！
            //_controller.DeviceListChanged += _controller_DeviceListChanged;
            //_controller.TCPConnected += _controller_TCPConnected;
        }

        /// <summary>
        /// 加载设备连接信息到UI上
        /// </summary>
        private void LoadDeviceInfo(DeviceInfo info)
        {
            if (info == null)
            {
                //重置信息
                tbNickName.ResetText();
                tbDeviceID.ResetText();
                tbDeviceName.ResetText();
                tbUserName.ResetText();
                tbIP.ResetText();
                tbClientVersion.ResetText();
                cbAutoConnect.Checked = false;
                cbDisabled.Checked = false;
                labelDeviceState.ResetText();
                btnDeviceModify.Enabled = false;
                btnDeviceDelete.Enabled = false;
            }
            else
            {
                //设置设备信息
                tbNickName.Text = info.NickName;
                tbDeviceID.Text = info.DeviceID;
                tbDeviceName.Text = info.DeviceName;
                tbUserName.Text = info.UserName;
                tbIP.Text = info.IP.ToString();
                tbClientVersion.Text = info.ClientVersion;
                //勾选框
                cbAutoConnect.Checked = info.IsAutoConnect;
                cbDisabled.Checked = info.IsDisabled;
                //状态文本
                if (info.IsDisabled)
                {
                    labelDeviceState.Text = "已禁用";
                }
                else if (info.IsConnected)
                {
                    labelDeviceState.Text = "已连接";
                }
                else if (info.HadConnected)
                {
                    labelDeviceState.Text = "已保存";
                }
                else
                {
                    labelDeviceState.Text = string.Empty;
                }
                //可用性
                btnDeviceModify.Enabled = true;
                btnDeviceDelete.Enabled = true;
                if (info.IsConnected)
                {
                    // TODO: 已连接状态下，允不允许【禁用】和【删除】
                }
            }
        }

        /// <summary>
        /// 设备连接列表选中项变更事件
        /// </summary>
        private void lbDeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxDeviceList.SelectedIndex;
            Logging.Debug($"select index: {index}");
            if (index >= 0)
            {
                //加载选中项的设备信息
                //LoadDeviceInfo(curDeviceList[index]);
            }
            else
            {
                //重置信息
                LoadDeviceInfo(null);
            }
        }

        private void _controller_TCPConnected(object sender, EventArgs e)
        {
            listBoxDeviceList.Items.Clear();
            listBoxDeviceList.Items.Add(curDeviceList[0].NickName + "(已连接)");

        }

        private void _controller_DeviceListChanged(object sender, EventArgs e)
        {
            listBoxDeviceList.Items.Clear();
            curDeviceList = _controller.GetCurrentDeviceList().ToList();
            foreach (var item in curDeviceList)
            {
                listBoxDeviceList.Items.Add(item.NickName);
            }
            if (listBoxDeviceList.Items.Count > 0)
            {
                listBoxDeviceList.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 保存修改按钮
        /// </summary>
        private void btnDeviceModify_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        private void btnDeviceDelete_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 断开连接按钮
        /// </summary>
        private void btnDisConnect_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
