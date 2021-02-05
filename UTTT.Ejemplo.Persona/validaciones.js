function validar() {
    debugger;
    var sexo = document.getElementById("ddlSexo");
    var clave = document.getElementById("txtClaveUnica").value;
    var nombre = document.getElementById("txtNombre").value;
    var apellidop = document.getElementById("txtAPaterno").value;
    var apellidom = document.getElementById("txtAMaterno").value;
    var fecha = document.getElementById("fecha");

    var rfc = document.getElementById("txtRfc");
    var cp = document.getElementById("txtCp")

    var Calendario = fecha.defaultValue;
    var fechapar = parseInt((""+ Calendario.substr(6, 10)));
    var ddlSexo = sexo.options[sexo.selectedIndex].value;
    var resF = 2021 - fechapar;

    var camp = true;

    var cod ="";

    if (clave == null || nombre == null || apellidop == null || apellidom == null || fecha == null || resF==null ) {

        cod = "El campo  esta vacio";
        camp = false;

    } else if (ddlSexo < 0 || ddlSexo > 2) {
        cod = "elige el sexo";
        camp = false;

    } else if (!(/^\d{3}$/.test(clave))) {
        cod = "ingresa mas de 3 dijitos ";
        camp = false;

    } else if (!(/^([aA-záéíóúZÁÉÍÓÚ]{3}[a-zñáéíóú]+[\s]*)+$/.test(nombre))) {
        cod = "el campo debe de acompletar por lo menos 3 caracteres ";
        camp = false;
    } else if (!(/^([aA-záéíóúZÁÉÍÓÚ]{3}[a-zñáéíóú]+[\s]*)+$/.test(apellidop))) {
        cod = "el campo debe de acompletar por lo menos 3 caracteres";
        camp = false;

    } else if (!(/^([aA-záéíóúZÁÉÍÓÚ]{3}[a-zñáéíóú]+[\s]*)+$/.test(apellidom))) {
        cod = "el campo debe de acompletar por lo menos 3 caracteres ";
        camp = false;

    } else if (resF <= 17) {
        cod = "tienes que ser mayor de edad ";
        camp = false;
    } else if (/^(?:0[1-9]\d{3}|[1-4]\d{4}|5[0-2]\d{3})$/.test(cp)) {
        cod = "codigo postal incorecto o vacio";
        camp = false;
    } else if (!(/^([aA-zZñÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([aA-zZ\d]{3})?$/.test(rfc))) {

        cod = "esta incorrecto el rfc o esta vacio ";
        cap = false;
    }
    alert(cod);
    alert(fecha);
    return camp;
    
} 
