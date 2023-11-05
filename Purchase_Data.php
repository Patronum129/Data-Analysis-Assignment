<?php
include 'db_connect.php'; // Include the database connection

$userId = $_POST["User_ID"];
$sessionId = $_POST["Session_ID"];
$itemId = $_POST["Item"];
$buyDate = $_POST["Buy_Date"];

if($conn->connect_error)
{
    die("Connection failed: " . $conn->connect_error);
}

$sql = "INSERT INTO `Purchases`(`userId`, `sessionId`, `itemId`, `buyDate`) VALUES('$userId','$sessionId', '$itemId', '$buyDate')";

if ($conn->query($sql) == TRUE) 
{
    $purchaseId = $conn->insert_id;
    echo $purchaseId;    
}

else {echo "Error";}
 
$conn->close();
?>
