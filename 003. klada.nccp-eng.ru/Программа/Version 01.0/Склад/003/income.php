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

<br />

<hr />

<?php
//-------------Фильтрация по дате----------------
include_once("db.php");  // подключиться к БД

$month = array("Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь");
 
echo '<form method="POST">';
 
echo '<p>Фильтр по дате.<p>';
 
echo '<table cellspacing="0" cellpadding="0" border="0">';
echo '<tr>';
echo '<td>Начало:</td>';
echo '<td>';
echo '<select name="start_day">';
 
for($i = 1; $i <= 31; $i++)
{
    echo '<option value="'.$i.'"'.((isset($_POST["start_day"]) && $_POST["start_day"] == $i)?(" selected"):("")).'>'.(($i < 10)?("0".$i):($i)).'</option>';
}
 
echo '</select>';
echo '</td>';
echo '<td>';
echo '<select name="start_month">';
 
for($i = 0; $i < count($month); $i++)
{
    echo '<option value="'.($i + 1).'"'.((isset($_POST["start_month"]) && $_POST["start_month"] == ($i+1))?(" selected"):("")).'>'.$month[$i].'</option>';
}
 
echo '</select>';
echo '</td>';
echo '<td>';
echo '<select name="start_year">';
 
for($i = date("Y"); $i >= 1901; $i--)
{
    echo '<option value="'.$i.'"'.((isset($_POST["start_year"]) && $_POST["start_year"] == $i)?(" selected"):("")).'>'.$i.'</option>';
}
 
echo '</select>';
echo '</td>';
echo '</tr>';
 
echo '<tr>';
echo '<td>Конец:</td>';
echo '<td>';
echo '<select name="end_day">';
 
for($i = 1; $i <= 31; $i++)
{
    echo '<option value="'.$i.'"'.((isset($_POST["end_day"]) && $_POST["end_day"] == $i)?(" selected"):("")).'>'.(($i < 10)?("0".$i):($i)).'</option>';
}
 
echo '</select>';
echo '</td>';
echo '<td>';
echo '<select name="end_month">';
 
for($i = 0; $i < count($month); $i++)
{
    echo '<option value="'.($i + 1).'"'.((isset($_POST["end_month"]) && $_POST["end_month"] == ($i+1))?(" selected"):("")).'>'.$month[$i].'</option>';
}
 
echo '</select>';
echo '</td>';
echo '<td>';
echo '<select name="end_year">';
 
for($i = date("Y"); $i >= 1901; $i--)
{
    echo '<option value="'.$i.'"'.((isset($_POST["end_year"]) && $_POST["end_year"] == $i)?(" selected"):("")).'>'.$i.'</option>';
}
 
echo '</select>';
echo '</td>';
echo '</tr>';
echo '</table><br />';
 
echo '<input type="submit" value="Фильтровать" name="filter" />';
echo '</form><br />';

//-------------------Поиск-------------------------
if(isset($_POST['filter']))  // если кнопка 'filter' была нажата
{
  $begin = mktime(0, 0, 0, $_POST["start_month"], $_POST["start_day"], $_POST["start_year"]);
  $end = mktime(23, 59, 59, $_POST["end_month"], $_POST["end_day"], $_POST["end_year"]);
	
  $sql = "SELECT *
          FROM incomes 
		  WHERE date>='".date("Y-m-d H:i:s", $begin)."' 
		  AND date<='".date("Y-m-d H:i:s", $end)."' 
		  ORDER BY id ASC";                                         // выбрать все записи из таблицы incomes
                                                            		// с датой между $begin и $end, сортировать по id
  $result = mysql_query($sql);
  
  if(mysql_num_rows($result) > 0)                                   // если количество рядов результата запроса больше нуля
  {                                                                 // шапка таблицы
?>
    <table>                         
      <tr>                                                           
        <th class="row11"><b>№ п/п</b></th>
        <th class="row12"><b>Наименование</b></th>
		<th class="row13"><b>Единица измерения</b></th>
	    <th class="row14"><b>Производитель</b></th>
	    <th class="row15"><b>Код</b></th>
	    <th class="row16"><b>Количество</b></th>
	    <th class="row17"><b>Дата</b></th>
      </tr>  
<?php
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
      </tr>
<?php 
	} 
?>
	</table>
<?php
  }
  else
  {
    echo "<b><span style='color:red;'>По запросу ничего не найдено!</span></b>";
  }
}
//-------------------По умолчанию-------------------------
else
{                                                                   // шапка таблицы
?>
  <table>
    <tr>
      <th class="row11"><b>№ п/п</b></th>
      <th class="row12"><b>Наименование</b></th>
	  <th class="row13"><b>Единица измерения</b></th>
	  <th class="row14"><b>Производитель</b></th>
	  <th class="row15"><b>Код</b></th>
	  <th class="row16"><b>Количество</b></th>
	  <th class="row17"><b>Дата</b></th>
    </tr>

<?php
  include_once("db.php");                                           // подключиться к БД

  $sql = "SELECT * 
          FROM incomes
          ORDER BY id ASC";                                         // выбрать все записи из таблицы incomes, сортировать по id
  
  $result = mysql_query($sql);                                      

  mysql_close();                                                    // закрыть соединение с бд

  //$row = mysql_fetch_array($result);                              // записать таблицу equips в переменную $row

  if(mysql_num_rows($result) > 0)                                   // если количество рядов результата запроса больше нуля
  {
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
      </tr>
<?php 
    }
  }
  else
  {
    echo "<b><span style='color:red;'>По запросу ничего не найдено!</span></b>";
  }
}
?>
  </table>
</body>
</html>