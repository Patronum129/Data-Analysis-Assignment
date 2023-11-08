<?php
include 'db_connect.php';

$sessionId = $_POST["Session_ID"];
$endSession = $_POST["End_Session"];

$stmt = $conn->prepare("UPDATE `Sessions` SET `endSession` = ? WHERE `sessionId` = ?");
$stmt->bind_param("si", $endSession, $sessionId);

if ($stmt->execute()) {
    echo "Session closed successfully";
} else {
    echo "Error: " . $stmt->error;
}

$stmt->close();
$conn->close();

?>
