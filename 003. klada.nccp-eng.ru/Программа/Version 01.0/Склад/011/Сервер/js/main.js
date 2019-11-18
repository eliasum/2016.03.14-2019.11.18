// функция выбора только одного checkbox для порядка сортировки
function checkTheBox(elem)  
{
  var ascVar=document.getElementById('ascCB');
  var descVar=document.getElementById('descCB');
  
  switch(elem.id)
  {
    case 'ascCB':{descVar.checked=!ascVar.checked}break;
    case 'descCB':{ascVar.checked=!descVar.checked}break;
  }
}

// функция выбора сортировки или 'по умолчанию' или '...'
(function(master, slaves) {  
  var mm = master.childNodes, ss = slaves.childNodes;
  function F(e, a) {
    e = (e = e || event).srcElement || e.target; 
    if(e.tagName.toLowerCase() == 'input' || e.tagName.toLowerCase() == 'label' && !e.checked)
      for(var i = 0, l = a.length; i < l; i++) 
        if(a[i].checked) a[i].checked = 0;
  }
  master.onclick = function(e) { return F(e, ss); }
  slaves.onclick = function(e) { return F(e, mm); }
})(document.getElementById('master'), document.getElementById('slaves'));

// функция выбора хотя бы одного checkbox для выбора сортировки
function checkTheSort(elem)  
{
  var idVar=document.getElementById('idID');
  var nameVar=document.getElementById('nameID');
  var unitVar=document.getElementById('unitID');
  var producerVar=document.getElementById('producerID');
  var codeVar=document.getElementById('codeID');
  var dateVar=document.getElementById('dateID');
  
  if((idVar.checked==false)&&(nameVar.checked==false)&&
     (unitVar.checked==false)&&(producerVar.checked==false)&&
	 (codeVar.checked==false)&&(dateVar.checked==false))  
  {
    alert('Нельзя просто так взять и отключить все галочки сортировки!');
	elem.checked=true;
  }
}

// подогнать ширину элементов под ширину элемента <fieldset id="block1"> с помощью jQuery
function resize()
{
    $('#block2').width($('#block1').width())
	$('#block3').width($('#block1').width()/2.5)
}
resize();
$( window ).resize(function() {
    resize()
});

// функции очистки полей ввода поискового запроса
function clearAllFields()
{
  var search1Var=document.getElementById('search1');
  var search2Var=document.getElementById('search2');
  var search3Var=document.getElementById('search3');
  
  search1Var.value="";
  search2Var.value="";
  search3Var.value="";
}

function clear1Fields()
{
  var search2Var=document.getElementById('search2');
  var search3Var=document.getElementById('search3');
  
  search2Var.value="";
  search3Var.value="";
}

function clear2Fields()
{
  var search1Var=document.getElementById('search1');
  var search3Var=document.getElementById('search3');
  
  search1Var.value="";
  search3Var.value="";
}

function clear3Fields()
{
  var search1Var=document.getElementById('search1');
  var search2Var=document.getElementById('search2');
  
  search1Var.value="";
  search2Var.value="";
}