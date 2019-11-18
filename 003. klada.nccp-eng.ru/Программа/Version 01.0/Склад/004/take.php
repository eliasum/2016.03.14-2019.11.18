<!doctype html>
<html>
<head>
    <!-- Тег <meta> - определяет метатеги, которые используются для хранения информации предназначенной для браузеров и поисковых систем. Например, механизмы поисковых систем обращаются к метатегам для получения описания сайта, ключевых слов и других данных. Разрешается использовать более чем один метатег, все они размещаются в контейнере <head>. Как правило, атрибуты любого метатега сводятся к парам «имя=значение», которые определяются ключевыми словами content, name или http-equiv.
	Атрибут http-equiv - предназначен для конвертирования метатега в заголовок HTTP.
	Занчение "Content-Type" - тип кодировки документа.
	Атрибут content - устанавливает значение атрибута, заданного с помощью name или http-equiv. -->
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Взять оборудование</title>
</head>
<body>

<?php
include_once("db.php");                                      // однократное подключение к БД

$id = $_GET['id'];                                           // глобальный массив $_GET, из которого получаем id

$result = mysql_query(" SELECT * 
                        FROM equips
                        WHERE id='$id'
					  ");                                    // запрос на выбор всех полей из таблицы equips с id='$id'

$row = mysql_fetch_assoc($result);                           // записать таблицу equips в переменную $row как массив

if(isset($_POST['saveName']))                                // если кнопка 'saveName' была нажата
{
  /*
    trim - удаляет пробелы из начала и конца строки
	strip_tags - удаляет HTML и PHP тэги из строки
  */
  // при нажатии на нопку 'добавить' данные формы сохраняются в переменные:
  $count = strip_tags(trim($_POST['countField']));           // $_POST - глобальный массив, ввод в переменные из полей с обработкой
	
  if(($count>0)&&($count<=$row['count']))                    // если количество больше нуля и меньше или равно количеству в БД
  {
	$temp = $count;                                          // сохранить $count
	
	$row['count']-=$count;                                   // вычесть из предыдущего значения
	$count = $row['count'];                                  // записать разницу в переменную
    
	// далее подлючаемся к бд:
	mysql_query(" 
				UPDATE equips 
				SET count='$count'
				WHERE id='$id' 
				");  // запрос на обновление полей таблицы equips, установить поле count с id='$id' значением переменной $count

	echo "<b><span style='color:red;'>Израсходовано единиц: " . $temp . ".</span></b>"; 
	
    mysql_query(" 
				INSERT INTO expenses(name,unit,producer,code,count,date) 
				VALUES('$row[name]','$row[unit]','$row[producer]','$row[code]','$temp','$row[date]')
				");  // запрос на вставку в поля таблицы expenses соответствующих значений из переменных	
  }   
  else echo "<br/><b><span style='color:red;'>Количество должно быть больше нуля,
            но меньше количества оборудования на складе!</span></b>"; 	
	
  mysql_close();                                             // закрыть соединение с бд
}
?>
<!-- Форма 
Метод post скрывает все передаваемые им переменные и их значения в своём теле.
Атрибут action указывает обработчик, к которому обращаются данные формы при их отправке на сервер.-->
<form method="post" action="take.php?id=<?php echo $id; ?>">

<br/>Количество<br/>
<input type="text" name="countField" value="0" /><br/><br/>

<input type="submit" name="saveName" value="Сохранить" /><br/><br/>

<a href="home.php">Вернуться на склад</a>

</form> 

</body>
</html>