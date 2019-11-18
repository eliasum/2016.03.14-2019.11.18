<?php
ob_start();
/*
Эта функция включает буферизацию вывода. Если буферизация вывода активна, вывод скрипта не высылается (кроме заголовков), а сохраняется во внутреннем буфере.
bool ob_start ([ callable $output_callback = NULL [, int $chunk_size = 0 [, int $flags = PHP_OUTPUT_HANDLER_STDFLAGS ]]] )
*/

session_start(); // Стартуем сессию

// Записываем в сессию состояние checkbox, если checked - записываем 1, если нет - 0
$_SESSION['id_checked']       = (int) isset($_POST['idVal']); 
$_SESSION['name_checked']     = (int) isset($_POST['nameVal']); 
$_SESSION['unit_checked']     = (int) isset($_POST['unitVal']); 
$_SESSION['producer_checked'] = (int) isset($_POST['producerVal']); 
$_SESSION['code_checked']     = (int) isset($_POST['codeVal']); 
$_SESSION['date_checked']     = (int) isset($_POST['dateVal']); 
$_SESSION['asc_checked']      = (int) isset($_POST['ascVal']); 
$_SESSION['desc_checked']     = (int) isset($_POST['descVal']); 
 
$checkboxes = array(
// Записываем в переменную текущее состояние галочки, если есть сессия и она равна 1, то записуем true, если нет - false
  'idInd'       => (isset($_SESSION['id_checked'])       && $_SESSION['id_checked']       == 1 ? true : false),
  'nameInd'     => (isset($_SESSION['name_checked'])     && $_SESSION['name_checked']     == 1 ? true : false),
  'unitInd'     => (isset($_SESSION['unit_checked'])     && $_SESSION['unit_checked']     == 1 ? true : false), 
  'producerInd' => (isset($_SESSION['producer_checked']) && $_SESSION['producer_checked'] == 1 ? true : false),
  'codeInd'     => (isset($_SESSION['code_checked'])     && $_SESSION['code_checked']     == 1 ? true : false), 
  'dateInd'     => (isset($_SESSION['date_checked'])     && $_SESSION['date_checked']     == 1 ? true : false),
  'ascInd'      => (isset($_SESSION['asc_checked'])      && $_SESSION['asc_checked']      == 1 ? true : false), 
  'descInd'     => (isset($_SESSION['desc_checked'])     && $_SESSION['desc_checked']     == 1 ? true : false),  
);

///////////////////////////////////////////////////////////
  $b="false";  // признак "самого первого" запуска страницы:

  if(($checkboxes['idInd']==0)&&
     ($checkboxes['nameInd']==0)&&
     ($checkboxes['unitInd']==0)&&
	 ($checkboxes['producerInd']==0)&&
     ($checkboxes['codeInd']==0)&&
     ($checkboxes['dateInd']==0)&&
	 ($checkboxes['ascInd']==0)&&
	 ($checkboxes['descInd']==0))
  {
    $b = "true";
  }
  else 
  {
    $b = "false";
  }
///////////////////////////////////////////////////////////
?>
<!doctype html>
<html>
<head>
  <!-- Тег link - устанавливает связь с внешним документом вроде файла со стилями или со шрифтами. В отличие от тега <a>, тег <link> размещается всегда внутри контейнера <head> и не создает ссылку.
  Атрибут rel - Определяет отношения между текущим документом и файлом, на который делается ссылка.
  Атрибут href - путь к связываемому файлу.
  Атрибут type - MIME-тип данных подключаемого файла.
  MIME (Multipurpose Internet Mail Extension, Многоцелевые расширения почты Интернета) — спецификация для передачи по сети файлов различного типа: изображений, музыки, текстов, видео, архивов и др. Указание MIME-типа используется в HTML обычно при передаче данных форм и вставки на страницу различных объектов.-->
  <link rel="icon" href="favicon.ico" type="image/x-icon"><!-- Иконка сайта -->
  <link rel="stylesheet" href="style.css" type="text/css"/>
  <!-- Тег <meta> - определяет метатеги, которые используются для хранения информации предназначенной для браузеров и поисковых систем. Например, механизмы поисковых систем обращаются к метатегам для получения описания сайта, ключевых слов и других данных. Разрешается использовать более чем один метатег, все они размещаются в контейнере <head>. Как правило, атрибуты любого метатега сводятся к парам «имя=значение», которые определяются ключевыми словами content, name или http-equiv.
  Атрибут http-equiv - предназначен для конвертирования метатега в заголовок HTTP.
  Занчение "Content-Type" - тип кодировки документа.
  Атрибут content - устанавливает значение атрибута, заданного с помощью name или http-equiv. -->
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <script type="text/javascript" src="jquery-3.2.1.min.js"></script>
  <title>Склад. Остаток.</title>
</head>

<body>

<p><b>Склад. Остаток.</b></p>

<div class="links">
  <a href="addName.php">Добавить наименование</a>
  <a href="income.php">Приход</a> 
  <a href="expense.php">Расход</a> 
</div><br />

<form method="post" action="home.php">
<!-- Элемент <fieldset> предназначен для группирования элементов формы. Такая группировка облегчает работу с формами, содержащими большое число данных. Например, один блок может быть предназначен для ввода текстовой информации, а другой — для флажков.
Браузеры для повышения наглядности отображают результат использования тега <fieldset> в виде рамки. Ее вид зависит от операционной системы, а также используемого браузера. -->
  <fieldset id="block1" name="sortingGroup" style="border:1px solid grey; display:inline;">
    <legend>Сортировать по полю:</legend>

      <div class="fieldgroup" id='master'>
	    <input id='idID' type="checkbox" <?=($checkboxes['idInd'] ? 'checked' : null); ?> name="idVal" />
		<label for='idID'> По умолчанию</label>
     </div>	
	 	
	<div class="fieldgroup">|</div>
	
	<div class="fieldgroup" id='slaves'>
      <input id="nameID" type="checkbox" <?=($checkboxes['nameInd'] ? 'checked' : null); ?> name="nameVal" />
	  <label for='nameID'> Наименование</label>

      <input style="margin-left: 3em;" id="unitID" type="checkbox" <?=($checkboxes['unitInd'] ? 'checked' : null); ?> name="unitVal" />
	  <label for='unitID'> Единица измерения</label>
	
      <input style="margin-left: 3em;" id="producerID" type="checkbox" <?=($checkboxes['producerInd'] ? 'checked' : null); ?> name="producerVal" />
	  <label for='producerID'> Производитель</label>
	
      <input style="margin-left: 3em;" id="codeID" type="checkbox" <?=($checkboxes['codeInd'] ? 'checked' : null); ?> name="codeVal" />
	  <label for='codeID'> Код</label>
	
      <input style="margin-left: 3em;" id="dateID" type="checkbox" <?=($checkboxes['dateInd'] ? 'checked' : null); ?> name="dateVal" />
	  <label for='dateID'> Дата</label>
	</div>
  </fieldset><br />
  
  <fieldset id="block2" name="orderGroup" style="border:1px solid grey; display:inline;">
    <legend>Порядкок сортировки:</legend>

    <div class="fieldgroup" >
	
    <input type="checkbox" <?=($checkboxes['ascInd'] ? 'checked' : null); ?> name="ascVal" id="ascCB" onClick="checkTheBox(this)"/>
	<label for='ascCB'> Прямой</label>
	
    <input style="margin-left: 3em;" type="checkbox" <?=($checkboxes['descInd'] ? 'checked' : null); ?> name="descVal" id="descCB" onClick="checkTheBox(this)"/>
	<label for='descCB'> Обратный</label>

	</div>
	
  </fieldset><br /><br />  
  <input type="submit" name="setButton"/>
</form><br />

<script type="text/javascript"> 

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

// алгоритм установки чекбоксов при "самой первой" загрузке страницы
var b = '<?php echo $b;?>';

if(b=="true") 
{
  document.getElementById('idID').checked=true;
  document.getElementById('ascCB').checked=true;
}

// подогнать ширину элемента <fieldset id="block2"> под ширину элемента <fieldset id="block1"> с помощью jQuery
function resize()
{
    $('#block2').width($('#block1').width())
}
resize();
$( window ).resize(function() {
    resize()
});

</script> 

<table>                                                         <!-- Шапка таблицы -->
  <tr>
    <th class="row1"><b>№ п/п</b></th>
    <th class="row2"><b>Наименование</b></th>
    <th class="row3"><b>Единица измерения</b></th>
	<th class="row4"><b>Производитель</b></th>
	<th class="row5"><b>Код</b></th>
	<th class="row6"><b>Количество</b></th>
    <th class="row7"><b>Дата</b></th>
	<th class="row8"><b>Комната/шкаф/полка</b></th>
	<th class="row9"><b>Действие</b></th>
  </tr>

<?php

if(isset($_POST['setButton']))                                  // если кнопка 'setButton' была нажата
{
  // выбор сортировки по полям
  if($checkboxes['idInd']==1) $sorting = "id";
  else
  {
    if($checkboxes['nameInd']==1) $sorting = "name";
    else                          $sorting = "";
  
    if($checkboxes['unitInd']==1)
    {
      if($sorting != "") $sorting .= ",unit";
	  else               $sorting .= "unit";
    }

    if($checkboxes['producerInd']==1) 
    {
      if($sorting != "") $sorting .= ",producer";
	  else               $sorting .= "producer";
    }                        

    if($checkboxes['codeInd']==1)
    {
      if($sorting != "") $sorting .= ",code";
	  else               $sorting .= "code";
    } 

    if($checkboxes['dateInd']==1)      
    {
      if($sorting != "") $sorting .= ",date";
	  else               $sorting .= "date";
    }  
  }

  // выбор порядка сортировки
  if($checkboxes['ascInd']==1) $order = "ASC";
  if($checkboxes['descInd']==1) $order = "DESC";  
  
include_once("db.php");                                         // однократное подключение к БД

// запрос на выбор всех полей из таблицы equips, сортировать по $sorting в $order порядке
$sql = " SELECT * FROM equips ORDER BY " . $sorting. " " . $order;  

$result = mysql_query($sql);                                    // результат запроса

mysql_close();                                                  // закрыть соединение с бд

//$row = mysql_fetch_array($result);                            // записать таблицу equips в переменную $row как массив

while($row = mysql_fetch_array($result))                        // mysql_fetch_array($result) || mysql_fetch_assoc($result)
{
?>
  <tr>
    <td><?php echo $row['id'];?></td>
    <td><?php echo $row['name'];?></td>
	<td><?php echo $row['unit'];?></td>
	<td><?php echo $row['producer'];?></td>
	<td><?php echo $row['code'];?></td>
	<td><?php echo $row['count'];?></td>
	<td><?php echo $row['date'];?></td>
	<td><?php echo $row['ksp'];?></td>
    <td>
	  <div class="links">
	    <a href="put.php?id=<?php echo $row['id']?>">Оприходовать</a>  <!-- Ссылка на страницу 'Оприходовать' для наименования с id -->
        <a href="take.php?id=<?php echo $row['id']?>">Расходовать</a>  <!-- Ссылка на страницу 'Расходовать' для наименования с id -->
	  </div>
	</td>
  </tr>
<?php
}  
}
else
{
include_once("db.php");                                         // однократное подключение к БД

$result = mysql_query(" SELECT *
                        FROM equips
                        ORDER BY id ASC
					  ");                                       // запрос на выбор всех полей из таблицы equips, сортировать по id

mysql_close();                                                  // закрыть соединение с бд

//$row = mysql_fetch_array($result);                            // записать таблицу equips в переменную $row как массив

while($row = mysql_fetch_array($result))                        // mysql_fetch_array($result) || mysql_fetch_assoc($result)
{
?>
  <tr>
    <td><?php echo $row['id'];?></td>
    <td><?php echo $row['name'];?></td>
	<td><?php echo $row['unit'];?></td>
	<td><?php echo $row['producer'];?></td>
	<td><?php echo $row['code'];?></td>
	<td><?php echo $row['count'];?></td>
	<td><?php echo $row['date'];?></td>
	<td><?php echo $row['ksp'];?></td>
    <td>
	  <div class="links">
	    <a href="put.php?id=<?php echo $row['id']?>">Оприходовать</a>  <!-- Ссылка на страницу 'Оприходовать' для наименования с id -->
        <a href="take.php?id=<?php echo $row['id']?>">Расходовать</a>  <!-- Ссылка на страницу 'Расходовать' для наименования с id -->
	  </div>
	</td>
  </tr>
<?php
}
}
?>

</table>
<script src="js/jquery-3.2.1.min.js"></script> 
</body>
</html>