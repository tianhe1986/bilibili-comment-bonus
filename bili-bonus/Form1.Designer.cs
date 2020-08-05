namespace bili_bonus
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dynamicId = new System.Windows.Forms.TextBox();
            this.bonusNum = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.outputTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // dynamicId
            // 
            this.dynamicId.Location = new System.Drawing.Point(109, 37);
            this.dynamicId.Name = "dynamicId";
            this.dynamicId.Size = new System.Drawing.Size(338, 21);
            this.dynamicId.TabIndex = 0;
            this.dynamicId.Text = "请输入动态ID";
            // 
            // bonusNum
            // 
            this.bonusNum.Location = new System.Drawing.Point(109, 80);
            this.bonusNum.Name = "bonusNum";
            this.bonusNum.Size = new System.Drawing.Size(338, 21);
            this.bonusNum.TabIndex = 1;
            this.bonusNum.Text = "请输入中奖人数";
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(487, 37);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "开始抽奖";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // outputTxt
            // 
            this.outputTxt.Location = new System.Drawing.Point(75, 146);
            this.outputTxt.Multiline = true;
            this.outputTxt.Name = "outputTxt";
            this.outputTxt.ReadOnly = true;
            this.outputTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTxt.Size = new System.Drawing.Size(593, 422);
            this.outputTxt.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 591);
            this.Controls.Add(this.outputTxt);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.bonusNum);
            this.Controls.Add(this.dynamicId);
            this.Name = "Form1";
            this.Text = "三国杀-b站评论抽奖";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox dynamicId;
        private System.Windows.Forms.TextBox bonusNum;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox outputTxt;
    }
}

