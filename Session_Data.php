<?php
include 'db_connect.php'; // Include the database connection

$userId = $_POST["User_ID"];
$startSession = $_POST["Start_Session"];

if($conn->connect_error)
{
    die("Connection failed: " . $conn->connect_error);
}
$sql ="INSERT INTO `Sessions`(`userId`, `startSession`)  VALUES ('$userId', '$startSession')";

if ($conn->query($sql) == TRUE) 
{       
  $sessionId = $conn->insert_id;
  echo $sessionId;
}
  
  $conn->close();
?>
