using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desafio09_CRUD
{
    public partial class Form1 : Form
    {
        Customer Model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            Model = new Customer();
            txtCidade.Text = txtEndereco.Text = txtPrimeiroNome.Text = txtSegundoNome.Text = "";
            btnSalvar.Text = "Salvar";
            btnDeletar.Text = "Deletar";
            btnCancelar.Text = "Cancelar";
            btnDeletar.Enabled = false;
            btnDeletar.BackColor = Color.WhiteSmoke;
            btnDeletar.ForeColor = Color.Silver;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDGView();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            Model.FirstName = txtPrimeiroNome.Text.Trim();
            Model.LastName = txtSegundoNome.Text.Trim();
            Model.Addres = txtEndereco.Text.Trim();
            Model.City = txtCidade.Text.Trim();
            bool Update = false;
            using(DBEntities db = new DBEntities())
            {
                if(Model.CustomerID == 0)
                    db.Customers.Add(Model);
                else
                {
                    Update = true;
                    db.Entry(Model).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            Clear();
            PopulateDGView();
            if(Update == true)
                MessageBox.Show("Cliente atualizado!");
            else
                MessageBox.Show("Cliente cadastrado!");
        }

        void PopulateDGView()
        {
            dgvCustomers.AutoGenerateColumns = false;
            using (DBEntities db = new DBEntities())
            {
                dgvCustomers.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dgvCustomers_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomers.CurrentRow.Index != -1)
            {
                Model.CustomerID = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["CustomerID"].Value);
                using (DBEntities db = new DBEntities())
                {
                    Model = db.Customers.Where(C => C.CustomerID == Model.CustomerID).FirstOrDefault();
                    txtPrimeiroNome.Text = Model.FirstName;
                    txtSegundoNome.Text = Model.LastName;
                    txtEndereco.Text = Model.Addres;
                    txtCidade.Text = Model.City;
                }
                btnSalvar.Text = "Atualizar";
                btnDeletar.Enabled = true;
                btnDeletar.BackColor = Color.DarkRed;
                btnDeletar.ForeColor = Color.White;
            }
        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Tem certeza que deseja deletar o cliente selecionado?", "EF CRUD Operation", MessageBoxButtons.YesNo)== DialogResult.Yes)
            {
                using(DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(Model);
                    if (entry.State == EntityState.Detached)
                        db.Customers.Attach(Model);
                    db.Customers.Remove(Model);
                    db.SaveChanges();
                    PopulateDGView();
                    Clear();
                    MessageBox.Show("Cliente deletado!");
                }
            }
        }
    }
}
