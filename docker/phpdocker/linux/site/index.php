 <?php


require_once 'Debug.php';
require_once 'SqlConnection.php';
require_once 'Sql.php';
require_once 'PdoSql.php';
require_once 'Query.php';
define("Development", true);
define("FIRST", true);
define("ALL", false);
define("ASC", "ASC");
define("DESC", "DESC");
define("NUM", "NUM");
define("STR", "STR");
define("ANYTHING", "ANYTHING");
define("NewLine", "\r\n"); 




$host = '80.211.196.146';
$db   = 'positions';
$user = 'root';
$pass = 'Qwe123!!!Qwe123!!!';
$charset = 'utf8mb4';
$dsn = "mysql:host=$host;dbname=$db;charset=$charset";
$options = [
    PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
    PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
    PDO::ATTR_EMULATE_PREPARES   => false,
];
try {
     $pdo = new PDO($dsn, $user, $pass, $options);
} catch (\PDOException $ex) {
     //throw new \PDOException($ex->getMessage(), (int)$ex->getCode());
     Debug::console_log($ex,'== PDOException ==');
     Debug::console_log($ex->getMessage(),'== $ex->getMessage() ==');
     Debug::console_log((int)$ex->getCode(),'== (int)$ex->getCode() ==');
}

//echo phpversion();
echo phpinfo();




/*
echo 'mivan e?<br>';
if (!function_exists('mysqli_init') && !extension_loaded('mysqli')) {
    echo 'We don\'t have mysqli!!!';
} else {
    echo 'Phew we have it!';
}

if (!extension_loaded('PDO')) {
    echo 'pdo nooo!!!';
} else {
    echo 'pdo wwwííwdaqt!';
}
*/
$sqlpositions = "SELECT * FROM positions";
// WHERE UserId = :UserId";
// $bindParams = new StdClass();
// $bindParams->UserId = 'e094a2e4-6bac-495e-b13c-d36589a501ea';
$positions =  PdoSql::selectObj($sqlpositions, ALL);
Debug::console_log($positions,'positions');


//$mysqli = new mysqli('127.0.0.1', 'your_user', 'your_pass', 'sakila');



                    



Debug::console_log('start');


//$positions = Sql::selectObj("SELECT * FROM position", FIRST);
//$bindParams = new StdClass();
//$myFavorites =  PdoSql::selectObj($sqlMyFavorites, ALL, null, $bindParams, "DockerMySqlDb2");
Debug::console_log($positions,'positions');
?>