<?php
$connection = mysql_connect("localhost", "mybd_user", "123")
 or die("Не могу подключиться: " . mysql_error());              // соединение
mysql_query(" SET NAMES 'utf-8' ");                             // || mysql_set_charset("utf-8") - выставить кодировку
$db = mysql_select_db("my_bd");                                 // БД

if(!$connection || !$db)                                        // если нет соединения или нет такой БД
{
  exit(mysql_error());                                          // прекратить работу скрипта и вывести ошибку
}
?>