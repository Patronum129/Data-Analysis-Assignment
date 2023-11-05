<?php
include 'db_connect.php'; // Include the database connection

$name = $_POST["Name"];
$country = $_POST["Country"];
$date = $_POST["Date"];

if($conn->connect_error)
{
    die("Connection failed: " . $conn->connect_error);
}

$sql = "INSERT INTO `Players`(`Name`, `Country`, `Date`) VALUES ('$name','$country', '$date')";

if ($conn->query($sql) == TRUE) {
    $userId = $conn->insert_id;
    echo $userId;
  }
  
  $conn->close();
?>
