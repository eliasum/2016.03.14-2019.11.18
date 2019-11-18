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
    document.getElementById('yesno').value = 1; 
  } 
  else 
  { 
    document.getElementById('yesno').value = 0; 
  } 
} 
</script> 

<body>

<?php
include_once("db.php");

global $name,$producer,$code,$count;                           // глобальные переменные                   

// Поиск совпадения:
$result = mysql_query(" SELECT id,name,producer,code,count,date FROM equips
                        ORDER BY id DESC
					  ");                                      // выбрать все записи из таблицы equips, сортировать по id

$codeArr = array();                                            // массив кодов оборудования
$i=0;                                                          // индекс элемента массива кодов оборудования
echo "<br/>";

while($row = mysql_fetch_assoc($result))                       // mysql_fetch_array($result) || mysql_fetch_assoc($result)
{
  $codeArr[$i] = $row['code'];
  $countArr[$i] = $row['count'];
  $i++;
} 
////////////////////////	

if(isset($_POST['add']))                                       // если кнопка 'add' была нажата
{
  // при нажатии на нопку 'добавить' данные формы сохраняются в переменные:
  $name = strip_tags(trim($_POST['name']));                    // $_POST - глобальный массив, ввод в переменные с обработкой
  $producer = strip_tags(trim($_POST['producer']));
  $code = strip_tags(trim($_POST['code']));
  $count = strip_tags(trim($_POST['count'])); 
  $date = $_POST['date'];                                      // вводится автоматически
  $answer=$_POST['answer'];                                    // ответ пользователя
		
  if(($name!=null)&&($producer!=null)&&
     ($code!=null)&&($count!=null))                            // если все поля заполнены
  {     
    function fQuery(
	$name1,$producer1,$code1,
	$count1,$date1,$answer1)                                   // функция mysql запросов
	{
	  if($count1>0)                                            // если количество больше нуля
	  { 	  
		if($answer1==1)                                        // если пользователь подтвердил ввод
	    {
		  // далее подлючаемся к бд:
		  mysql_query(" 
			  		  INSERT INTO equips(name, producer, code, count,date) 
					  VALUES('$name1','$producer1','$code1','$count1','$date1')
					  ");                                      // вставить все записи в таблицу equips
				
		  echo "<b><span style='color:red;'>Наименование успешно добавлено!</span></b><br/><br/>";			

		  mysql_query(" 
					  INSERT INTO incomes(name, producer, code, count,date) 
					  VALUES('$name1','$producer1','$code1','$count1','$date1')
					  ");                                      // вставить все записи в таблицу incomes

		  echo "<b><span style='color:red;'>Оприходовано единиц: " . $count1 . ".</span></b><br/><br/>"; 				

		  mysql_close();                                       // закрыть соединение с бд	
	    }
		else
        {	
		  echo "<b><span style='color:red;'>Добавление наименования отменено!</span></b><br/><br/>";
		}
	  }   
      else echo "<b><span style='color:red;'>Количество должно быть больше нуля!</span></b><br/><br/>"; 
	}

	if(count($codeArr)!=0)                                     // если уже были добавления наименований
	{
	  $check = false;
	
	  // Проверка совпадения:
      for($j=0; $j<count($codeArr); $j++)                      // цикл по массиву кодов оборудования
      {
	    if($codeArr[$j] == $code)                              // если коды совпадают
	    {
	      echo "<b><span style='color:red;'>Наименование " . $name . " c кодом " . $code . " уже существует!</span></b><br/><br/>";
		  break;
	    }
	    else                                                   // иначе
	    {
	      $check = true;                                       // коды наименований не совпадают
	    } 
	  }
	
	  if($check)                                               // если коды наименований не совпадают
	  {
	    fQuery($name,$producer,$code,$count,$date,$answer);	   // функция mysql запросов
	  }
	}
	else                                                       // если добавления наименований ещё не было
	{
      fQuery($name,$producer,$code,$count,$date,$answer);      // функция mysql запросов
	}
  }
  else echo "<b><span style='color:red;'>Заполните все поля!</span></b><br/><br/>";  
}

?>

<form method="post" action="addName.php">

Наименование<br/>
<textarea cols="30" rows="5" name="name" ><?php echo $name; ?></textarea><br/><br/>

Производитель<br/>
<input type="text" name="producer" value = "<?php echo $producer; ?>" /><br/><br/>

Код<br/>
<input type="text" name="code" value = "<?php echo $code; ?>" /><br/><br/>

Количество<br/>
<input type="text" name="count" value = "<?php echo $count; ?>" /><br/><br/>

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