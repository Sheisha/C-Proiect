﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace ProiectFinal
{
    public partial class Specializari : Form
    {
        public Specializari()
        {
            InitializeComponent();
        }

        private void Specializari_Load(object sender, EventArgs e)
        {
            config(true);
            refresh();
            dataGridView1.ReadOnly = true;

        }

        private void config(bool v)
        {
            dataGridView1.AllowUserToAddRows = !v;
            dataGridView1.AllowUserToDeleteRows = !v;
            dataGridView1.ReadOnly = v;

            btnActualizare.Enabled = v;
            btnSalvare.Visible = !v;
            btnRenuntare.Visible = !v;
        }


        private void refresh()
        {
            specializariTableAdapter.Fill(this.dataSet1.Specializari);
        }
        private bool completareCampuri()
        {
            bool raspuns = true;
            foreach (DataRow r in this.dataSet1.Specializari)
            {
                if (r.RowState == DataRowState.Deleted) continue;

                if (r["DenumireSpecializare"] == DBNull.Value)
                {
                    MessageBox.Show("Completati DenumireSpecializare la linia cu Id " + r["IdSpecializare"]);
                    raspuns = false;
                }
                if (r["NrAniStudiu"] == DBNull.Value)
                {
                    MessageBox.Show("Completati NrAniStudiu la linia cu Id " + r["IdSpecializare"]);
                    raspuns = false;
                }
                if (r["TaxaAnuala"] == DBNull.Value)
                {
                    MessageBox.Show("Completati TaxaAnuala la linia cu Id " + r["IdSpecializare"]);
                    raspuns = false;
                }
                
            }
            return raspuns;
        }

        private void btnActualizare_Click(object sender, EventArgs e) {
            //A2
            config(false);
        }

        private void btnSalvare_Click(object sender, EventArgs e) {
            if (!completareCampuri()) return;
            try {
                specializariTableAdapter.Update(this.dataSet1.Specializari);
                config(true);
                refresh();
            }
            catch (Exception exc) {
                string s = exc.Message;
               
                if (s.IndexOf("duplicate values") > 0)
                    MessageBox.Show("Inregistrare deja existenta !");
                else if (s.IndexOf("cannot be deleted") > 0)
                    MessageBox.Show("Ati sters inregistrari referite in alte tabele !");
            }
        }

        private void btnRenuntare_Click(object sender, EventArgs e) {
            //A3
            config(true);
            refresh();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            if (btnRenuntare.Focused) {
                dataGridView1.CancelEdit();
                //A3
                config(true);
                refresh();

                return;
            }
            MessageBox.Show("Id-ul exista deja in tabel.");
        }


        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) {
            const string mesaj = "Confirmati stergerea";
            const string titlu = "Stergere inregistrare";
            var rezultat = MessageBox.Show(mesaj, titlu, MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Warning);
            if (rezultat == DialogResult.No) e.Cancel = true;


            
        }



    }
}
