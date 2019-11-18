<!doctype html>
<html>
<head>
    <link rel="stylesheet" href="style.css" type="text/css"/>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<title>Склад. Приход.</title>
</head>
<body>

<p><b>Склад. Приход.</b></p>

<a href="home.php">Вернуться на склад</a>

<br /><br />

<table>
  <tr>
    <th class="row11"><b>№ п/п</b></th>
    <th class="row12"><b>Наименование</b></th>
	<th class="row13"><b>Производитель</b></th>
	<th class="row14"><b>Код</b></th>
	<th class="row15"><b>Количество</b></th>
	<th class="row16"><b>Дата</b></th>
  </tr>

<?php
include_once("db.php");

$result = mysql_query(" SELECT id,name,producer,code,count,date FROM incomes
                        ORDER BY id ASC
					  ");                                       // выбрать все записи из таблицы equips, сортировать по id

mysql_close();                                                  // закрыть соединение с бд

//$row = mysql_fetch_array($result);                            // записать таблицу equips в переменную $row

while($row = mysql_fetch_array($result))                        // mysql_fetch_array($result) || mysql_fetch_assoc($result)
{
?>

  <tr>
    <td><?php echo $row['id'];?></td>
    <td><?php echo $row['name'];?></td>
	<td><?php echo $row['producer'];?></td>
	<td><?php echo $row['code'];?></td>
	<td><?php echo $row['count'];?></td>
	<td><?php echo $row['date'];?></td>
  </tr>

<?php } ?>

</table>
</body>
</html>