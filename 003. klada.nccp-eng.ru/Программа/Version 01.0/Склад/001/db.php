<?php
$connection = mysql_connect("localhost", "mybd_user", "123");   // ����������
mysql_query(" SET NAMES 'utf-8' ");                             // || mysql_set_charset("utf-8") - ��������� ���������
$db = mysql_select_db("my_bd");                                 // ��

if(!$connection || !$db)                                        // ���� ��� ���������� ��� ��� ����� ��
{
  exit(mysql_error());                                          // ���������� ������ ������� � ������� ������
}
?>