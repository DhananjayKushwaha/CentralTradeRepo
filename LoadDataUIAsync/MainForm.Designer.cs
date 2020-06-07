namespace LoadDataUIAsync
{
    partial class MainForm
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
            this.statusBar = new System.Windows.Forms.ProgressBar();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.progressReport = new System.Windows.Forms.Label();
            this.btnCancelAsync = new System.Windows.Forms.Button();
            this.btmLoadAsync = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.controlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBar.Location = new System.Drawing.Point(0, 533);
            this.statusBar.Margin = new System.Windows.Forms.Padding(7);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1234, 51);
            this.statusBar.TabIndex = 0;
            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(259, 0);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(7);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersWidth = 92;
            this.dataGridView.Size = new System.Drawing.Size(975, 533);
            this.dataGridView.TabIndex = 1;
            // 
            // controlPanel
            // 
            this.controlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.controlPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.controlPanel.Controls.Add(this.progressReport);
            this.controlPanel.Controls.Add(this.btnCancelAsync);
            this.controlPanel.Controls.Add(this.btmLoadAsync);
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Margin = new System.Windows.Forms.Padding(7);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(245, 533);
            this.controlPanel.TabIndex = 2;
            // 
            // progressReport
            // 
            this.progressReport.AutoSize = true;
            this.progressReport.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressReport.Location = new System.Drawing.Point(0, 504);
            this.progressReport.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.progressReport.Name = "progressReport";
            this.progressReport.Size = new System.Drawing.Size(178, 29);
            this.progressReport.TabIndex = 2;
            this.progressReport.Text = "Loading... 52 %";
            this.progressReport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancelAsync
            // 
            this.btnCancelAsync.Location = new System.Drawing.Point(21, 80);
            this.btnCancelAsync.Margin = new System.Windows.Forms.Padding(7);
            this.btnCancelAsync.Name = "btnCancelAsync";
            this.btnCancelAsync.Size = new System.Drawing.Size(205, 51);
            this.btnCancelAsync.TabIndex = 1;
            this.btnCancelAsync.Text = "Cancel Async";
            this.btnCancelAsync.UseVisualStyleBackColor = true;
            this.btnCancelAsync.Click += new System.EventHandler(this.btnCancelAsync_Click);
            // 
            // btmLoadAsync
            // 
            this.btmLoadAsync.Location = new System.Drawing.Point(21, 16);
            this.btmLoadAsync.Margin = new System.Windows.Forms.Padding(7);
            this.btmLoadAsync.Name = "btmLoadAsync";
            this.btmLoadAsync.Size = new System.Drawing.Size(205, 51);
            this.btmLoadAsync.TabIndex = 0;
            this.btmLoadAsync.Text = "Load Async";
            this.btmLoadAsync.UseVisualStyleBackColor = true;
            this.btmLoadAsync.Click += new System.EventHandler(this.btmLoadAsync_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 584);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.statusBar);
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Async Data Load Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar statusBar;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button btmLoadAsync;
        private System.Windows.Forms.Button btnCancelAsync;
        private System.Windows.Forms.Label progressReport;

    }
}

