﻿using popper.app.Base;
using popper.app.Models;
using popper.domain.Base;
using popper.domain.Entities;
using popper.Service.Validators;


namespace popper.app.Cadastros
{
    public partial class CadastroChamado : CadastroBase
    {
        private readonly IBaseService<Chamado> _chamadoService;
        private readonly IBaseService<Tecnico> _tecnicoService;
        private readonly IBaseService<Usuario> _usuarioService;

        private List<ChamadoModel>? chamados;
        public CadastroChamado(IBaseService<Chamado> chamadoService, IBaseService<Tecnico> tecnicoService, IBaseService<Usuario> usuarioService)
        {
            _chamadoService = chamadoService;
            _tecnicoService = tecnicoService;
            _usuarioService = usuarioService;
            InitializeComponent();
            CarregarCombo();
        }

        private void CarregarCombo()
        {
            cboNome.ValueMember = "Id";
            cboNome.DisplayMember = "Nome";
            cboNome.DataSource = _usuarioService.Get<Usuario>().ToList();

            cboTecnico.ValueMember = "Id";
            cboTecnico.DisplayMember = "Nome";
            cboTecnico.DataSource = _tecnicoService.Get<Tecnico>().ToList();
        }

        private void PreencheObjeto(Chamado chamado)
        {
            var nome = (Usuario)cboNome.SelectedItem;
            var tecnico = (Tecnico)cboTecnico.SelectedItem;
            chamado.Usuario = nome;
            chamado.Tecnico = tecnico;
            chamado.Desc = txtDesc.Text;
            chamado.Tipo = cboTipo.Text;
        }

        protected override void Salvar()
        {
            try
            {
                if (IsAlteracao)
                {
                    if (int.TryParse(txtId.Text, out var id))
                    {
                        var chamado = _chamadoService.GetById<Chamado>(id);
                        PreencheObjeto(chamado);
                        chamado = _chamadoService.Update<Chamado, Chamado, ChamadoValidator>(chamado);
                    }
                }
                else
                {
                    var chamado = new Chamado();
                    PreencheObjeto(chamado);
                    _chamadoService.Add<Chamado, Chamado, ChamadoValidator>(chamado);
                }

                tabControl1.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Popper", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Deletar(int id)
        {
            try
            {
                _chamadoService.Delete(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, @"Popper", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void CarregaGrid()
        {
            
            chamados = _chamadoService.Get<ChamadoModel>().ToList();

            dataGridView1.DataSource = chamados;
            dataGridView1.Columns["Nome"]!.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["idUsuario"]!.Visible = false;
            dataGridView1.Columns["idTecnico"]!.Visible = false;
            dataGridView1.Columns["idLocal"]!.Visible = false;
            dataGridView1.Columns["idTipoChamado"]!.Visible = false;
        }

        protected override void CarregaRegistro(DataGridViewRow? linha)
        {
            txtId.Text = linha?.Cells["Id"].Value.ToString();
            cboNome.Text = linha?.Cells["Nome do cliente"].Value.ToString();
            cboTecnico.Text = linha?.Cells["Tecnico"].Value.ToString();
            txtDesc.Text = linha?.Cells["Descricao"].Value.ToString();
            cboTipo.Text = linha?.Cells["Tipo do problema"].Value.ToString();
        }
    }
}
