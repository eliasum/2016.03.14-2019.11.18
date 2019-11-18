<!doctype html>
<html>
<head>
    <link rel="icon" href="favicon.ico" type="image/x-icon">
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

<br />

<table>
  <tr>
    <th class="row1"><b>№ п/п</b></th>
    <th class="row2"><b>Наименование</b></th>
    <th class="row3"><b>Единица измерения</b></th>
	<th class="row4"><b>Производитель</b></th>
	<th class="row5"><b>Код</b></th>
	<th class="row6"><b>Количество</b></th>
    <th class="row7"><b>Дата</b></th>
	<th class="row8"><b>Комната/шкаф/полка</b></th>
	<th class="row9"><b>Действие</b></th>
  </tr>

<?php
include_once("db.php");

$result = mysql_query(" SELECT *
                        FROM equips
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
	<td><?php echo $row['unit'];?></td>
	<td><?php echo $row['producer'];?></td>
	<td><?php echo $row['code'];?></td>
	<td><?php echo $row['count'];?></td>
	<td><?php echo $row['date'];?></td>
	<td><?php echo $row['ksp'];?></td>
    <td>
	  <div class="links">
	    <a href="put.php?id=<?php echo $row['id']?>">Оприходовать</a>
        <a href="take.php?id=<?php echo $row['id']?>">Расходовать</a>
	  </div>
	</td>
  </tr>
<?php
}
?>

</table>
</body>
</html>