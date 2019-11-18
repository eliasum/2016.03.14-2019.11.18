<!doctype html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Добавление наименования</title>
</head>
<body>

<form method="post" action="addName.php">

Наименование<br/>
<input type="text" name="name" /><br/>

Производитель<br/>
<textarea cols="40" rows="10" name="producer"></textarea><br/>

Код<br/>
<input type="text" name="code" /><br/>

Количество<br/>
<input type="text" name="count" /><br/><br/>

<input type="submit" name="add" value="Добавить" /><br/><br/>

<a href="home.php">Вернуться на склад</a>

</form> 

<?php
include_once("db.php");

// Поиск совпадения:
$result = mysql_query(" SELECT id,name,producer,code,count FROM equips
                        ORDER BY id DESC
					  ");                                    // выбрать все записи из таблицы equips, сортировать по id

$codeArr = array();                                          // массив кодов оборудования
$i=0;                                                        // индекс элемента массива кодов оборудования
echo "<br/>";

while($row = mysql_fetch_assoc($result))                     // mysql_fetch_array($result) || mysql_fetch_assoc($result)
{
   $codeArr[$i] = $row['code'];
   $countArr[$i] = $row['count'];
   $i++;
} 
////////////////////////

if(isset($_POST['add']))                                     // если кнопка 'add' была нажата
{
	// при нажатии на нопку 'добавить' данные формы сохраняются в переменные:
	$name = strip_tags(trim($_POST['name']));                // $_POST - глобальный массив, ввод в переменные с обработкой
	$producer = strip_tags(trim($_POST['producer']));
	$code = strip_tags(trim($_POST['code']));
	$count = strip_tags(trim($_POST['count'])); 
	
	function fQuery($name1,$producer1,$code1,$count1)        // функция mysql запросов
	{
	    if($count1>0)                                        // если количество больше нуля
	    { 
		  // далее подлючаемся к бд:
		  mysql_query(" 
					  INSERT INTO equips(name, producer, code, count) 
					  VALUES('$name1','$producer1','$code1','$count1')
					  ");                                    // вставить все записи в таблицу equips
				
		  echo "<br/>Наименование успешно добавлено!";			
				
		  mysql_query(" 
					  INSERT INTO incomes(name, producer, code, count) 
					  VALUES('$name1','$producer1','$code1','$count1')
					  ");                                    // вставить все записи в таблицу incomes

		  echo "<br/>Добавлено " . $count1 . " штук!"; 				

		  mysql_close();                                     // закрыть соединение с бд	
	    }   
        else echo "<br/>Количество должно быть больше нуля!"; 
	}

	if(count($codeArr)!=0)                                   // если уже были добавления наименований
	{
	  $check = false;
	
	  // Проверка совпадения:
      for($j=0; $j<count($codeArr); $j++)                    // цикл по массиву кодов оборудования
      {
	    if($codeArr[$j] == $code)                            // если коды совпадают
	    {
	      echo "<br/>Наименование " . $name . " c кодом " . $code . " уже существует!";
		  break;
	    }
	    else                                                 // иначе
	    {
	      $check = true;                                     // коды наименований не совпадают
	    } 
	  }
	
	  if($check)                                             // если коды наименований не совпадают
	  {
	    fQuery($name,$producer,$code,$count);	             // функция mysql запросов
	  }
	}
	else                                                     // если добавления наименований ещё не было
	{
      fQuery($name,$producer,$code,$count); 	             // функция mysql запросов
	}
}
?>
</body>
</html>