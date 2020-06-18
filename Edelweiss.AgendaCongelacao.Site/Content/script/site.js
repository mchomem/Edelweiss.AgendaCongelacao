///
function mostraCarregandoDados()
{
	bloqueiaPagina();
	$("#divCarregandoDados").show();
}

///
function fechaCarregandoDados()
{
	$("#divCarregandoDados").hide();
	desbloqueiaPagina();
}

///
function mostraSalvandoDados()
{
	$("#divSalvandoDados").show();
	bloqueiaConfirmacaoSolicitacao();
	
}

///
function fechaSalvandoDados()
{
	$("#divSalvandoDados").hide();
	desbloqueiaPagina();
}

///
function bloqueiaPagina()
{
	$("#divBlockPageJs").show();
}

///
function desbloqueiaPagina()
{
	$("#divBlockPageJs").hide();
}

///
function filtraNumero(event)
{

	if (event.keyCode < 48 || event.keyCode > 57)
		event.returnValue = false;
}

///
function mostraAviso(aviso)
{
	alertify.defaults.maintainFocus = false;	
	alertify.alert('Aviso', aviso).set('modal', false);	
}

///
function novaSolicitacao()
{
	window.location = 'NovaSolicitacaoMedica.aspx';
}

///
function listaSolicitacao()
{
	window.location = 'ListaSolicitacaoMedica.aspx';
}

function printPDF(filePath)
{
	var objFra = document.createElement('iframe');   // Create an IFrame.
	objFra.style.visibility = "hidden";    // Hide the frame.
	objFra.src = filePath; // Set source.
	document.body.appendChild(objFra);  // Add the frame to the web page.
	objFra.contentWindow.focus();       // Set focus.
	objFra.contentWindow.print();      // Print it.
}


