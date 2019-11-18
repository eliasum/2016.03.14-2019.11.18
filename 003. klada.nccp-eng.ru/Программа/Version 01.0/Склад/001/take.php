<!doctype html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Взять оборудование</title>
</head>
<body>

<?php
include_once("db.php");

$id = $_GET['id'];                                           // глобальный массив $_GET, из которого получаем id

$result = mysql_query(" SELECT id,name,producer,code,count FROM equips
                        WHERE id='$id'
					  ");                                    // запрос на выбор count с id='$id'

$row = mysql_fetch_assoc($result);                           // записать count с id='$id' в переменную $row

if(isset($_POST['saveName']))                                // если кнопка 'saveName' была нажата
{
	// при нажатии на нопку 'добавить' данные формы сохраняются в переменные:
	$count = strip_tags(trim($_POST['count']));              // $_POST - глобальный массив, ввод в переменные с обработкой
	
	if(($count>0)&&($count<=$row['count']))
	{
	$temp = $count;                                          // сохранить $count
	
	$row['count']-=$count;
	$count = $row['count'];
    
	// далее подлючаемся к бд:
	mysql_query(" 
				UPDATE equips SET count='$count'
				WHERE id='$id' 
				");                                          // запрос на обновление count с id='$id'

	echo "Взято " . $temp . " штук!"; 
	
    mysql_query(" 
				INSERT INTO expenses(name, producer, code, count) 
				VALUES('$row[name]','$row[producer]','$row[code]','$temp')
				");                                          // вставить все записи в таблицу incomes	
    }   
    else echo "Количество должно быть больше нуля но меньше количества оборудования на складе!"; 	
	
	mysql_close();                                           // закрыть соединение с бд
}
?>

<form method="post" action="take.php?id=<?php echo $id; ?>">

<br/>Количество<br/>
<input type="text" name="count" value="0" /><br/><br/>

<input type="submit" name="saveName" value="Сохранить" /><br/><br/>

<a href="home.php">Вернуться на склад</a>

</form> 

</body>
</html>