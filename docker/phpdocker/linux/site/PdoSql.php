<?php

class PdoSql extends SqlConnection
{
    protected const ftm_server = "80.211.196.146";    
    protected const ftm_user = "root";
    protected const ftm_password = "Qwe123!!!Qwe123!!!";

    private static function connect($alternateDb)
    {
        try {
            if ($alternateDb == false) {
                $db = self::db;
                $server = self::server;
                $conn = new PDO("mysql:host=$server;dbname=$db", self::user, self::password);
                Debug::console_log($conn, "PdoSql / connect => conn mysql:host=$server;dbname=$db");
                $conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
                return $conn;
            }
            else {
                $db = $alternateDb;
                $server = self::ftm_server;
                $conn = new PDO("mysql:host=$server;dbname=$db;charset=utf8", self::ftm_user, self::ftm_password);
                Debug::console_log($conn, "PdoSql / connect altdb => conn mysql:host=$server;dbname=$db");
                $conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
                return $conn;
            }
        }
        catch(PDOException $e) {
            $msg = $e->getMessage();
            Debug::console_log($e, 'PdoSql / connect PDOException msg:' . $msg);
            echo "Connection failed: " . $msg;
        }
    }
    
    public static function selectObj($query, $firstOnly = false, $object = false, $bparams = null, $alternateDb = false)
    {
        $ret = array();
        try {
            Debug::console_log($conn, 'before PdoSql / selectObj => conn');
            $conn = self::connect($alternateDb);
            Debug::console_log($conn, 'after PdoSql / selectObj => conn');
            $userid = 0;
            $statement = $conn->prepare($query);
            if ($bparams != null){
                $bindParamsArray = array();
                if (is_object($bparams)){
                    $properties = get_object_vars($bparams);
                    $bindParamsArray = $properties;
                }
                else if (is_array($bparams))
                    $bindParamsArray = $bparams;
                else 
                    throw new Exception('Invalid Bindparam! Expecting Object or Array');                
                foreach ($bindParamsArray as $key => &$value) {
                    if(is_object($value)){
                        if ($value->length != null)
                            $statement->bindParam(":$key", $value->value, $value->param, $value->length);
                        else
                            $statement->bindParam(":$key", $value->value, $value->param);
                    }
                    else {
                        $statement->bindParam(":$key", $value);
                    } 
                }
            }
            if ($statement->execute()) {
                if ($firstOnly){
                    if ($object)
                        $ret = $statement->fetchObject($object);
                    else
                        $ret = $statement->fetchObject();
                }
                else {
                    if ($object){
                        while ($row = $statement->fetchObject($object)) {
                            $ret[] = $row;
                        }
                    }
                    else {
                        while ($row = $statement->fetchObject()) {
                            $ret[] = $row;
                        }
                    } 
                }
            }
            $conn = null;
        }
        catch(Exception $e) {
            Debug::console_log($e,'EXCEPTION',null,'RED','BLUE');
            $error = 'Exception at PdoSql::selectObj()' . NewLine . NewLine;
            $error .= 'MESSAGE: ' . $e->getMessage() . NewLine;;
            $error .= 'TYPE: ' . get_class($e) . NewLine;
            $error .= 'QUERY: ' . $query . NewLine;
            $error .= 'FIRSONLY: ' . ($firstOnly?'true':'false') . NewLine;
            $error .= 'OBJECT: ' . $object . NewLine;            
            $error .= 'BINDPARAMS: ' . var_export($bparams,true) . NewLine;            
            Debug::console_log($error,'PDO ERROR selectObj()', 'error@collectioninventory.com', 'white', 'red');
            return false;
        }
        return $ret;
    } 
    
    public static function selectAssoc($query, $firstOnly = false)
    {        
    }            
    
    public static function update($obj, $table = null, $pk = null)
    {
    }
    
    public static function insert($obj, $table = null)
    {    
    }                
    
    public static function deleteObj($obj, $table = null, $pk = null)
    {
    }
    
    public static function query($sql)
    {                     
    }
    
    public static function listTables($all = false)
    {     
    }    
}

// http://deadlime.hu/2006/02/11/mi-is-az-a-pdo/

?>