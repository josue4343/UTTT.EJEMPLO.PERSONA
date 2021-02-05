#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTTT.Ejemplo.Linq.Data.Entity;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Collections;
using UTTT.Ejemplo.Persona.Control;
using UTTT.Ejemplo.Persona.Control.Ctrl;
using System.Net.Configuration;
using System.Configuration;
using System.Net.Mail;
using System.Net;

#endregion

namespace UTTT.Ejemplo.Persona
{
    public partial class PersonaManager : System.Web.UI.Page
    {
        #region Variables

        private SessionManager session = new SessionManager();
        private int idPersona = 0;
        private UTTT.Ejemplo.Linq.Data.Entity.Persona baseEntity;
        private DataContext dcGlobal = new DcGeneralDataContext();
        private int tipoAccion = 0;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Response.Buffer = true;
                this.session = (SessionManager)this.Session["SessionManager"];
                this.idPersona = this.session.Parametros["idPersona"] != null ?
                    int.Parse(this.session.Parametros["idPersona"].ToString()) : 0;
                if (this.idPersona == 0)
                {
                    this.baseEntity = new Linq.Data.Entity.Persona();
                    this.tipoAccion = 1;
                }
                else
                {
                    this.baseEntity = dcGlobal.GetTable<Linq.Data.Entity.Persona>().First(c => c.id == this.idPersona);
                    this.tipoAccion = 2;
                }

                if (!this.IsPostBack)
                {
                    if (this.session.Parametros["baseEntity"] == null)
                    {
                        this.session.Parametros.Add("baseEntity", this.baseEntity);
                    }
                    List<CatSexo> lista = dcGlobal.GetTable<CatSexo>().ToList();
                    CatSexo catTemp = new CatSexo();
                    catTemp.id = -1;
                    catTemp.strValor = "Seleccionar";
                    lista.Insert(0, catTemp);
                    this.ddlSexo.DataTextField = "strValor";
                    this.ddlSexo.DataValueField = "id";
                    this.ddlSexo.DataSource = lista;
                    this.ddlSexo.DataBind();

                    this.ddlSexo.SelectedIndexChanged += new EventHandler(ddlSexo_SelectedIndexChanged);
                    this.ddlSexo.AutoPostBack = true;
                    if (this.idPersona == 0)
                    {
                        this.lblAccion.Text = "Agregar";
                    }
                    else
                    {
                        this.lblAccion.Text = "Editar";
                        this.txtNombre.Text = this.baseEntity.strNombre;
                        this.txtAPaterno.Text = this.baseEntity.strAPaterno;
                        this.txtAMaterno.Text = this.baseEntity.strAMaterno;
                        this.txtClaveUnica.Text = this.baseEntity.strClaveUnica;
                        this.txtCorreo.Text = this.baseEntity.correoE;
                        this.txtCp.Text = this.baseEntity.Cp;
                        this.txtRfc.Text = this.baseEntity.Rfc;

                        this.setItem(ref this.ddlSexo, baseEntity.CatSexo.strValor);
                        ddlSexo.Items.FindByValue("-1").Enabled = false;
                        int valor = baseEntity.CatSexo.id;
                        if (valor == 0)
                        {
                            ddlSexo.Items.FindByValue("2").Enabled = true;


                        }
                        else
                        {
                            ddlSexo.Items.FindByValue("1").Enabled = true;
                        }
                        DateTime? fecha = this.baseEntity.calendario;
                        this.fecha.Value = fecha.ToString();
                        



                    }                 
                }

            }
            catch (Exception _e)
            {
                this.showMessage("Ha ocurrido un problema al cargar la página");
                this.Response.Redirect("~/PersonaPrincipal.aspx", false);
            }

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
               
                DataContext dcGuardar = new DcGeneralDataContext();
                UTTT.Ejemplo.Linq.Data.Entity.Persona persona = new Linq.Data.Entity.Persona();
                if (this.idPersona == 0)
                {
                    persona.strClaveUnica = this.txtClaveUnica.Text.Trim();
                    persona.strNombre = this.txtNombre.Text.Trim();
                    persona.strAMaterno = this.txtAMaterno.Text.Trim();
                    persona.strAPaterno = this.txtAPaterno.Text.Trim();
                    persona.idCatSexo = int.Parse(this.ddlSexo.Text);

                     DateTime calendari = this.Calendar1.SelectedDate.Date;
                    
                    persona.calendario = calendari;

                    persona.correoE = this.txtCorreo.Text.Trim();
                    persona.Cp = this.txtCp.Text.Trim();
                    persona.Rfc = this.txtRfc.Text.Trim();


                    String mensaje = String.Empty;
                    //Validacion de datos correctos


                    if (!this.validacion(persona, ref mensaje))
                    {
                        this.Label2.Text = mensaje;
                        this.Label2.Visible = true;
                        return;
                    }


                    if (!this.validaSql(ref mensaje))
                    {
                        this.Label2.Text = mensaje;
                        this.Label2.Visible = true;
                        return;
                    }

                    if (!this.validaHTML(ref mensaje))
                    {
                        this.Label2.Text = mensaje;
                        this.Label2.Visible = true;
                        return;
                    }

                    dcGuardar.GetTable<UTTT.Ejemplo.Linq.Data.Entity.Persona>().InsertOnSubmit(persona);
                    dcGuardar.SubmitChanges();
                    this.showMessage("El registro se agrego correctamente.");
                    this.Response.Redirect("~/PersonaPrincipal.aspx", false);
                    
                }
                if (this.idPersona > 0)
                {
                    persona = dcGuardar.GetTable<UTTT.Ejemplo.Linq.Data.Entity.Persona>().First(c => c.id == idPersona);
                    persona.strClaveUnica = this.txtClaveUnica.Text.Trim();
                    persona.strNombre = this.txtNombre.Text.Trim();
                    persona.strAMaterno = this.txtAMaterno.Text.Trim();
                    persona.strAPaterno = this.txtAPaterno.Text.Trim();
                    persona.idCatSexo = int.Parse(this.ddlSexo.Text);

                   DateTime calendari = this.Calendar1.SelectedDate.Date;
                    persona.calendario = calendari;

                    persona.correoE = this.txtCorreo.Text.Trim();
                    persona.Cp = this.txtCp.Text.Trim();
                    persona.Rfc = this.txtRfc.Text.Trim();

                    dcGuardar.SubmitChanges();
                    this.showMessage("El registro se edito correctamente.");
                    this.Response.Redirect("~/PersonaPrincipal.aspx", false);
                }
            }
            catch (Exception _e)
            {
                //this.showMessageException(_e.Message);
                var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                string strHost = smtpSection.Network.Host;
                int port = smtpSection.Network.Port;
                string strUserName = smtpSection.Network.UserName;
                string strFromPass = smtpSection.Network.Password;
                SmtpClient smtp = new SmtpClient(strHost, port);
                MailMessage msg = new MailMessage();
                string body = "<h1>El Error Es: " + _e.Message + "</h1>";
                msg.From = new MailAddress(smtpSection.From, "CORREO");
                msg.To.Add(new MailAddress("18300225@uttt.edu.mx"));
                msg.Subject = "Se ha un  error"; ;
                msg.IsBodyHtml = true;
                msg.Body = body;
                smtp.Credentials = new NetworkCredential(strUserName, strFromPass);
                smtp.EnableSsl = true;
                smtp.Send(msg);
                Response.Redirect("~/Error.aspx", false);

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {              
                this.Response.Redirect("~/PersonaPrincipal.aspx", false);
            }
            catch (Exception _e)
            {
                this.showMessage("Ha ocurrido un error inesperado");
            }
        }

        protected void ddlSexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idSexo = int.Parse(this.ddlSexo.Text);
                Expression<Func<CatSexo, bool>> predicateSexo = c => c.id == idSexo;
                predicateSexo.Compile();
                List<CatSexo> lista = dcGlobal.GetTable<CatSexo>().Where(predicateSexo).ToList();
                CatSexo catTemp = new CatSexo();            
                this.ddlSexo.DataTextField = "strValor";
                this.ddlSexo.DataValueField = "id";
                this.ddlSexo.DataSource = lista;
               
            }
            catch (Exception)
            {
                this.showMessage("Ha ocurrido un error inesperado");
            }
        }

        #endregion

        #region Metodos

        public void setItem(ref DropDownList _control, String _value)
        {
            foreach (ListItem item in _control.Items)
            {
                if (item.Value == _value)
                {
                    item.Selected = true;
                    break;
                }
            }
            _control.Items.FindByText(_value).Selected = true;
        }

        #endregion

     

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            fecha.Value = Calendar1.SelectedDate.ToString();
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public bool validacion(UTTT.Ejemplo.Linq.Data.Entity.Persona people, ref String ms)
        {
            if (people.idCatSexo == -1)
            {
                ms = "Debe de seleccionar algun campo especifico, ya sea hombre o mujer ";
                return false;
            }
            int j = 0;
            if (int.TryParse(people.strClaveUnica, out j) == false)
            {
                ms = "La clave unica solo permite numeros ";
                return false;
            }  if (people.strClaveUnica.Equals(String.Empty))
            {
                ms = "Clave Unica no debe de estar vacia ";
                return false;
            }
            if (int.Parse(people.strClaveUnica) < 000 || int.Parse(people.strClaveUnica) > 999)
            {
                ms = "La clave Unica debe de constar exactamente con 3 numeros";
                return false;
            }  if (people.strNombre.Equals(String.Empty))
            {
                ms = "El nombre no debe de esta vacio";
                return false;
            }  if (people.strNombre.Length > 50)
            {
                ms = "El nombre no debe de rebasar mas 50 caracteres";
                return false;
            } if (people.strAPaterno.Equals(String.Empty))
            {
                ms = "El Apellido Paterno no debe de estar vacio";
                return false;
            }  if (people.strAPaterno.Length > 50)
            {
                ms = "Los caracteres permitidos para Apellido Paterno rebasan lo establecido";
                return false;
            }
            if (people.strAMaterno.Equals(String.Empty))
            {
                ms = "Apellido Materno esta vacio";
                return false;
            }
            if (people.strAPaterno.Length > 50)
            {
                ms = "Los caracteres permitidos para Apellido Materno rebasan lo establecido";
                return false;
            }
            if (people.correoE.Equals(String.Empty))
            {
                ms = " El Correo Electronico se encuentra vacio esta vacio";
                return false;
            }
            if (people.correoE.Length > 50)
            {
                ms = "Los caracteres permitidos para Correo Electronico rebasan lo establecido";
                return false;
            }
            //validacion de la rfc 
            if (people.Rfc.Equals(String.Empty))
            {
                ms = "RFC esta vacio";
                return false;
            }if (people.Rfc.Length > 50)
            {
                ms = "Los caracteres permitidos para RFC rebasan lo establecido";
                return false;
            } if (int.TryParse(people.Cp, out j) == false)
            {
                ms = "El codigo postal no es numero";
                return false;
            }
            if (people.Cp.Equals(String.Empty)) {
                ms = "Codigo Postal esta vacio";
                return false;
            } if (int.Parse(people.Cp) < 0000 || int.Parse(people.Cp) > 99999)
            {
                ms = "El codigo postal solo consta de 5 dijitos numericos";
                return false;
            }

            // Validacion Calendario
            DateTime? fecha = this.baseEntity.calendario;
            this.fecha.Value = fecha.ToString();
            TimeSpan timeSpan = DateTime.Now - fecha.Value.Date;
            if (timeSpan.Days < 6570)
            {
                ms = "La persona es menor de edad";
                return false;
            }
            return true;
        }


        private bool validaSql(ref String ms)
        {
            controladora validacion = new controladora();
            string mensajeF = string.Empty;
            if (validacion.sqlInyectionValida(this.txtNombre.Text.Trim(), ref mensajeF, "Nombre", ref this.txtNombre))
            {
                ms = mensajeF;
                return false;
            }
            if (validacion.sqlInyectionValida(this.txtAPaterno.Text.Trim(), ref mensajeF, "A Paterno", ref this.txtAPaterno))
            {
                ms = mensajeF;
                return false;
            }
            if (validacion.sqlInyectionValida(this.txtAMaterno.Text.Trim(), ref mensajeF, "A Materno", ref this.txtAMaterno))
            {
                ms = mensajeF;
                return false;
            }
           
            return true;
        }
        private bool validaHTML(ref String ms)
        {
            controladora validacion = new controladora();
            string mensajeF = string.Empty;
            if (validacion.htmlInyectionValida(this.txtNombre.Text.Trim(), ref mensajeF, "Nombre", ref this.txtNombre))
            {
                ms = mensajeF;
                return false;
            }
            if (validacion.htmlInyectionValida(this.txtAPaterno.Text.Trim(), ref mensajeF, "A Paterno", ref this.txtAPaterno))
            {
                ms = mensajeF;
                return false;
            }
            if (validacion.htmlInyectionValida(this.txtAMaterno.Text.Trim(), ref mensajeF, "A Materno", ref this.txtAMaterno))
            {
                ms = mensajeF;
                return false;
            }
            if (validacion.htmlInyectionValida(this.txtCorreo.Text.Trim(), ref mensajeF, "Correo Electronico", ref this.txtCorreo))
            {
                ms = mensajeF;
                return false;
            }
            if (validacion.htmlInyectionValida(this.txtRfc.Text.Trim(), ref mensajeF, "RFC", ref this.txtRfc))
            {
                ms = mensajeF;
                return false;
            }
            if (validacion.htmlInyectionValida(this.txtClaveUnica.Text.Trim(), ref mensajeF, "Clave Unica", ref this.txtClaveUnica))
            {
                ms = mensajeF;
                return false;
            }
            if (validacion.htmlInyectionValida(this.txtCp.Text.Trim(), ref mensajeF, "Codigo Postal", ref this.txtCp))
            {
                ms = mensajeF;
                return false;
            }


            return true;
        }

    }
}