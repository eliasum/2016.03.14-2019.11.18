<!doctype html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Склад. Расход.</title>
</head>
<body>

<p><b>Склад. Расход.</b></p>

<a href="home.php">Вернуться на склад</a>

<hr />

<?php
include_once("db.php");

$result = mysql_query(" SELECT id,name,producer,code,count FROM expenses
                        ORDER BY id DESC
					  ");                                       // выбрать все записи из таблицы equips, сортировать по id в обрат порядке

mysql_close();                                                  // закрыть соединение с бд

//$row = mysql_fetch_array($result);                            // записать таблицу equips в переменную $row

while($row = mysql_fetch_array($result))                        // mysql_fetch_array($result) || mysql_fetch_assoc($result)
{
?>
<p><b>Наименование: </b><?php echo $row['name'];?></p>
<p><b>Производитель: </b><?php echo $row['producer'];?></p>
<p><b>Код: </b><?php echo $row['code'];?></p>
<p><b>Количество: </b><?php echo $row['count'];?></p>

<hr />
<?php } ?>
</body>
</html>