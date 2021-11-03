
namespace DCBerserkTextureTool
{
    partial class DCBerserkTextureTool
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DCBerserkTextureTool));
            this.buttonUnpack = new System.Windows.Forms.Button();
            this.buttonPack = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label_Working = new System.Windows.Forms.Label();
            this.button_unpackSingle = new System.Windows.Forms.Button();
            this.button_packSingle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonUnpack
            // 
            this.buttonUnpack.Location = new System.Drawing.Point(12, 12);
            this.buttonUnpack.Name = "buttonUnpack";
            this.buttonUnpack.Size = new System.Drawing.Size(272, 52);
            this.buttonUnpack.TabIndex = 0;
            this.buttonUnpack.Text = "Unpack DC Berserk \"TEXTURE/*.TEX\" Folder";
            this.buttonUnpack.UseVisualStyleBackColor = true;
            this.buttonUnpack.Click += new System.EventHandler(this.buttonUnpack_Click);
            // 
            // buttonPack
            // 
            this.buttonPack.Location = new System.Drawing.Point(12, 70);
            this.buttonPack.Name = "buttonPack";
            this.buttonPack.Size = new System.Drawing.Size(272, 52);
            this.buttonPack.TabIndex = 1;
            this.buttonPack.Text = "Repack DC Berserk \"TEXTURE/*.TEX\" Folder";
            this.buttonPack.UseVisualStyleBackColor = true;
            this.buttonPack.Click += new System.EventHandler(this.buttonPack_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "by derplayer - https://derplayer.neocities.org";
            // 
            // label_Working
            // 
            this.label_Working.AutoSize = true;
            this.label_Working.Location = new System.Drawing.Point(12, 125);
            this.label_Working.Name = "label_Working";
            this.label_Working.Size = new System.Drawing.Size(188, 13);
            this.label_Working.TabIndex = 3;
            this.label_Working.Text = "Working... Please wait a few seconds!";
            this.label_Working.Visible = false;
            this.label_Working.Click += new System.EventHandler(this.label2_Click);
            // 
            // button_unpackSingle
            // 
            this.button_unpackSingle.Location = new System.Drawing.Point(290, 12);
            this.button_unpackSingle.Name = "button_unpackSingle";
            this.button_unpackSingle.Size = new System.Drawing.Size(78, 52);
            this.button_unpackSingle.TabIndex = 4;
            this.button_unpackSingle.Text = "Unpack Single *.TEX";
            this.button_unpackSingle.UseVisualStyleBackColor = true;
            this.button_unpackSingle.Click += new System.EventHandler(this.button_unpackSingle_Click);
            // 
            // button_packSingle
            // 
            this.button_packSingle.Location = new System.Drawing.Point(290, 70);
            this.button_packSingle.Name = "button_packSingle";
            this.button_packSingle.Size = new System.Drawing.Size(78, 52);
            this.button_packSingle.TabIndex = 5;
            this.button_packSingle.Text = "Pack Single *.TEX";
            this.button_packSingle.UseVisualStyleBackColor = true;
            this.button_packSingle.Click += new System.EventHandler(this.button_packSingle_Click);
            // 
            // DCBerserkSubtitlesTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 165);
            this.Controls.Add(this.button_packSingle);
            this.Controls.Add(this.button_unpackSingle);
            this.Controls.Add(this.label_Working);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonPack);
            this.Controls.Add(this.buttonUnpack);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DCBerserkSubtitlesTool";
            this.Text = "Sword of the Berserk: TEXTURE Tool v1.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonUnpack;
        private System.Windows.Forms.Button buttonPack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_Working;
        private System.Windows.Forms.Button button_unpackSingle;
        private System.Windows.Forms.Button button_packSingle;
    }
}

