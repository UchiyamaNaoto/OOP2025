namespace RssReader {
    partial class FmNameInput {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            btResist = new Button();
            btCancel = new Button();
            tbNameText = new TextBox();
            label1 = new Label();
            SuspendLayout();
            // 
            // btResist
            // 
            btResist.Location = new Point(132, 92);
            btResist.Name = "btResist";
            btResist.Size = new Size(75, 23);
            btResist.TabIndex = 0;
            btResist.Text = "登録";
            btResist.UseVisualStyleBackColor = true;
            btResist.Click += btResist_Click;
            // 
            // btCancel
            // 
            btCancel.Location = new Point(213, 92);
            btCancel.Name = "btCancel";
            btCancel.Size = new Size(75, 23);
            btCancel.TabIndex = 0;
            btCancel.Text = "キャンセル";
            btCancel.UseVisualStyleBackColor = true;
            btCancel.Click += btCancel_Click;
            // 
            // tbNameText
            // 
            tbNameText.Location = new Point(21, 45);
            tbNameText.Name = "tbNameText";
            tbNameText.Size = new Size(267, 23);
            tbNameText.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 27);
            label1.Name = "label1";
            label1.Size = new Size(31, 15);
            label1.TabIndex = 2;
            label1.Text = "名称";
            // 
            // FmNameInput
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(306, 131);
            Controls.Add(label1);
            Controls.Add(tbNameText);
            Controls.Add(btCancel);
            Controls.Add(btResist);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FmNameInput";
            Text = "FmNameInput";
            Load += FmNameInput_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btResist;
        private Button btCancel;
        private TextBox tbNameText;
        private Label label1;
    }
}