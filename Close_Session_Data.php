<?php
include 'db_connect.php'; // Include the database connection

$sessionId = $_POST["Session_ID"];
$endSession = $_POST["End_Session"];

echo $endSession;

if($conn->connect_error)
{
    die("Connection failed: " . $conn->connect_error);
}
$sql = "UPDATE `Sessions` SET `endSession`= '$endSession' WHERE `sessionId`='$sessionId'";

if ($conn->query($sql) == TRUE) 
{
  echo "Session closed successfully"; 
} 

$conn->close();

?>
