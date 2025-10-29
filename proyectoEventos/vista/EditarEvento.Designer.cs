namespace proyectoEventos.vista
{
    partial class EditarEvento
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtnombre = new System.Windows.Forms.TextBox();
            this.txtfechaevento = new System.Windows.Forms.Label();
            this.txtevento = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtlugar = new System.Windows.Forms.TextBox();
            this.l = new System.Windows.Forms.Label();
            this.txtdescripcion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtentradas = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(33, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nombre";
            // 
            // txtnombre
            // 
            this.txtnombre.Location = new System.Drawing.Point(36, 68);
            this.txtnombre.Name = "txtnombre";
            this.txtnombre.Size = new System.Drawing.Size(172, 22);
            this.txtnombre.TabIndex = 1;
            // 
            // txtfechaevento
            // 
            this.txtfechaevento.AutoSize = true;
            this.txtfechaevento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtfechaevento.Location = new System.Drawing.Point(33, 112);
            this.txtfechaevento.Name = "txtfechaevento";
            this.txtfechaevento.Size = new System.Drawing.Size(159, 22);
            this.txtfechaevento.TabIndex = 2;
            this.txtfechaevento.Text = "Fecha de evento";
            // 
            // txtevento
            // 
            this.txtevento.Location = new System.Drawing.Point(33, 149);
            this.txtevento.Name = "txtevento";
            this.txtevento.Size = new System.Drawing.Size(175, 22);
            this.txtevento.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "Lugar de Evento";
            // 
            // txtlugar
            // 
            this.txtlugar.Location = new System.Drawing.Point(33, 247);
            this.txtlugar.Name = "txtlugar";
            this.txtlugar.Size = new System.Drawing.Size(172, 22);
            this.txtlugar.TabIndex = 5;
            // 
            // l
            // 
            this.l.AutoSize = true;
            this.l.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l.Location = new System.Drawing.Point(30, 300);
            this.l.Name = "l";
            this.l.Size = new System.Drawing.Size(211, 22);
            this.l.TabIndex = 6;
            this.l.Text = "Descripcion de Evento";
            // 
            // txtdescripcion
            // 
            this.txtdescripcion.Location = new System.Drawing.Point(33, 338);
            this.txtdescripcion.Name = "txtdescripcion";
            this.txtdescripcion.Size = new System.Drawing.Size(172, 22);
            this.txtdescripcion.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(29, 381);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 22);
            this.label3.TabIndex = 8;
            this.label3.Text = "Entradas totales";
            // 
            // txtentradas
            // 
            this.txtentradas.Location = new System.Drawing.Point(36, 416);
            this.txtentradas.Name = "txtentradas";
            this.txtentradas.Size = new System.Drawing.Size(172, 22);
            this.txtentradas.TabIndex = 9;
            // 
            // EditarEvento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::proyectoEventos.Properties.Resources._1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtentradas);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtdescripcion);
            this.Controls.Add(this.l);
            this.Controls.Add(this.txtlugar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtevento);
            this.Controls.Add(this.txtfechaevento);
            this.Controls.Add(this.txtnombre);
            this.Controls.Add(this.label1);
            this.Name = "EditarEvento";
            this.Text = "EditarEvento";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtnombre;
        private System.Windows.Forms.Label txtfechaevento;
        private System.Windows.Forms.TextBox txtevento;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtlugar;
        private System.Windows.Forms.Label l;
        private System.Windows.Forms.TextBox txtdescripcion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtentradas;
    }
}