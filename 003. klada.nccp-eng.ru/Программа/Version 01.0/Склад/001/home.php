<!doctype html>
<html>
<head>
    <link rel="stylesheet" href="style.css" type="text/css"/>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Склад. Остаток.</title>
</head>
<body>

<p><b>Склад. Остаток.</b></p>

<div class="links">
    <a href="addName.php">Добавить наименование</a>
    <a href="income.php">Приход</a> 
    <a href="expense.php">Расход</a> 
</div>

<hr />

<?php
include_once("db.php");

$result = mysql_query(" SELECT id,name,producer,code,count FROM equips
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

<a href="put.php?id=<?php echo $row['id']?>">Добавить оборудование</a><br />
<a href="take.php?id=<?php echo $row['id']?>">Взять оборудование</a><br /><br />

<hr />
<?php } ?>
</body>
</html>