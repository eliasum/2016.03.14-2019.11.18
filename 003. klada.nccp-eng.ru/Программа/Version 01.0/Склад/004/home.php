<?php
ob_start();
/*
Эта функция включает буферизацию вывода. Если буферизация вывода активна, вывод скрипта не высылается (кроме заголовков), а сохраняется во внутреннем буфере.
bool ob_start ([ callable $output_callback = NULL [, int $chunk_size = 0 [, int $flags = PHP_OUTPUT_HANDLER_STDFLAGS ]]] )
*/

session_start(); // Стартуем сессию
 
$_SESSION['my_checkbox_checked1'] = (int) isset($_POST['my_checkbox1']); // Записываем в сессию состояние checkbox, если checked - записываем 1, если нет - 0

$_SESSION['my_checkbox_checked2'] = (int) isset($_POST['my_checkbox2']); // Записываем в сессию состояние checkbox, если checked - записываем 1, если нет - 0
 
$checkboxes = array(
    'my_checkbox1' => (isset($_SESSION['my_checkbox_checked1']) && $_SESSION['my_checkbox_checked1'] == 1 ? true : false), // Записуем в my_checkbox текущее состояние галочки, если есть сессия и она равна 1, то записуем true, если нет - false
	
    'my_checkbox2' => (isset($_SESSION['my_checkbox_checked2']) && $_SESSION['my_checkbox_checked2'] == 1 ? true : false) // Записуем в my_checkbox текущее состояние галочки, если есть сессия и она равна 1, то записуем true, если нет - false	
	
);
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
  <input type="checkbox" <?=($checkboxes['my_checkbox1'] ? 'checked' : null); ?> name="my_checkbox1" onClick="getElementById('form').submit()" /><br />
  <input type="checkbox" <?=($checkboxes['my_checkbox2'] ? 'checked' : null); ?> name="my_checkbox2" onClick="getElementById('form').submit()" /><br />  
<!-- Элемент <fieldset> предназначен для группирования элементов формы. Такая группировка облегчает работу с формами, содержащими большое число данных. Например, один блок может быть предназначен для ввода текстовой информации, а другой — для флажков.
Браузеры для повышения наглядности отображают результат использования тега <fieldset> в виде рамки. Ее вид зависит от операционной системы, а также используемого браузера. -->
  <fieldset name="sortingGroup" style="border:1px solid grey; display:inline;">
    <legend>Сортировать по полю:</legend>
    <div class="fieldgroup"><input type="radio" name="sortingRB" value="idVal" checked ="true" /> № п/п</div>
    <div class="fieldgroup"><input type="radio" name="sortingRB" value="nameVal" /> Наименование</div>
    <div class="fieldgroup"><input type="radio" name="sortingRB" value="unitVal" /> Единица измерения</div>
    <div class="fieldgroup"><input type="radio" name="sortingRB" value="producerVal" /> Производитель</div>
    <div class="fieldgroup"><input type="radio" name="sortingRB" value="codeVal" /> Код</div>
    <div class="fieldgroup"><input type="radio" name="sortingRB" value="dateVal" /> Дата</div>
  </fieldset><br />
  <fieldset name="orderGroup" style="border:1px solid grey; display:inline;">
    <legend>Порядкок сортировки:</legend>
    <div class="fieldgroup"><input type="radio" name="orderRB" value="ascVal" checked ="true" /> Прямой</div>
    <div class="fieldgroup"><input type="radio" name="orderRB" value="descVal" /> Обратный</div>
  </fieldset><br /><br />
  <input type="submit" name="setButton"/>
</form><br />

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
global $order, $sorting;

if(isset($_POST['setButton']))                                  // если кнопка 'setButton' была нажата
{
  switch($_POST['orderRB'])                                     // варианты радиокнопок orderRB
  {
     case 'ascVal':
       $order = "ASC";
       break;
     case 'descVal':
       $order = "DESC";
       break;
  }
  
  switch($_POST['sortingRB'])                                   // варианты радиокнопок sortingRB
  {
     case 'idVal':
       $sorting = "id";
       break;
     case 'nameVal':
       $sorting = "name";
       break;
     case 'unitVal':
       $sorting = "unit";
       break;
     case 'producerVal':
       $sorting = "producer";
       break;
     case 'codeVal':
       $sorting = "code";
       break;
     case 'dateVal':
       $sorting = "date";
       break;	   
  }
  
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
</body>
</html>