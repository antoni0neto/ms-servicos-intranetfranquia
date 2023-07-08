/*
Exemplo de Uso:
onkeypress="return fnValidarNumero(event);"
*/
function fnValidarNumero(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 8)
        return false;
    return true;
}
/*
Exemplo de Uso:
onkeypress="return fnValidarNumeroDecimal(event);"
*/
function fnValidarNumeroDecimal(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 44 && charCode != 8)
        return false;
    return true;
}
/*
Exemplo de Uso:
onkeypress="return fnValidarNumeroDecimalNegativo(event);"
*/
function fnValidarNumeroDecimalNegativo(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 44 && charCode != 8 && charCode != 45)
        return false;
    return true;
}
/*
Exemplo de Uso:
onkeypress="return fnValidarData(event);"
*/
function fnValidarData(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 47 && charCode != 8)
        return false;
    return true;
}

function fnValidarHora(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 58 && charCode != 8)
        return false;
    return true;
}

function fnReadOnly(event) {
    var charCode = (event.which) ? event.which : event.keyCode
    return false;
}
/*
Abrir pop_up
*/
function fnAbrirTelaCadastro(url) {
    var ret;
    var Left = 400;
    var Top = 120;

    ret = window.open(url, "_blank", "status=no, resizable=no, help=no, menubar=no, scrollbars=no, titlebar=no, toolbar=no, width=800, height=560, top=" + Top + ", left=" + Left);
}
function fnAbrirTelaCadastroMaior(url) {
    var ret;
    var Left = 400;
    var Top = 120;

    ret = window.open(url, "_blank", "status=no, resizable=no, help=no, menubar=no, scrollbars=no, titlebar=no, toolbar=no, width=1200, height=660, top=" + Top + ", left=" + Left);
}
function fnAbrirTelaCadastroMaior2(url) {
    var ret;
    var Left = 400;
    var Top = 120;

    ret = window.open(url, "_blank", "status=no, resizable=no, help=no, menubar=no, scrollbars=no, titlebar=no, toolbar=no, width=1250, height=750, top=" + Top + ", left=" + Left);
}
function fnAbrirTelaCadastroMaiorVert(url) {
    var ret;
    var Left = 400;
    var Top = 25;

    ret = window.open(url, "_blank", "status=no, resizable=no, help=no, menubar=no, scrollbars=no, titlebar=no, toolbar=no, width=1250, height=850, top=" + Top + ", left=" + Left);
}
/*
Confirmação de exclusão
*/
function ConfirmarExclusao() {
    if (confirm("Deseja realmente EXCLUIR este registro?")) {
        return true;
    }
    return false;
}
/*
Confirmação de Envio para o Risco
*/
function ConfirmarAmpliacao() {
    if (confirm("Deseja realmente enviar para o Risco?")) {
        return true;
    }
    return false;
}
/*
Confirmação de Relacionamento de TECIDO E HB
*/
function ConfirmarTecidoHB() {
    if (confirm("Deseja realmente relacionar o produto neste HB?")) {
        return true;
    }
    return false;
}
/*
Confirmação de Cancelamento
*/
function ConfirmarCancelamento() {
    if (confirm("Deseja realmente CANCELAR este registro?")) {
        return true;
    }
    return false;
}
/*
Confirmação de Aprovação
*/
function ConfirmarAprovacao() {
    if (confirm("Deseja realmente APROVAR este registro?")) {
        return true;
    }
    return false;
}
/*
Confirmação de BAIXA
*/
function ConfirmarBaixa() {
    if (confirm("Deseja realmente BAIXAR este registro?")) {
        return true;
    }
    return false;
}
/*
Confirmação de GENERICA
*/
function Confirmar(vMsg) {
    if (confirm(vMsg)) {
        return true;
    }
    return false;
}
/*
Abrir pop UP para impressão
*/
function AbrirImpressao(nomeArquivo) {
    window.open("html\\" + nomeArquivo, "_blank", "status=no, resizable=no, help=no, menubar=no, scrollbars=no, titlebar=no, toolbar=no, width=800, height=450, top=100, left=100");
}
/*
Abrir pop UP do pocket
*/
function AbrirPocket(nomeArquivo) {
    window.open("html\\" + nomeArquivo, "_blank", "status=no, resizable=no, help=no, menubar=no, scrollbars=no, titlebar=no, toolbar=no, width=800, height=750, top=100, left=100");
}
/*
Desabitar botão
*/
function DesabilitarBotao(objBotao) {
    objBotao.disabled = true;
}
//Seleciona todo o texto de um textbox
function selecionaFoco(txtbox) {
    txtbox.focus()
    txtbox.select()
}

function formataMascara(campo, evt, formato) {
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;


    var result = "";
    var maskIdx = formato.length - 1;
    var error = false;
    var valor = campo.value;
    var posFinal = false;
    if (campo.setSelectionRange) {
        if (campo.selectionStart == valor.length)
            posFinal = true;
    }

    valor = valor.replace(/[^0123456789Xx]/g, '');
    for (var valIdx = valor.length - 1; valIdx >= 0 && maskIdx >= 0; --maskIdx) {
        var chr = valor.charAt(valIdx);
        var chrMask = formato.charAt(maskIdx);
        switch (chrMask) {
            case '#':
                if (!(/\d/.test(chr)))
                    error = true;
                result = chr + result;
                --valIdx;
                break;
            case '@':
                result = chr + result;
                --valIdx;
                break;
            default:
                result = chrMask + result;
        }
    }

    campo.value = result;
    campo.style.color = error ? 'red' : '';
    if (posFinal) {
        campo.selectionStart = result.length;
        campo.selectionEnd = result.length;
    }
    return result;
}

function alta(valor) {
    valor.value = valor.value.toUpperCase();
}

// Formata o campo valor monetário
function formataValor(campo, evt) {
    //1.000.000,00
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    vr = campo.value = filtraNumeros(filtraCampo(campo));
    if (vr.length > 0) {
        vr = parseFloat(vr.toString()).toString();
        tam = vr.length;

        if (tam == 1)
            campo.value = "0,0" + vr;
        if (tam == 2)
            campo.value = "0," + vr;
        if ((tam > 2) && (tam <= 5)) {
            campo.value = vr.substr(0, tam - 2) + ',' + vr.substr(tam - 2, tam);
        }
        if ((tam >= 6) && (tam <= 8)) {
            campo.value = vr.substr(0, tam - 5) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
        }
        if ((tam >= 9) && (tam <= 11)) {
            campo.value = vr.substr(0, tam - 8) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
        }
        if ((tam >= 12) && (tam <= 14)) {
            campo.value = vr.substr(0, tam - 11) + '.' + vr.substr(tam - 11, 3) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
        }
        if ((tam >= 15) && (tam <= 18)) {
            campo.value = vr.substr(0, tam - 14) + '.' + vr.substr(tam - 14, 3) + '.' + vr.substr(tam - 11, 3) + '.' + vr.substr(tam - 8, 3) + '.' + vr.substr(tam - 5, 3) + ',' + vr.substr(tam - 2, tam);
        }
    }
    MovimentaCursor(campo, xPos);
}

// Formata data no padrão DD/MM/YYYY
function formataData(campo, evt) {
    var xPos = PosicaoCursor(campo);
    //dd/MM/yyyy
    evt = getEvent(evt);

    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;

    if (tam >= 2 && tam < 4)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2);
    if (tam == 4)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/';
    if (tam > 4)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/' + vr.substr(4);

    MovimentaCursor(campo, xPos);
}

//descobre qual a posição do cursor no campo
function PosicaoCursor(textarea) {
    var pos = 0;
    if (typeof (document.selection) != 'undefined') {
        //IE
        var range = document.selection.createRange();
        var i = 0;
        for (i = textarea.value.length; i > 0; i--) {
            if (range.moveStart('character', 1) == 0)
                break;
        }
        pos = i;
    }
    if (typeof (textarea.selectionStart) != 'undefined') {
        //FireFox
        pos = textarea.selectionStart;
    }

    if (pos == textarea.value.length)
        return 0; //retorna 0 quando não precisa posicionar o elemento
    else
        return pos; //posição do cursor
}

// move o cursor para a posição pos
function MovimentaCursor(textarea, pos) {
    if (pos <= 0)
        return; //se a posição for 0 não reposiciona

    if (typeof (document.selection) != 'undefined') {
        //IE
        var oRange = textarea.createTextRange();
        var LENGTH = 1;
        var STARTINDEX = pos;

        oRange.moveStart("character", -textarea.value.length);
        oRange.moveEnd("character", -textarea.value.length);
        oRange.moveStart("character", pos);
        //oRange.moveEnd("character", pos);
        oRange.select();
        textarea.focus();
    }
    if (typeof (textarea.selectionStart) != 'undefined') {
        //FireFox
        textarea.selectionStart = pos;
        textarea.selectionEnd = pos;
    }
}

//Formata data e hora no padrão DD/MM/YYYY HH:MM
function formataDataeHora(campo, evt) {
    xPos = PosicaoCursor(campo);
    //dd/MM/yyyy
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;

    if (tam >= 2 && tam < 4)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2);
    if (tam == 4)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/';
    if (tam > 4)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/' + vr.substr(4);
    if (tam > 8 && tam < 11)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/' + vr.substr(4, 4) + ' ' + vr.substr(8, 2);
    if (tam >= 11)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2, 2) + '/' + vr.substr(4, 4) + ' ' + vr.substr(8, 2) + ':' + vr.substr(10);

    campo.value = campo.value.substr(0, 16);
    //    if(xPos == 2 || xPos == 5)
    //        xPos = xPos +1;
    //    if(xPos == 11 || xPos == 14)
    //        xPos = xPos +2;
    MovimentaCursor(campo, xPos);
}

// Formata só números
function formataInteiro(campo, evt) {
    //1234567890
    xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    campo.value = filtraNumeros(filtraCampo(campo));
    MovimentaCursor(campo, xPos);
}

// Formata hora no padrao HH:MM
function formataHora(campo, evt) {
    //HH:mm
    xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    vr = campo.value = filtraNumeros(filtraCampo(campo));

    if (tam == 2)
        campo.value = vr.substr(0, 2) + ':';
    if (tam > 2 && tam < 5)
        campo.value = vr.substr(0, 2) + ':' + vr.substr(2);
    //    if(xPos == 2)
    //        xPos = xPos + 1;
    MovimentaCursor(campo, xPos);
}

// limpa todos os caracteres especiais do campo solicitado
function filtraCampo(campo) {
    var s = "";
    var cp = "";
    vr = campo.value;
    tam = vr.length;
    for (i = 0; i < tam; i++) {
        if (vr.substring(i, i + 1) != "/"
            && vr.substring(i, i + 1) != "-"
            && vr.substring(i, i + 1) != "."
            && vr.substring(i, i + 1) != "("
            && vr.substring(i, i + 1) != ")"
            && vr.substring(i, i + 1) != ":"
            && vr.substring(i, i + 1) != ",") {
            s = s + vr.substring(i, i + 1);
        }
    }
    return s;
    //return campo.value.replace("/", "").replace("-", "").replace(".", "").replace(",", "")
}

// limpa todos caracteres que não são números
function filtraNumeros(campo) {
    var s = "";
    var cp = "";
    vr = campo;
    tam = vr.length;
    for (i = 0; i < tam; i++) {
        if (vr.substring(i, i + 1) == "0" ||
            vr.substring(i, i + 1) == "1" ||
            vr.substring(i, i + 1) == "2" ||
            vr.substring(i, i + 1) == "3" ||
            vr.substring(i, i + 1) == "4" ||
            vr.substring(i, i + 1) == "5" ||
            vr.substring(i, i + 1) == "6" ||
            vr.substring(i, i + 1) == "7" ||
            vr.substring(i, i + 1) == "8" ||
            vr.substring(i, i + 1) == "9") {
            s = s + vr.substring(i, i + 1);
        }
    }
    return s;
    //return campo.value.replace("/", "").replace("-", "").replace(".", "").replace(",", "")
}

// limpa todos caracteres que não são letras
function filtraCaracteres(campo) {
    vr = campo;
    for (i = 0; i < tam; i++) {
        //Caracter
        if (vr.charCodeAt(i) != 32 && vr.charCodeAt(i) != 94 && (vr.charCodeAt(i) < 65 ||
        (vr.charCodeAt(i) > 90 && vr.charCodeAt(i) < 96) ||
            vr.charCodeAt(i) > 122) && vr.charCodeAt(i) < 192) {
            vr = vr.replace(vr.substr(i, 1), "");
        }
    }
    return vr;
}

// limpa todos caracteres que não são números, menos a vírgula
function filtraNumerosComVirgula(campo) {
    var s = "";
    var cp = "";
    vr = campo;
    tam = vr.length;
    var complemento = 0; //flag paga contar o número de virgulas
    for (i = 0; i < tam; i++) {
        if ((vr.substring(i, i + 1) == "," && complemento == 0 && s != "") ||
            vr.substring(i, i + 1) == "0" ||
            vr.substring(i, i + 1) == "1" ||
            vr.substring(i, i + 1) == "2" ||
            vr.substring(i, i + 1) == "3" ||
            vr.substring(i, i + 1) == "4" ||
            vr.substring(i, i + 1) == "5" ||
            vr.substring(i, i + 1) == "6" ||
            vr.substring(i, i + 1) == "7" ||
            vr.substring(i, i + 1) == "8" ||
            vr.substring(i, i + 1) == "9") {
            if (vr.substring(i, i + 1) == ",")
                complemento = complemento + 1;
            s = s + vr.substring(i, i + 1);
        }
    }
    return s;
}

function formataMesAno(campo, evt) {
    //MM/yyyy
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;

    if (tam >= 2)
        campo.value = vr.substr(0, 2) + '/' + vr.substr(2);
    MovimentaCursor(campo, xPos);
}

function formataCNPJ(campo, evt) {
    //99.999.999/9999-99
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;

    if (tam >= 2 && tam < 5)
        campo.value = vr.substr(0, 2) + '.' + vr.substr(2);
    else if (tam >= 5 && tam < 8)
        campo.value = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5);
    else if (tam >= 8 && tam < 12)
        campo.value = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8);
    else if (tam >= 12)
        campo.value = vr.substr(0, 2) + '.' + vr.substr(2, 3) + '.' + vr.substr(5, 3) + '/' + vr.substr(8, 4) + '-' + vr.substr(12);
    MovimentaCursor(campo, xPos);
}

function formataCPF(campo, evt) {
    //999.999.999-99
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;
    if (tam >= 3 && tam < 6)
        campo.value = vr.substr(0, 3) + '.' + vr.substr(3);
    else if (tam >= 6 && tam < 9)
        campo.value = vr.substr(0, 3) + '.' + vr.substr(3, 3) + '.' + vr.substr(6);
    else if (tam >= 9)
        campo.value = vr.substr(0, 3) + '.' + vr.substr(3, 3) + '.' + vr.substr(6, 3) + '-' + vr.substr(9);
    MovimentaCursor(campo, xPos);
}

function formataDouble(campo, evt) {
    //18,53012
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    campo.value = filtraNumerosComVirgula(campo.value);
    MovimentaCursor(campo, xPos);
}

function formataTelefone(campo, evt) {
    //(00) 0000-0000
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;

    if (tam == 1)
        campo.value = '(' + vr;
    else if (tam >= 2 && tam < 6)
        campo.value = '(' + vr.substr(0, 2) + ') ' + vr.substr(2);
    else if (tam >= 6)
        campo.value = '(' + vr.substr(0, 2) + ') ' + vr.substr(2, 4) + '-' + vr.substr(6);

    //(
    //    if(xPos == 1 || xPos == 3 || xPos == 5 || xPos == 9)
    //        xPos = xPos +1
    MovimentaCursor(campo, xPos);
}

function formataTexto(campo, evt, sMascara) {
    //Nome com Inicial Maiuscula.
    evt = getEvent(evt);
    xPos = PosicaoCursor(campo);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;
    vr = campo.value = filtraCaracteres(filtraCampo(campo));
    tam = vr.length;

    if (sMascara == "Aa" || sMascara == "Xx") {
        var valor = campo.value.toLowerCase();
        var count = campo.value.split(" ").length - 1;
        var i;
        var pos = 0;
        var valorIni;
        var valorMei;
        var valorFim;
        valor = valor.substring(0, 1).toUpperCase() + valor.substring(1, valor.length);
        for (i = 0; i < count; i++) {
            pos = valor.indexOf(" ", pos + 1);
            valorIni = valor.substring(0, valor.indexOf(" ", pos - 1)) + " ";
            valorMei = valor.substring(valor.indexOf(" ", pos) + 1, valor.indexOf(" ", pos) + 2).toUpperCase();
            valorFim = valor.substring(valor.indexOf(" ", pos) + 2, valor.length);
            valor = valorIni + valorMei + valorFim;
        }
        campo.value = valor;
    }
    if (sMascara == "Aaa" || sMascara == "Xxx") {
        var valor = campo.value.toLowerCase();
        var count = campo.value.split(" ").length - 1;
        var i;
        var pos = 0;
        var valorIni;
        var valorMei;
        var valorFim;
        var ligacao = false;
        var chrLigacao = Array("de", "da", "do", "para", "e")
        valor = valor.substring(0, 1).toUpperCase() + valor.substring(1, valor.length);
        for (i = 0; i < count; i++) {
            ligacao = false;
            pos = valor.indexOf(" ", pos + 1);
            valorIni = valor.substring(0, valor.indexOf(" ", pos - 1)) + " ";
            for (var a = 0; a < chrLigacao.length; a++) {
                if (valor.substring(valorIni.length, valor.indexOf(" ", valorIni.length)).toLowerCase() == chrLigacao[a].toLowerCase()) {
                    ligacao = true;
                    break;
                }
                else if (ligacao == false && valor.indexOf(" ", valorIni.length) == -1) {
                    if (valor.substring(valorIni.length, valor.length).toLowerCase() == chrLigacao[a].toLowerCase()) {
                        ligacao = true;
                        break;
                    }
                }
            }
            if (ligacao == true) {
                valorMei = valor.substring(valor.indexOf(" ", pos) + 1, valor.indexOf(" ", pos) + 2).toLowerCase();
            }
            else {
                valorMei = valor.substring(valor.indexOf(" ", pos) + 1, valor.indexOf(" ", pos) + 2).toUpperCase();
            }
            valorFim = valor.substring(valor.indexOf(" ", pos) + 2, valor.length);
            valor = valorIni + valorMei + valorFim;
        }

        campo.value = valor;
    }
    MovimentaCursor(campo, xPos);
    return true;
}

// Formata o campo CEP
function formataCEP(campo, evt) {
    //312555-650
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    vr = campo.value = filtraNumeros(filtraCampo(campo));
    tam = vr.length;

    if (tam < 5)
        campo.value = vr;
    else if (tam == 5)
        campo.value = vr + '-';
    else if (tam > 5)
        campo.value = vr.substr(0, 5) + '-' + vr.substr(5);
    MovimentaCursor(campo, xPos);
}

function formataCartaoCredito(campo, evt) {
    //0000.0000.0000.0000
    var xPos = PosicaoCursor(campo);
    evt = getEvent(evt);
    var tecla = getKeyCode(evt);
    if (!teclaValida(tecla))
        return;

    var vr = campo.value = filtraNumeros(filtraCampo(campo));
    var tammax = 16;
    var tam = vr.length;

    if (tam < tammax && tecla != 8)
    { tam = vr.length + 1; }

    if (tam < 5)
    { campo.value = vr; }
    if ((tam > 4) && (tam < 9))
    { campo.value = vr.substr(0, 4) + '.' + vr.substr(4, tam - 4); }
    if ((tam > 8) && (tam < 13))
    { campo.value = vr.substr(0, 4) + '.' + vr.substr(4, 4) + '.' + vr.substr(8, tam - 4); }
    if (tam > 12)
    { campo.value = vr.substr(0, 4) + '.' + vr.substr(4, 4) + '.' + vr.substr(8, 4) + '.' + vr.substr(12, tam - 4); }
    MovimentaCursor(campo, xPos);
}

//recupera tecla
//evita criar mascara quando as teclas são pressionadas
function teclaValida(tecla) {
    if (tecla == 8 //backspace
        //Esta evitando o post, quando são pressionadas estas teclas.
        //Foi comentado pois, se for utilizado o evento texchange, é necessario o post.
        || tecla == 9 //TAB
        || tecla == 27 //ESC
        || tecla == 16 //Shif TAB 
        || tecla == 45 //insert
        || tecla == 46 //delete
        || tecla == 35 //home
        || tecla == 36 //end
        || tecla == 37 //esquerda
        || tecla == 38 //cima
        || tecla == 39 //direita
        || tecla == 40)//baixo
        return false;
    else
        return true;
}

// recupera o evento do form
function getEvent(evt) {
    if (!evt) evt = window.event; //IE
    return evt;
}

//Recupera o código da tecla que foi pressionado
function getKeyCode(evt) {
    var code;
    if (typeof (evt.keyCode) == 'number')
        code = evt.keyCode;
    else if (typeof (evt.which) == 'number')
        code = evt.which;
    else if (typeof (evt.charCode) == 'number')
        code = evt.charCode;
    else
        return 0;

    return code;
}

// Invoked to check/uncheck all when header checkbox is clicked
// ---------------------------------------------------------------
function CheckAll(me) {
    // Assume the ID of this element is controlID_HeaderButton 
    // (i.e. GridView1_HeaderButton)

    var index = me.name.indexOf('_');
    var prefix = me.name.substr(0, index);

    // Looks for the right checkbox
    for (i = 0; i < document.forms[0].length; i++) {
        var o = document.forms[0][i];
        if (o.type == 'checkbox') {
            if (me.name != o.name) {
                if (o.name.substring(0, prefix.length) == prefix) {
                    // Needs to set the state of the header to any child 
                    // checkbox. To avoid passing too many parameters, 
                    // sets state by simulating the click. To ensure that 
                    // the click sets the checkbox to the "me.checked" 
                    // state, it must first the current value. 
                    o.checked = !me.checked;
                    o.click();
                }
            }
        }
    }
}

// Invoked to properly style the checkbox on a row
// ---------------------------------------------------------
function ApplyStyle(me, selectedForeColor, selectedBackColor, foreColor, backColor, bold, checkBoxHeaderId) {
    var td = me.parentNode;
    if (td == null)
        return;

    var tr = td.parentNode;
    if (me.checked) {
        tr.style.fontWeight = 700;
        tr.style.color = selectedForeColor;
        tr.style.backgroundColor = selectedBackColor;
    }
    else {
        document.getElementById(checkBoxHeaderId).checked = false;
        tr.style.fontWeight = bold;
        tr.style.color = foreColor;
        tr.style.backgroundColor = backColor;
    }
}

//Seleciona todo o texto de um textbox
function selecionaFoco(txtbox) {
    txtbox.focus()
    txtbox.select()
}