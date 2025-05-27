using System;
using System.Drawing;
using System.Windows.Forms;

public class AdaugareMasina : Form
{
    public AdaugareMasina()
    {
        this.Text = "Adăugare Mașină";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Size = new Size(800, 600);
        this.Font = new Font("Segoe UI", 10F);
        this.BackColor = ColorTranslator.FromHtml("#e3f2fd");
    }
} 