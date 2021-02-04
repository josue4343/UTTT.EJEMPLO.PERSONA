function validar() {
    debugger;
    var sexo = document.getElementById("ddlSexo");
    var clave = document.getElementById("txtClaveUnica").value;
    var nombre = document.getElementById("txtNombre").value;
    var apellidop = document.getElementById("txtAPaterno").value;
    var apellidom = document.getElementById("txtAMaterno").value;
    var fecha = document.getElementById("fecha");

    var Calendario = fecha.defaultValue;
    var fechapar = parseInt((""+ Calendario.substr(6, 10)));
    var ddlSexo = sexo.options[sexo.selectedIndex].value;
    var resF = 2021 - fechapar;

    var camp = true;

    var cod ="";

    if (clave == null || nombre == null || apellidop == null || apellidom == null || fecha == null) {

        cod = "El campo  esta vacio";
        camp = false;


    } else if (!(/[A-z]{3}/.test(nombre)) || !(/[A-z]{3}/.test(apellidop)) || !(/[A-z]{3}/.test(apellidom))) {
        cod = "el campo debe de ac ompletar por lo menos 3 caracteres ";
        camp = false;
    } else if (!(/^\d{3}$/.test(clave))) {
        cod = "ingresa mas de 3 dijitos ";
        camp = false;

    } else if (resF <= 17) {
        cod = "tienes que ser mayor de edad ";
        camp = false;
    } else if (ddlSexo < 0 || ddlSexo > 2) {
        cod = "elige el sexo";
        camp = false;

    }
    alert(cod);
    alert(fecha);
    return camp;

} 
