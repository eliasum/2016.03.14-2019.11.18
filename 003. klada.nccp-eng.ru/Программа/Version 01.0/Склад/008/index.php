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
$_SESSION['w_checked']        = (int) isset($_POST['wVal']);
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
  'wInd'        => (isset($_SESSION['w_checked'])        && $_SESSION['w_checked']        == 1 ? true : false),
  'unitInd'     => (isset($_SESSION['unit_checked'])     && $_SESSION['unit_checked']     == 1 ? true : false), 
  'producerInd' => (isset($_SESSION['producer_checked']) && $_SESSION['producer_checked'] == 1 ? true : false),
  'codeInd'     => (isset($_SESSION['code_checked'])     && $_SESSION['code_checked']     == 1 ? true : false), 
  'dateInd'     => (isset($_SESSION['date_checked'])     && $_SESSION['date_checked']     == 1 ? true : false),
  'ascInd'      => (isset($_SESSION['asc_checked'])      && $_SESSION['asc_checked']      == 1 ? true : false), 
  'descInd'     => (isset($_SESSION['desc_checked'])     && $_SESSION['desc_checked']     == 1 ? true : false),  
);

//---------------------------------------------------->
  $b="false";  // признак "самого первого" запуска страницы:

  if(($checkboxes['idInd']==0)&&
     ($checkboxes['nameInd']==0)&&
     ($checkboxes['wInd']==0)&&
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
//---------------------------------------------------->
// Получение информации о пользователе из домена:
$authorized_users = array('eng-171', 'eng-174', 'eng-229', 'eng-233','srv2012-testweb-v');  // разрешенные пользователи

// 1. Получаем имя удаленного хоста, при входе пользователя на страницу.
$hostname = gethostbyaddr($_SERVER['REMOTE_ADDR']);

/*
2. Убираем все символы после первой точки (т.к. полученное имя имя будет иметь формат «логин.субдомен.домен») и в переменную «текущий пользователь» берем результат.
*/
list($hostname,) = explode('.',$hostname);
$cur_user=$hostname;

//$cur_user='eng-100500';

// 3. Проверка пользователя на разрешенный доступ:
$authorization=0;

for($i=0;$i<count($authorized_users); $i++)
{
  if($authorized_users[$i] == $cur_user) $authorization++;             
}
 
$_SESSION['authorization'] = $authorization;                    // записать $authorization

if($authorization==0)                                           // проверка пользователя
{	
  echo "<b><span style='color:blue;'>Вам недоступен ввод и редактирование записей!</span></b>";
}
//---------------------------------------------------->

// Функция построения таблицы ответа на запрос:
function tableBuild($query,$authorization)                      
{
  include_once("db.php");                                       // однократное подключение к БД	
  
  $result = mysql_query($query) or die ("<b><span style='color:red;'>Запрос ошибочный!</span></b>");   // результат запроса
  
  mysql_close();                                                // закрыть соединение с бд
 
  //$row = mysql_fetch_array($result);                          // записать результат запроса в переменную $row как массив
  
  if(mysql_num_rows($result) > 0)                               // если количество рядов результата запроса больше нуля
  {
    echo 
    "<table> 
       <tr>                                                        
         <th class='row1'><b>№ п/п</b></th>
         <th class='row2'><b>Наименование</b></th>
         <th class='row3'><b>Единица измерения</b></th>
	     <th class='row4'><b>Производитель</b></th>
	     <th class='row5'><b>Код</b></th>
	     <th class='row6'><b>Количество</b></th>
         <th class='row7'><b>Дата</b></th>
	     <th class='row8'><b>Место хранения (комната/шкаф/полка)</b></th>";
//---------------------------------------------------->		 
	if($authorization!=0)                                       // проверка пользователя
    {	
	  echo
	    "<th class='row9'><b>Действие</b></th>";                // открыть доступ к действиям
	}
//---------------------------------------------------->
    echo
      "</tr>";                                                  
	   
    while($row = mysql_fetch_array($result))                    // mysql_fetch_array($result) || mysql_fetch_assoc($result)
    {                                                           // шапка таблицы
      echo                                                      // открыть доступ к действиям
      "<tr>
         <td>" . $row['id'] . "</td>                            
         <td>" . $row['name'] . "</td>
	     <td>" . $row['unit'] . "</td>
	     <td>" . $row['producer'] . "</td>
	     <td>" . $row['code'] . "</td>
	     <td>" . $row['count'] . "</td>
	     <td>" . substr($row['date'],0,10) . "</td>
	     <td>" . $row['ksp'] . "</td>";
//---------------------------------------------------->			 
	  if($authorization!=0)                                     // проверка пользователя
      {	
	    echo		                                            // открыть доступ к действиям
         "<td>
	       <div class='links'>";
?>
	         <a href='put.php?id=<?php echo $row['id']?>'>Оприходовать</a>  <!-- Ссылка на страницу 'Оприходовать' для наименования с id -->
             <a href='take.php?id=<?php echo $row['id']?>'>Расходовать</a>  <!-- Ссылка на страницу 'Расходовать' для наименования с id -->
<?php   echo 
	       "</div>
	     </td>";
	  }
//---------------------------------------------------->	  
	  echo
      "</tr>";
    }
    echo
    "</table>";  
  }
  else echo "<b><span style='color:red;'>По запросу ничего не найдено!</span></b>";
}

// Функция проверки состояния checkbox
function checkbox_verify($_name)
{
  // обязательно прописываем, чтобы функция всегда возвращала результат
  $result=0;

  // проверяем, а есть ли вообще такой checkbox на HTML форме, а то часто промахиваются
  if (isset($_REQUEST[$_name]))
  {
    if ($_REQUEST[$_name]=='on') {$result=1;}
  }
  
  return $result;
}
?>
<!---------------------------------------------------->
<!doctype html>
<html>
<head>
  <!-- Тег link - устанавливает связь с внешним документом вроде файла со стилями или со шрифтами. В отличие от тега <a>, тег <link> размещается всегда внутри контейнера <head> и не создает ссылку.
  Атрибут rel - Определяет отношения между текущим документом и файлом, на который делается ссылка.
  Атрибут href - путь к связываемому файлу.
  Атрибут type - MIME-тип данных подключаемого файла.
  MIME (Multipurpose Internet Mail Extension, Многоцелевые расширения почты Интернета) — спецификация для передачи по сети файлов различного типа: изображений, музыки, текстов, видео, архивов и др. Указание MIME-типа используется в HTML обычно при передаче данных форм и вставки на страницу различных объектов.-->
  <link rel="icon" href="img/favicon.ico" type="image/x-icon"><!-- Иконка сайта -->
  <link rel="stylesheet" href="css/style.css" type="text/css"/>
  <!-- Тег <meta> - определяет метатеги, которые используются для хранения информации предназначенной для браузеров и поисковых систем. Например, механизмы поисковых систем обращаются к метатегам для получения описания сайта, ключевых слов и других данных. Разрешается использовать более чем один метатег, все они размещаются в контейнере <head>. Как правило, атрибуты любого метатега сводятся к парам «имя=значение», которые определяются ключевыми словами content, name или http-equiv.
  Атрибут http-equiv - предназначен для конвертирования метатега в заголовок HTTP.
  Занчение "Content-Type" - тип кодировки документа.
  Атрибут content - устанавливает значение атрибута, заданного с помощью name или http-equiv. -->
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <script src="js/jquery-3.2.1.min.js"></script> 
  <title>Склад. Остаток. (Главная страница)</title>
</head>

<body>

<p><b>Склад. Остаток. (Главная страница)</b></p>

<div class="links">
<?php
if($authorization!=0)                                     // проверка пользователя
{	
  echo		                                              // открыть доступ к действиям
 "<a href='addName.php'>Добавить наименование</a>";
}
?>
  <a href="income.php">Приход</a> 
  <a href="expense.php">Расход</a> 
</div><br />

<form method="post" action="index.php">
<!-- Элемент <fieldset> предназначен для группирования элементов формы. Такая группировка облегчает работу с формами, содержащими большое число данных. Например, один блок может быть предназначен для ввода текстовой информации, а другой — для флажков.
Браузеры для повышения наглядности отображают результат использования тега <fieldset> в виде рамки. Ее вид зависит от операционной системы, а также используемого браузера. -->
  <fieldset id="block1" name="sortingGroup" style="border:0px solid grey; display:inline;">
    <legend><b>Выбор сортировки по полю:</b></legend>

      <div class="fieldgroup" id='master'>
	    <input id='idID' type="checkbox" <?=($checkboxes['idInd'] ? 'checked' : null); ?> name="idVal" onClick="checkTheSort(this)" />
		<label for='idID'> По умолчанию</label>
     </div>	
<!---------------------------------------------------->	 	
	<div class="fieldgroup">|</div>
		
	<div class="fieldgroup">
	  <input id="wID" type="checkbox" <?=($checkboxes['wInd'] ? 'checked' : null); ?> name="wVal" />
	  <label for='wID'> Приоритет наименования</label>
	</div>
	
    <div class="fieldgroup">|</div>
<!---------------------------------------------------->		
	<div class="fieldgroup" id='slaves'>
      <input id="nameID" type="checkbox" <?=($checkboxes['nameInd'] ? 'checked' : null); ?> name="nameVal" onClick="checkTheSort(this)" />
	  <label for='nameID'> Наименование</label>

      <input style="margin-left: 3em;" id="unitID" type="checkbox" <?=($checkboxes['unitInd'] ? 'checked' : null); ?> name="unitVal" onClick="checkTheSort(this)" />
	  <label for='unitID'> Единица измерения</label>
	
      <input style="margin-left: 3em;" id="producerID" type="checkbox" <?=($checkboxes['producerInd'] ? 'checked' : null); ?> name="producerVal" onClick="checkTheSort(this)" />
	  <label for='producerID'> Производитель</label>
	
      <input style="margin-left: 3em;" id="codeID" type="checkbox" <?=($checkboxes['codeInd'] ? 'checked' : null); ?> name="codeVal" onClick="checkTheSort(this)" />
	  <label for='codeID'> Код</label>
	
      <input style="margin-left: 3em;" id="dateID" type="checkbox" <?=($checkboxes['dateInd'] ? 'checked' : null); ?> name="dateVal" onClick="checkTheSort(this)" />
	  <label for='dateID'> Дата</label>
	</div>	
  </fieldset><br />
<!---------------------------------------------------->	   
  <fieldset id="block2" name="orderGroup" style="border:0px solid grey; display:inline;">
    <legend><b>Выбор порядка сортировки:</b></legend>

    <div class="fieldgroup" >
	
    <input type="checkbox" <?=($checkboxes['ascInd'] ? 'checked' : null); ?> name="ascVal" id="ascCB" onClick="checkTheBox(this)"/>
	<label for='ascCB'> Прямой</label>
	
    <input style="margin-left: 3em;" type="checkbox" <?=($checkboxes['descInd'] ? 'checked' : null); ?> name="descVal" id="descCB" onClick="checkTheBox(this)"/>
	<label for='descCB'> Обратный</label>
	
	</div>
	<div class="fieldgroup" >
	  <input type="submit" name="setButton" value="Отсортировать" />
	</div>
	
  </fieldset><br />
<!---------------------------------------------------->	
  <fieldset id="block3" name="searchGroup" style="border:0px solid grey; display:inline;">
    <label><b>Поиск полного совпадения по:</b></legend>
	<table>
	  <tr>
	    <td class="search" align="left"><label for='search1'> Наименованию:</label></td>
		<td class="search"><input style="margin-left: 3em;" type="text" name="name" id="search1" value="<?php ((isset($_POST["name"])?($_POST["name"]):(""))) ?>"></td>
		<td class="search"><input style="margin-left: 3em;" type="submit" name="showName" value="Показать"></td>
	  </tr>
	  <tr>
	    <td class="search" align="left"><label for='search2'> Коду: </label></td>
		<td class="search"><input style="margin-left: 3em;" type="text" name="code" id="search2" value="<?php ((isset($_POST["code"])?($_POST["code"]):(""))) ?>"></td>
		<td class="search"><input style="margin-left: 3em;" type="submit" name="showCode" value="Показать"></td>
	  </tr>
	  <tr>
	    <td class="search" align="left"><label for='search3'> Месту хранения: </label></td>
		<td class="search"><input style="margin-left: 3em;" type="text" name="ksp" id="search3" value="<?php ((isset($_POST["ksp"])?($_POST["ksp"]):(""))) ?>"></td>
		<td class="search"><input style="margin-left: 3em;" type="submit" name="showKsp" value="Показать"></td>
	  </tr>
	</table>
  </fieldset><br /><br />  
</form><br />
<!---------------------------------------------------->	
<script type="text/javascript" src="js/main.js"></script> 
<!---------------------------------------------------->	
<script>
// алгоритм установки чекбоксов при "самой первой" загрузке страницы
var b = '<?php echo $b;?>';

if(b=="true") 
{
  document.getElementById('idID').checked=true;
  document.getElementById('wID').checked=true;
  document.getElementById('ascCB').checked=true;
}
</script>
<!---------------------------------------------------->	
<?php
// выбор сортировки по полям:
if($checkboxes['idInd']==1) $sorting = "id";
else
{
//---------------------------------------------------->	
  if(checkbox_verify('wVal'))
  {
    if($checkboxes['nameInd']==1) $sorting = "name";  
    else                          $sorting = "";    
  }
  else
  {
    if($checkboxes['producerInd']==1) $sorting = "producer";    
    else                          $sorting = "";
  }
//---------------------------------------------------->	
  if($checkboxes['unitInd']==1)
  {
    if($sorting != "") $sorting .= ",unit";
	else               $sorting .= "unit";
  }
//---------------------------------------------------->	
  if(checkbox_verify('wVal'))
  {
    if($checkboxes['producerInd']==1) 
    {
      if($sorting != "") $sorting .= ",producer";
	  else               $sorting .= "producer";
    } 
  }
  else
  {
    if($checkboxes['nameInd']==1) 
    {
      if($sorting != "") $sorting .= ",name";
	  else               $sorting .= "name";
    } 
  }
//---------------------------------------------------->	
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
//---------------------------------------------------->	
if(isset($_POST['showName']))                                   // если кнопка 'showName' была нажата
{
  $name=$_POST['name'];                                         // что ищем?
  
  $query = " SELECT *
             FROM equips
			 WHERE name='$name'
			 ORDER BY " . $sorting. " " . $order;               // запрос к БД
			 
  tableBuild($query,$authorization);                            // построить таблицу с результатом запроса
}
else
if(isset($_POST['showCode']))                                   // если кнопка 'showCode' была нажата
{
  $code=$_POST['code'];                                         // что ищем?
  
  $query = " SELECT *
             FROM equips
			 WHERE code='$code'
			 ORDER BY " . $sorting. " " . $order;               // запрос к БД

  tableBuild($query,$authorization);                            // построить таблицу с результатом запроса
}
else
if(isset($_POST['showKsp']))                                    // если кнопка 'showKsp' была нажата
{
  $ksp=$_POST['ksp'];                                           // что ищем?
  
  $query = " SELECT *
             FROM equips
			 WHERE ksp='$ksp'
			 ORDER BY " . $sorting. " " . $order;               // запрос к БД

  tableBuild($query,$authorization);                            // построить таблицу с результатом запроса
}
else
if(isset($_POST['setButton']))                                  // если кнопка 'setButton' была нажата
{
  // запрос на выбор всех полей из таблицы equips, сортировать по $sorting в $order порядке
  $query = " SELECT * 
             FROM equips 
			 ORDER BY " . $sorting. " " . $order;  	
			 
  tableBuild($query,$authorization);                            // построить таблицу с результатом запроса
}
else
{
  $query = " SELECT *
             FROM equips
             ORDER BY id ASC ";                                 // запрос на выбор всех полей из таблицы equips, сортировать по id

  tableBuild($query,$authorization);                            // построить таблицу с результатом запроса
}
?>
</body>
</html>