﻿namespace EliteMMO.Scripted.Views
{
    using System.Windows.Forms;
    using System.Diagnostics;
    using System.Linq;
    using System.Drawing;
    using API;
    using System.IO;
    using System.Net;
    using System;

    public partial class MainWindow : Form
    {
        public MainWindow(EliteAPI core)
        {
            InitializeComponent();
            api = core;

            #region Final Fantasy XI [POL]
            var data = Process.GetProcessesByName("pol");

            if (data.Count() != 0)
            {
                var proc = Process.GetProcessesByName("pol").First().Id;
                PID = proc;
                api = new EliteAPI(proc);

                foreach (var dats in data)
                {
                    EliteMMO_PROC?.Items.Add(dats.MainWindowTitle);
                }

                if (EliteMMO_PROC != null)
                    EliteMMO_PROC.SelectedIndex = 0;

                xStatusLabel.Text = @":: " + api.Entity.GetLocalPlayer().Name + @" ::";
            }
            else
            {
                xStatusLabel.Text = @":: Final Fantasy Not Found ::";
            }
            #endregion

            x1 = new ScriptFarmDNC(api);
            x2 = new ScriptHealing(api);
            x3 = new ScriptNaviMap(api);
            x4 = new ScriptOnEventTool(api);

            string apidll = FileVersionInfo.GetVersionInfo(Application.StartupPath + @"\EliteAPI.dll").FileVersion;
            string apidllnew = GetStringFromUrl("http://ext.elitemmonetwork.com/downloads/eliteapi/index.php?v");
            string mmodll = FileVersionInfo.GetVersionInfo(Application.StartupPath + @"\EliteMMO.API.dll").FileVersion;
            string mmodllnew = GetStringFromUrl("http://ext.elitemmonetwork.com/downloads/elitemmo_api/index.php?v");
            string appexe = FileVersionInfo.GetVersionInfo(Application.StartupPath + @"\Scripted.exe").FileVersion;
            string appexenew = GetStringFromUrl("https://raw.githubusercontent.com/smd111/EliteMMO.Scripted/master/EliteMMO.Scripted/ScriptedVer.txt").Replace("\n", "");
            string message = "";
            if (apidll.CompareTo(apidllnew) > 0) message = message + "\nEliteAPI.dll";
            if (mmodll.CompareTo(mmodllnew) > 0) message = message + "\nEliteMMO.API.dll";
            if (appexe.CompareTo(appexenew) > 0) message = message + "\nScripted";
            DialogResult result;
            if (message != "") result = MessageBox.Show("Update Files:" + message, "Update Files", MessageBoxButtons.YesNo);
            else result = DialogResult.No;
            if (result == DialogResult.Yes)
            {
                Process.Start(Application.StartupPath + @"\Updater.exe");
                Environment.Exit(0);
            }
        }
        private string GetStringFromUrl(string location)
        {
            System.Net.WebRequest request = WebRequest.Create(location);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            return responseFromServer;
        }


        private void RefreshCharactersToolStripMenuItemClick(object sender, System.EventArgs e)
        {
            var data = Process.GetProcessesByName("pol");
            var node = EliteMMO_PROC.SelectedIndex;

            if (data.Count() <= EliteMMO_PROC.Items.Count && data.Count() >= EliteMMO_PROC.Items.Count) return;

            var count = data.Count();
            if (count != 0)
            {
                //EliteMMO_PROC.Items.Clear();
                foreach (var dats in data.Where(dats => EliteMMO_PROC.Contains(new Control(dats.MainWindowTitle)) == false))
                {
                    EliteMMO_PROC.Items.Add(dats.MainWindowTitle);
                }
                EliteMMO_PROC.SelectedIndex = node;
            }
            else
            {
                if (EliteMMO_PROC.Items.Count != 0)
                    EliteMMO_PROC.Items.Clear();
            }
        }

        private void EliteMmoProcSelectedIndexChanged(object sender, System.EventArgs e)
        {
            var data = Process.GetProcessesByName("pol");

            if (!data.Any()) return;

            foreach (var dats in data.Where(dats => EliteMMO_PROC.Text == dats.MainWindowTitle))
            {
                PID = dats.Id;
                api.Reinitialize(dats.Id);
                xStatusLabel.Text = @":: " + api.Entity.GetLocalPlayer().Name + @" ::";
            }
        }

        private void FarmDncToolStripMenuItemClick(object sender, System.EventArgs e)
        {
            if (xStatusLabel.Text == @":: Final Fantasy Not Found ::") return;

            //if (InventoryItems.items.Count == 0)
            //    InventoryItems.PopulateItems();

            #region show/hide objects
            xpic.Hide();
            header1.Hide();
            header2.Hide();
            header3.Hide();
            header4.Hide();
            header5.Hide();
            label1.Hide();
            //button1.Hide();
            EliteMMO_PROC.Hide();

            x4.Hide();
            x2.Hide();
            x1.Show();
            #endregion

            refreshCharactersToolStripMenuItem.Enabled = false;

            x1.Dock = DockStyle.Fill;
            Controls.Add(x1);
            Size = new Size(734, 468);
        }

        private void HealingSupportToolStripMenuItemClick(object sender, System.EventArgs e)
        {
            if (xStatusLabel.Text == @":: Final Fantasy Not Found ::") return;

            #region show/hide objects
            xpic.Hide();
            header1.Hide();
            header2.Hide();
            header3.Hide();
            header4.Hide();
            header5.Hide();
            label1.Hide();
            //button1.Hide();
            EliteMMO_PROC.Hide();

            x4.Hide();
            x1.Hide();
            x2.Show();
            #endregion

            refreshCharactersToolStripMenuItem.Enabled = false;

            x1.Dock = DockStyle.Fill;
            Controls.Add(x2);
            Size = new Size(650, 442);
        }

        private void AboutToolStripMenuItemClick(object sender, System.EventArgs e)
        {
            #region show/hide objects
            xpic.Show();
            header1.Show();
            header2.Show();
            header3.Show();
            header4.Show();
            header5.Show();
            label1.Show();
            //button1.Show();
            EliteMMO_PROC.Show();

            x4.Hide();
            x1.Hide();
            #endregion

            refreshCharactersToolStripMenuItem.Enabled = true;

            Size = new Size(372, 237);
        }

        private void CloseExitToolStripMenuItemClick(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void OnEventToolStripMenuItemClick(object sender, System.EventArgs e)
        {
            //if (xStatusLabel.Text == @":: Final Fantasy Not Found ::") return;

            #region show/hide objects
            xpic.Hide();
            header1.Hide();
            header2.Hide();
            header3.Hide();
            header4.Hide();
            header5.Hide();
            label1.Hide();
            //button1.Hide();
            EliteMMO_PROC.Hide();

            x2.Hide();
            x1.Hide();
            x4.Show();
            #endregion

            refreshCharactersToolStripMenuItem.Enabled = false;

            x4.Dock = DockStyle.Fill;
            Controls.Add(x4);
            Size = new Size(482, 488);
        }
    }
}
