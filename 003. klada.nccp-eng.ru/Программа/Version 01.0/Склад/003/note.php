<!doctype html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Добавить примечание</title>
</head>
<body>

<?php
include_once("db.php");

global $note;                                                // глобальные переменные

$id = $_GET['id'];                                           // глобальный массив $_GET, из которого получаем id

$result = mysql_query(" SELECT * 
                        FROM expenses
                        WHERE id='$id'
					  ");                                    // запрос на выбор note с id='$id'

$row = mysql_fetch_assoc($result);                           // записать note с id='$id' в переменную $row

if(isset($_POST['saveNote']))                                // если кнопка 'saveNote' была нажата
{
  // при нажатии на нопку 'добавить' данные формы сохраняются в переменные:
  $note = strip_tags(trim($_POST['note']));                  // $_POST - глобальный массив, ввод в переменные с обработкой
	
  if($note!=null)
  {
	// далее подлючаемся к бд:
	mysql_query(" 
				UPDATE expenses 
				SET note='$note'
				WHERE id='$id' 
				");                                          // запрос на обновление count с id='$id'
				
    echo "<b><span style='color:red;'>Примечание добавлено!</span></b>"; 		
  }   
	
  mysql_close();                                             // закрыть соединение с бд
}
?>

<form method="post" action="note.php?id=<?php echo $id; ?>">

<br/>Примечание<br/>
<?php
if($note!=null)  // если примечание было изменено
{
?>
<input type="text" name="note" value="<?php echo $note; ?>" /><br/><br/>
<?php
}
else
{
?>
<input type="text" name="note" value="<?php echo $row['note'] ?>" /><br/><br/>  
<?php  
}
?>
<input type="submit" name="saveNote" value="Сохранить" /><br/><br/>

<a href="home.php">Вернуться на склад</a>

</form> 

</body>
</html>