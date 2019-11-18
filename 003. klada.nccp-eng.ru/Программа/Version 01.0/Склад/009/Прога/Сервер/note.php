<!doctype html>
<html>
<head>
  <!-- Тег link - устанавливает связь с внешним документом вроде файла со стилями или со шрифтами. В отличие от тега <a>, тег <link>   размещается всегда внутри контейнера <head> и не создает ссылку.
  Атрибут rel - Определяет отношения между текущим документом и файлом, на который делается ссылка.
  Атрибут href - путь к связываемому файлу.
  Атрибут type - MIME-тип данных подключаемого файла.
  MIME (Multipurpose Internet Mail Extension, Многоцелевые расширения почты Интернета) — спецификация для передачи по сети файлов различного типа: изображений, музыки, текстов, видео, архивов и др. Указание MIME-типа используется в HTML обычно при передаче данных форм и вставки на страницу различных объектов.-->
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <title>Добавить примечание</title>
</head>
<body>

<?php
include_once("db.php");                                      // однократное подключение к БД

global $note;                                                // глобальные переменные

$id = $_GET['id'];                                           // глобальный массив $_GET, из которого получаем id

$result = mysql_query(" SELECT * 
                        FROM expenses
                        WHERE id='$id'
					  ");                                    // запрос на выбор всех полей из таблицы expenses с id='$id'

$row = mysql_fetch_assoc($result);                           // записать таблицу expenses в переменную $row как массив

if(isset($_POST['saveNote']))                                // если кнопка 'saveNote' была нажата
{
  /*
    trim - удаляет пробелы из начала и конца строки
	strip_tags - удаляет HTML и PHP тэги из строки
  */
  // при нажатии на нопку 'добавить' данные формы сохраняются в переменные:
  $note = strip_tags(trim($_POST['note']));                  // $_POST - глобальный массив, ввод в переменные с обработкой
	
  if($note!=null)                                            // если поле 'примечание' не пустое 
  {
	// далее подлючаемся к бд:
	mysql_query(" 
				UPDATE expenses 
				SET note='$note'
				WHERE id='$id' 
				");  // запрос на обновление полей таблицы expenses, установить поле note с id='$id' значением переменной $note
				
    echo "<b><span style='color:red;'>Примечание добавлено!</span></b>"; 		
  }   
	
  mysql_close();                                             // закрыть соединение с бд
}
?>
<!-- Форма 
Метод post скрывает все передаваемые им переменные и их значения в своём теле.
Атрибут action указывает обработчик, к которому обращаются данные формы при их отправке на сервер.-->
<form method="post" action="note.php?id=<?php echo $id; ?>">

<br/>Примечание<br/>
<?php
if($note!=null)  // если примечание было изменено
{
?>
<input type="text" name="note" value="<?php echo $note; // записать в поле отредактированное значение ?>" /><br/><br/>
<?php
}
else
{
?>
<input type="text" name="note" value="<?php echo $row['note'] // записать в поле значение из БД ?>" /><br/><br/>  
<?php  
}
?>
<input type="submit" name="saveNote" value="Сохранить" /><br/><br/>

<a href="index.php">Вернуться на склад</a>

</form> 

</body>
</html>