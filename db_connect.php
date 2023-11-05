<?php
$servername = "localhost:3306";
$username = "hangx";
$password = "7vGPx2sehzBY";
$database = "hangx"; 

// Create connection
$conn = new mysqli($servername, $username, $password, $database);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}
?>
