<!doctype html>
<html>
<head>
    <!-- Тег <meta> - определяет метатеги, которые используются для хранения информации предназначенной для браузеров и поисковых систем. Например, механизмы поисковых систем обращаются к метатегам для получения описания сайта, ключевых слов и других данных. Разрешается использовать более чем один метатег, все они размещаются в контейнере <head>. Как правило, атрибуты любого метатега сводятся к парам «имя=значение», которые определяются ключевыми словами content, name или http-equiv.
	Атрибут http-equiv - предназначен для конвертирования метатега в заголовок HTTP.
	Занчение "Content-Type" - тип кодировки документа.
	Атрибут content - устанавливает значение атрибута, заданного с помощью name или http-equiv. -->
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Добавление наименования</title>
</head>

<script> 
function question()                              // функция передачи ответа пользователя в скрытое поле с id="yesno"
{ 
  if(confirm("Вы уверены, что хотите добавить новое наименование? Его нельзя будет удалить!")) 
  { 
    document.getElementById('yesno').value = 1;  // поиск элемента документа по id и запись в него значения 1
  } 
  else 
  { 
    document.getElementById('yesno').value = 0;  // поиск элемента документа по id и запись в него значения 0
  } 
} 
</script> 

<body>

<?php
include_once("db.php");                                        // однократное подключение к БД

global $name,$unit,$producer,$code,$count,$ksp;                // глобальные переменные                   

if(isset($_POST['addButton']))                                 // если кнопка 'addButton' была нажата
{
  // при нажатии на кнопку 'добавить' данные формы сохраняются в переменные:
  /*
    trim - удаляет пробелы из начала и конца строки
	strip_tags - удаляет HTML и PHP тэги из строки
  */
  $name = strip_tags(trim($_POST['nameField']));               // $_POST - глобальный массив, ввод в переменные из полей с обработкой
  $unit = strip_tags(trim($_POST['unitField']));
  $producer = strip_tags(trim($_POST['producerField']));
  $code = strip_tags(trim($_POST['codeField']));
  $count = strip_tags(trim($_POST['countField'])); 
  $ksp = strip_tags(trim($_POST['kspField'])); 
  $date = $_POST['dateField'];                                 // дата вводится автоматически
  $answer = $_POST['answerField'];                             // ответ пользователя
	
  if(($name!=null)&&($unit!=null)&&
     ($producer!=null)&&($code!=null)&&
	 ($count!=null)&&($ksp!=null))                             // если все поля заполнены
  {     
    if($count>0)                                               // если количество больше нуля
    { 	  
      if($answer==1)                                           // если пользователь подтвердил ввод
	  {
		// далее подлючаемся к бд:
		mysql_query(" 
			  		INSERT INTO equips(name,unit,producer,code,count,date,ksp) 
					VALUES('$name','$unit','$producer','$code','$count','$date','$ksp')
					");  // запрос на вставку в поля таблицы equips соответствующих значений из переменных
				
		echo "<b><span style='color:red;'>Наименование успешно добавлено!</span></b><br/><br/>";			
  
		mysql_query(" 
					INSERT INTO incomes(name,unit,producer,code,count,date)
					VALUES('$name','$unit','$producer','$code','$count','$date')
					");  // запрос на вставку в поля таблицы incomes соответствующих значений из переменных

		echo "<b><span style='color:red;'>Оприходовано единиц: " . $count . ".</span></b><br/><br/>"; 				

		mysql_close();                                         // закрыть соединение с бд	
	  }
	  // если пользователь не подтвердил ввод
      else echo "<b><span style='color:red;'>Добавление наименования отменено!</span></b><br/><br/>";
	}   
	// если количество не больше нуля
    else echo "<b><span style='color:red;'>Количество должно быть больше нуля!</span></b><br/><br/>"; 
  }
  // если не все поля заполнены
  else echo "<b><span style='color:red;'>Заполните все поля!</span></b><br/><br/>";  
}

?>

<!-- Форма 
Метод post скрывает все передаваемые им переменные и их значения в своём теле.
Атрибут action указывает обработчик, к которому обращаются данные формы при их отправке на сервер.-->
<form method="post" action="addName.php">

Наименование<br/>
<textarea cols="30" rows="5" name="nameField" ><?php echo $name; ?></textarea><br/><br/>

Единица измерения<br/>
<input type="text" name="unitField" value = "<?php echo $unit; ?>" /><br/><br/>

Производитель<br/>
<input type="text" name="producerField" value = "<?php echo $producer; ?>" /><br/><br/>

Код<br/>
<input type="text" name="codeField" value = "<?php echo $code; ?>" /><br/><br/>

Количество<br/>
<input type="text" name="countField" value = "<?php echo $count; ?>" /><br/><br/>

Комната/шкаф/полка<br/>
<textarea cols="30" rows="5" name="kspField" ><?php echo $ksp; ?></textarea><br/><br/>

<!-- Дата, поле скрыто от пользователя -->
<input type="hidden" name="dateField" value=<?php echo date('Y-m-d'); ?> />

<!-- Кнопка 'Добавить', по нажатию на кнопку вызов функции question() -->
<input id="adder" type="submit" name="addButton" value="Добавить" onClick="question()"/><br/><br/>

<!-- Поле, принимающее ответ пользователя, поле скрыто от пользователя -->
<input id="yesno" type ="hidden" name="answerField"> 

<a href="index.php">Вернуться на склад</a>

</form> 

</body>
</html>