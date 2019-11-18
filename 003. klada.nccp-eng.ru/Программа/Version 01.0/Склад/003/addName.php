<!doctype html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Добавление наименования</title>
</head>

<script> 
function question() // функция передачи ответа пользователя в скрытое поле с id="yesno"
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

if(isset($_POST['add']))                                       // если кнопка 'add' была нажата
{
  // при нажатии на нопку 'добавить' данные формы сохраняются в переменные:
  $name = strip_tags(trim($_POST['name']));                    // $_POST - глобальный массив, ввод в переменные с обработкой
  $unit = strip_tags(trim($_POST['unit']));
  $producer = strip_tags(trim($_POST['producer']));
  $code = strip_tags(trim($_POST['code']));
  $count = strip_tags(trim($_POST['count'])); 
  $ksp = strip_tags(trim($_POST['ksp'])); 
  $date = $_POST['date'];                                      // вводится автоматически
  $answer=$_POST['answer'];                                    // ответ пользователя
	
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
					");                                        // вставить все записи в таблицу equips
				
		echo "<b><span style='color:red;'>Наименование успешно добавлено!</span></b><br/><br/>";			
  
		mysql_query(" 
					INSERT INTO incomes(name,unit,producer,code,count,date)
					VALUES('$name','$unit','$producer','$code','$count','$date')
					");                                        // вставить все записи в таблицу incomes

		echo "<b><span style='color:red;'>Оприходовано единиц: " . $count . ".</span></b><br/><br/>"; 				

		mysql_close();                                         // закрыть соединение с бд	
	  }
      else
      {	
		echo "<b><span style='color:red;'>Добавление наименования отменено!</span></b><br/><br/>";
	  }
	}   
    else echo "<b><span style='color:red;'>Количество должно быть больше нуля!</span></b><br/><br/>"; 
  }
  else echo "<b><span style='color:red;'>Заполните все поля!</span></b><br/><br/>";  
}

?>

<form method="post" action="addName.php">

Наименование<br/>
<textarea cols="30" rows="5" name="name" ><?php echo $name; ?></textarea><br/><br/>

Единица измерения<br/>
<input type="text" name="unit" value = "<?php echo $unit; ?>" /><br/><br/>

Производитель<br/>
<input type="text" name="producer" value = "<?php echo $producer; ?>" /><br/><br/>

Код<br/>
<input type="text" name="code" value = "<?php echo $code; ?>" /><br/><br/>

Количество<br/>
<input type="text" name="count" value = "<?php echo $count; ?>" /><br/><br/>

Комната/шкаф/полка<br/>
<textarea cols="30" rows="5" name="ksp" ><?php echo $ksp; ?></textarea><br/><br/>

<!-- Дата -->
<input type="hidden" name="date" value=<?php echo date('Y-m-d'); ?> />

<!-- Кнопка 'Добавить' -->
<input id="adder" type="submit" name="add" value="Добавить" onClick="question()"/><br/><br/>

<!-- Ответ пользователя -->
<input id="yesno" type ="hidden" name="answer"> 

<a href="home.php">Вернуться на склад</a>

</form> 

</body>
</html>