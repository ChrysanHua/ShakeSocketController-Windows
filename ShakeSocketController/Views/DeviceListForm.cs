using ShakeSocketController.Controller;
using ShakeSocketController.Model;
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
    public partial class DeviceListForm : Form
    {
        private TransactionController _controller;
        private List<DeviceInfo> curDeviceList;

        public DeviceListForm(TransactionController controller)
        {
            InitializeComponent();
            _controller = controller;

            _controller.DeviceListChanged += _controller_DeviceListChanged;
            _controller.TCPConnected += _controller_TCPConnected;
        }

        private void _controller_TCPConnected(object sender, EventArgs e)
        {
            lbDeviceList.Items.Clear();
            lbDeviceList.Items.Add(curDeviceList[0].NickName + "(已连接)");

        }

        private void _controller_DeviceListChanged(object sender, EventArgs e)
        {
            lbDeviceList.Items.Clear();
            curDeviceList = _controller.GetCurrentDeviceList().ToList();
            foreach (var item in curDeviceList)
            {
                lbDeviceList.Items.Add(item.NickName);
            }
            if (lbDeviceList.Items.Count > 0)
            {
                lbDeviceList.SelectedIndex = 0;
            }
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            _controller.StopTCPHandler();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbDeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = lbDeviceList.SelectedIndex;
            tbNickName.Text = curDeviceList[index].NickName;
            tbDeviceName.Text = curDeviceList[index].DeviceName;
            tbIP.Text = curDeviceList[index].IP.ToString();
        }
    }
}
