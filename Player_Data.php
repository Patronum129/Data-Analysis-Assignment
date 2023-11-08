<?php
include 'db_connect.php';

$name = $_POST["Name"];
$country = $_POST["Country"];
$date = $_POST["Date"];

$stmt = $conn->prepare("INSERT INTO `Players`(`Name`, `Country`, `Date`) VALUES (?, ?, ?)");
$stmt->bind_param("sss", $name, $country, $date);

if ($stmt->execute()) {
    echo $conn->insert_id;
} else {
    echo "Error: " . $stmt->error;
}

$stmt->close();
$conn->close();

?>
