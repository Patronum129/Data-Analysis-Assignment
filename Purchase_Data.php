<?php
include 'db_connect.php';

$userId = $_POST["User_ID"];
$sessionId = $_POST["Session_ID"];
$itemId = $_POST["Item"];
$buyDate = $_POST["Buy_Date"];

$stmt = $conn->prepare("INSERT INTO `Purchases`(`userId`, `sessionId`, `itemId`, `buyDate`) VALUES (?, ?, ?, ?)");
$stmt->bind_param("iiis", $userId, $sessionId, $itemId, $buyDate);

if ($stmt->execute()) {
    echo $conn->insert_id;
} else {
    echo "Error: " . $stmt->error;
}

$stmt->close();
$conn->close();

?>
