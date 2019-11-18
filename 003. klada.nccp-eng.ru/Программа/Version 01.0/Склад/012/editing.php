<!doctype html>
<html>
<head>
  <!-- Тег link - устанавливает связь с внешним документом вроде файла со стилями или со шрифтами. В отличие от тега <a>, тег <link> размещается всегда внутри контейнера <head> и не создает ссылку.
  Атрибут rel - Определяет отношения между текущим документом и файлом, на который делается ссылка.
  Атрибут href - путь к связываемому файлу.
  Атрибут type - MIME-тип данных подключаемого файла.
  MIME (Multipurpose Internet Mail Extension, Многоцелевые расширения почты Интернета) — спецификация для передачи по сети файлов различного типа: изображений, музыки, текстов, видео, архивов и др. Указание MIME-типа используется в HTML обычно при передаче данных форм и вставки на страницу различных объектов.-->
  <link rel="icon" href="img/favicon.ico" type="image/x-icon"><!-- Иконка сайта -->
  <!-- Тег <meta> - определяет метатеги, которые используются для хранения информации предназначенной для браузеров и поисковых систем. Например, механизмы поисковых систем обращаются к метатегам для получения описания сайта, ключевых слов и других данных. Разрешается использовать более чем один метатег, все они размещаются в контейнере <head>. Как правило, атрибуты любого метатега сводятся к парам «имя=значение», которые определяются ключевыми словами content, name или http-equiv.
  Атрибут http-equiv - предназначен для конвертирования метатега в заголовок HTTP.
  Занчение "Content-Type" - тип кодировки документа.
  Атрибут content - устанавливает значение атрибута, заданного с помощью name или http-equiv. -->
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <title>Редактирование наименования</title>
</head>

<script> 
function question()                              // функция передачи ответа пользователя в скрытое поле с id="yesno"
{ 
  if(confirm("Вы уверены, что хотите утвердить изменения наименование?")) 
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

$id = $_GET['id'];                                             // глобальный массив $_GET, из которого получаем id

global $name,$unit,$producer,$code,$count,$ksp,$row;           // глобальные переменные  

$result = mysql_query(" SELECT * 
                        FROM `equips`
                        WHERE `id`='$id'
					  ");                                      // запрос на выбор всех полей из таблицы equips с id='$id'

$row = mysql_fetch_assoc($result);                             // записать таблицу equips в переменную $row как массив        

if(isset($_POST['editButton']))                                // если кнопка 'editButton' была нажата
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
				UPDATE `equips` 
				SET `name`='$name',`unit`='$unit',`producer`='$producer',`code`='$code',`count`='$count',`ksp`='$ksp',`date`='$date'
				WHERE `id`='$id' 
				") or die(mysql_error());  // запрос на обновление полей таблицы equips, установить поля с id='$id' соответствующими значениями $noteVal				
				
		echo "<b><span style='color:red;'>Наименование изменено!</span></b><br/><br/>"; 			
	  }
	  // если пользователь не подтвердил ввод
      else echo "<b><span style='color:red;'>Изменение наименования отменено!</span></b><br/><br/>";
	}   
	// если количество не больше нуля
    else echo "<b><span style='color:red;'>Количество должно быть больше нуля!</span></b><br/><br/>"; 
  }
  // если не все поля заполнены
  else echo "<b><span style='color:red;'>Заполните все поля!</span></b><br/><br/>";  
}

// для вывода пользователю актуальных данных:
$result = mysql_query(" SELECT * 
                        FROM `equips`
                        WHERE `id`='$id'
					  ");                                      // запрос на выбор всех полей из таблицы equips с id='$id'

$row = mysql_fetch_assoc($result);                             // записать таблицу equips в переменную $row как массив

mysql_close();                                                 // закрыть соединение с бд	
?>

<!-- Форма 
Метод post скрывает все передаваемые им переменные и их значения в своём теле.
Атрибут action указывает обработчик, к которому обращаются данные формы при их отправке на сервер.-->
<form method="post" action="editing.php?id=<?php echo $id; ?>">

Наименование<br/>
<textarea cols="30" rows="5" name="nameField" ><?php echo $row['name']; ?></textarea><br/><br/>

Единица измерения<br/>
<input type="text" name="unitField" value = "<?php echo $row['unit']; ?>" /><br/><br/>

Производитель<br/>
<input type="text" name="producerField" value = "<?php echo $row['producer']; ?>" /><br/><br/>

Код<br/>
<input type="text" name="codeField" value = "<?php echo $row['code']; ?>" /><br/><br/>

Количество<br/>
<input type="text" name="countField" value = "<?php echo $row['count']; ?>" /><br/><br/>

Комната/шкаф/полка<br/>
<textarea cols="30" rows="5" name="kspField" ><?php echo $row['ksp']; ?></textarea><br/><br/>

Дата<br/>
<input type="text" name="dateField" value=<?php echo $row['date']; ?> /><br/><br/>

<!-- Кнопка 'Редактировать', по нажатию на кнопку вызов функции question() -->
<input id="adder" type="submit" name="editButton" value="Редактировать" onClick="question()"/><br/><br/>

<!-- Поле, принимающее ответ пользователя, поле скрыто от пользователя -->
<input id="yesno" type ="hidden" name="answerField"> 

<a href="index.php">Вернуться на начальную страницу</a>

</form> 

</body>
</html>