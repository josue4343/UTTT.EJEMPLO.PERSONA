using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UTTT.Ejemplo.Persona.Control
{
    public class controladora
    {
        public bool htmlInyectionValida(string inf, ref string ms, string _etiquetaReferente,
            ref System.Web.UI.WebControls.TextBox _control)
        {
            Regex tagRegex = new Regex(@"<.*?>|&.*?;");
            bool Resultado = tagRegex.IsMatch(inf);
            if (Resultado)
            {
                ms = "Caracteres no permitidos en " + _etiquetaReferente.Replace(":", "");
                _control.Focus();
            }
            return Resultado;
        }

        public bool sqlInyectionValida(string info, ref string ms, string etc,
            ref System.Web.UI.WebControls.TextBox ct)
        {
            Regex tagRegex = new Regex(@"('(''|[^'])*')|(\b(ALTER|alter|Alter|CREATE|
                create|Create|DELETE|delete|Delete|DROP|drop|Drop|EXEC(UTE){0,1}|exec(ute){0,1}|
                Exec(ute){0,1}|INSERT( +INTO){0,1}|insert( +into){0,1}|Insert( +into){0,1}|MERGE|
                merge|Merge|SELECT|Select|select|UPDATE|update|Update|UNION( +ALL){0,1}|
                union( +all){0,1}|Union( +all){0,1})\b)");
            bool respuesta = tagRegex.IsMatch(info);
            if (respuesta)
            {
                ms = "Sintaxis no permetida en " + etc.Replace(":", "");
                ct.Focus();
            }
            return respuesta;
        }




    }
}
