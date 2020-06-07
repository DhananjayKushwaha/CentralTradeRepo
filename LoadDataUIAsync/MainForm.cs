using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;

namespace LoadDataUIAsync
{
    public partial class MainForm : Form
    {
        //temporarily stores the piece of data and shared on main thread and background thread
        DataTable ChunkData = null;

        //Datasource to DataGridView
        DataTable Data = null;

        //BackgroundWorker instance to load data on background thread
        BackgroundWorker bgWorker = new BackgroundWorker();

        //Total rows to load in grid, you can define according to your needs
        long TotalRows2Load = 500000;

        //To notifies a waiting data load thread that datagridview has consumed current chunk data
        AutoResetEvent autoResetEvent = new AutoResetEvent(false);

        public MainForm()
        {
            InitializeComponent();

            //setting up background worker to load data on thread
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //holds loading rows count
            int iLoadedRowsCount = 0;
            do
            {
                //check if cancel is requested
                if (bgWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                //load chunk data
                ChunkData = ProduceData();
                iLoadedRowsCount += ChunkData.Rows.Count;
                                
                bgWorker.ReportProgress((int)(((float)iLoadedRowsCount / TotalRows2Load) * 100));
                
                //wait for datagrid view to load chunk data completely
                autoResetEvent.WaitOne();
            }
            while( iLoadedRowsCount < TotalRows2Load );

            e.Result = 100;
        }

        void AddChunkData()
        {
            Data.BeginLoadData();

            foreach (DataRow row in ChunkData.Rows)
            {
                DataRow dataRow = Data.Rows.Add();
                dataRow["Name"] = row["Name"];
            }

            Data.EndLoadData();
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //add the chunk data to data grid view datasource
            AddChunkData();

            progressReport.Text = "Loading... " + e.ProgressPercentage + "%";
            statusBar.Value = e.ProgressPercentage;

            //notify background thread to continue to load next data chunk
            autoResetEvent.Set();            
        }

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //handle background thread error if occured
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                progressReport.Text = "action cancelled.";
            }
            else if( e.Result != null )
            {
                progressReport.Text = "Loading... " + (int)e.Result + "%";
            }
        }

        private void btmLoadAsync_Click(object sender, EventArgs e)
        {
            //if user hit the data load again then cancel the current background thread if busy
            CancelLoad();

            //clear the rows to load fresh data
            Data.Rows.Clear();

            //start asynchronous data load 
            if (!bgWorker.IsBusy)
            {
                bgWorker.RunWorkerAsync();
            }           
        }

        void CancelLoad()
        {
            //cancel asynchronous load 
            if (bgWorker.IsBusy)
            {
                bgWorker.CancelAsync();
            }
        }

        private void btnCancelAsync_Click(object sender, EventArgs e)
        {
            CancelLoad();
        }

        //Method to produce dummy data to simulate async large data load. This can be relaced according to business requirement like loading data from file
        DataTable ProduceData()
        {
            ConcurrentBag<DataTable> logTables = new ConcurrentBag<DataTable>();

            string logDir = Path.Combine(Environment.CurrentDirectory, @"Logs");
            var logFiles = Directory.GetFiles(logDir);

            Parallel.ForEach(logFiles, (file) => {
                //read file here  & build table
                
                //fill the data in data table using guid for demo
                DataTable table = CreateDataTable();
                for (int i = 0; i < 500; i++)
                {
                    DataRow row = table.Rows.Add();
                    row["Name"] = Guid.NewGuid().ToString();
                }

                logTables.Add(table);
            });

            DataTable allLogs = CreateDataTable();

            foreach (var logTable in logTables)
            {
                allLogs.Merge(logTable);
                allLogs.AcceptChanges();
            }
            
            return allLogs;
        }

        //Method to create dummy table to hold data
        DataTable CreateDataTable()
        {
            DataColumn clmID = new DataColumn( "ID", typeof(long));
            clmID.AutoIncrement = true;
            DataColumn clmName = new DataColumn("Name", typeof(string));
            DataTable table = new DataTable();
            table.Columns.Add(clmID);
            table.Columns.Add(clmName);

            return table;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //create a dummy table and set as datasource to data grid view 
            Data = CreateDataTable();
            dataGridView.DataSource = Data;            
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //cancel the background thread (if busy) if form is closing.
            CancelLoad();
        }
    }
}
