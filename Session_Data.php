<?php
include 'db_connect.php';

$userId = $_POST["User_ID"];
$startSession = $_POST["Start_Session"];

$stmt = $conn->prepare("INSERT INTO `Sessions`(`userId`, `startSession`) VALUES (?, ?)");
$stmt->bind_param("is", $userId, $startSession);

if ($stmt->execute()) {
    echo $conn->insert_id;
} else {
    echo "Error: " . $stmt->error;
}

$stmt->close();
$conn->close();

?>
