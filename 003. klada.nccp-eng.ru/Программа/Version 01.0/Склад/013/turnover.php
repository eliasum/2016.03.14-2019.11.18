<!doctype html>
<html>
<head>
  <!-- Тег link - устанавливает связь с внешним документом вроде файла со стилями или со шрифтами. В отличие от тега <a>, тег <link>   размещается всегда внутри контейнера <head> и не создает ссылку.
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
  <title>Кладовка КБА. Оборотная ведомость</title>
</head>
<body>

<p><b>Кладовка КБА. Оборотная ведомость</b></p>

<a href="index.php">Вернуться на начальную страницу</a>

<br />

<hr />

<?php
function tableBuild($query)                                     // функция построения таблицы ответа на запрос
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
         <th class='row11'><b>№ п/п</b></th>
         <th class='row12'><b>Наименование</b></th>
         <th class='row13'><b>Единица измерения</b></th>
	     <th class='row14'><b>Остаток начальный</b></th>
	     <th class='row15'><b>Приход</b></th>
	     <th class='row16'><b>Расход</b></th>
         <th class='row17'><b>Остаток конечный</b></th>
       </tr>";                                                  // шапка таблицы
	   	
    while($row = mysql_fetch_array($result))                    // mysql_fetch_array($result) || mysql_fetch_assoc($result)
    {
      echo
      "<tr>
         <td>" . $row['id'] . "</td>
         <td>" . $row['name'] . "</td>
	     <td>" . $row['unit'] . "</td>
	     <td>" . $row['initial'] . "</td>
	     <td>" . $row['income'] . "</td>
	     <td>" . $row['expence'] . "</td>
	     <td>" . $row['final'] . "</td>
       </tr>";
    }
    echo
    "</table>";  
  }
  else echo "<b><span style='color:red;'>По запросу ничего не найдено!</span></b>";
}

//-------------Фильтрация по дате----------------
include_once("db.php");  // однократное подключение к БД

$month = array("Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь");
  
echo '<form method="POST">';
 
echo '<p>Фильтр по дате.<p>';
 
echo '<table cellspacing="0" cellpadding="0" border="0">';
echo '<tr>';
echo '<td>Начало:</td>';
echo '<td>';
/*
Тег <select> позволяет создать элемент интерфейса в виде раскрывающегося списка, а также список с одним или множественным выбором. Конечный вид зависит от использования атрибута size тега <select>, который устанавливает высоту списка. Ширина списка определяется самым широким текстом, указанным в теге <option>, а также может изменяться с помощью стилей. Каждый пункт создается с помощью тега <option>, который должен быть вложен в контейнер <select>. Если планируется отправлять данные списка на сервер, то требуется поместить элемент <select> внутрь формы. Это также необходимо, когда к данным списка идет обращение через скрипты.
*/
echo '<select name="start_day">';
 
for($i = 1; $i <= 31; $i++)
{
/*
    Тег <option> определяет отдельные пункты списка, создаваемого с помощью контейнера <select>. Ширина списка определяется самым широким текстом, указанным в теге <option>, а также может изменяться с помощью стилей. Если планируется отправлять данные списка на сервер, то требуется поместить элемент <select> внутрь формы. Это также необходимо, когда к данным списка идет обращение через скрипты.
*/
    echo '<option value="'.$i.'"'.((isset($_POST["start_day"]) && $_POST["start_day"] == $i)?(" selected"):("")).'>'.(($i < 10)?("0".$i):($i)).'</option>';
}
 
echo '</select>';
echo '</td>';
echo '<td>';
echo '<select name="start_month">';
 
for($i = 0; $i < count($month); $i++)
{
    echo '<option value="'.($i + 1).'"'.((isset($_POST["start_month"]) && $_POST["start_month"] == ($i+1))?(" selected"):("")).'>'.$month[$i].'</option>';
}
 
echo '</select>';
echo '</td>';
echo '<td>';
echo '<select name="start_year">';
 
for($i = date("Y"); $i >= 1901; $i--)
{
    echo '<option value="'.$i.'"'.((isset($_POST["start_year"]) && $_POST["start_year"] == $i)?(" selected"):("")).'>'.$i.'</option>';
}
 
echo '</select>';
echo '</td>';
echo '</tr>';
 
echo '<tr>';
echo '<td>Конец:</td>';
echo '<td>';
echo '<select name="end_day">';
 
for($i = 1; $i <= 31; $i++)
{
    echo '<option value="'.$i.'"'.((isset($_POST["end_day"]) && $_POST["end_day"] == $i)?(" selected"):("")).'>'.(($i < 10)?("0".$i):($i)).'</option>';
}
 
echo '</select>';
echo '</td>';
echo '<td>';
echo '<select name="end_month">';
 
for($i = 0; $i < count($month); $i++)
{
    echo '<option value="'.($i + 1).'"'.((isset($_POST["end_month"]) && $_POST["end_month"] == ($i+1))?(" selected"):("")).'>'.$month[$i].'</option>';
}
 
echo '</select>';
echo '</td>';
echo '<td>';
echo '<select name="end_year">';
 
for($i = date("Y"); $i >= 1901; $i--)
{
    echo '<option value="'.$i.'"'.((isset($_POST["end_year"]) && $_POST["end_year"] == $i)?(" selected"):("")).'>'.$i.'</option>';
}
 
echo '</select>';
echo '</td>';
echo '</tr>';
echo '</table><br />';
 
echo '<input type="submit" value="Фильтровать" name="filter" />';
echo '</form><br />';

//-------------------Поиск-------------------------
if(isset($_POST['filter']))  // если кнопка 'filter' была нажата
{
/*
  mktime - возвращает метку времени для заданной даты
  int mktime ( [int hour [, int minute [, int second [, int month [, int day [, int year [, int is_dst]]]]]]] )
*/
  $begin = mktime(0, 0, 0, $_POST["start_month"], $_POST["start_day"], $_POST["start_year"]);
  $end = mktime(23, 59, 59, $_POST["end_month"], $_POST["end_day"], $_POST["end_year"]);
	
  // запрос на выбор всех полей из таблицы incomes со значениями поля date от $begin до $end, сортировать по id
  $query = "SELECT *
            FROM `incomes` 
		    WHERE `date`>='".date("Y-m-d H:i:s", $begin)."' 
		    AND `date`<='".date("Y-m-d H:i:s", $end)."' 
		    ORDER BY `id` ASC";  
			
  tableBuild($query);                                               // построить таблицу с результатом запроса
}
//-------------------По умолчанию-------------------------
else
{                                                                   // шапка таблицы
  // запрос на выбор всех полей из таблицы incomes, сортировать по id
  $query = "SELECT * 
            FROM `equips`
            ORDER BY `id` ASC";  

  include_once("db.php");                                       // однократное подключение к БД	
  
  $result = mysql_query($query) or die ("<b><span style='color:red;'>Запрос ошибочный!</span></b>");   // результат запроса
  
  mysql_close();                                                // закрыть соединение с бд
 
  //$row = mysql_fetch_array($result);                          // записать результат запроса в переменную $row как массив
  
  $names = Array();
  
  if(mysql_num_rows($result) > 0)                               // если количество рядов результата запроса больше нуля
  {
    while($row = mysql_fetch_array($result))                    // mysql_fetch_array($result) || mysql_fetch_assoc($result)
    {
	  $names[] = $row['name'];
	  $counts[] = $row['count'];
	  $dates[] = $row['date'];
    }
/*	
	for($i=0; $i<count($names); $i++)
	{
	  if($names[$i]==$names[$i-1])
	  {
	    $counts[$i]+=$counts[$i-1];
		unset($names[$i-1])
	  }
	}	
	
	for($i=0; $i<count($names); $i++)
	{
	  echo $names[$i] . " " . $counts[$i] . " " . $dates[$i] . "<br />";
	}*/
  }

/*
$a=array(5=>"Назв1",3=>"Назв2",1=>"Назв1");
 
foreach($a as $k=>$v) $b[$v]+=$k;
 
$a=array_flip($b);
 
print_r($a);
*/




			
//  tableBuild($query);                                               // построить таблицу с результатом запроса
}
?>

</body>
</html>