<?php

class Sql extends SqlConnection
{
    protected const ftm_server = "80.211.196.146";    
    protected const ftm_user = "root";
    protected const ftm_password = "Qwe123!!!Qwe123!!!";

    private static function connect($alternateDb = false)
    {                
        if (isset($_SESSION['sql'])) $_SESSION['sql']++;
        else $_SESSION['sql'] = 1;
        if ($alternateDb == false) {
            $conn = new mysqli(self::server, self::user, self::password, self::db);
        }
        else {
            $conn = new mysqli(self::ftm_server, self::ftm_user, self::ftm_password, $alternateDb);
        }
        if ($conn->connect_error){
            Debug::console_log($conn->error,'UNABLE TO CONNECT TO DATABASE',null,'RED','BLACK');
            Email::send('error@collectioninventory.com', 'SQL CONNECTION ERROR', $conn->error);
            exit();
        }
        return $conn;    
    }
    
    public static function selectObj($query, $firstOnly = false, $object = false)
    {
        $ret = array();
        $conn = self::connect();
        $result = $conn->query($query);
        if (!$result){
            $error = $conn->error . NewLine . 'In Sql::selectObj()' . NewLine . NewLine;
            $error .= 'Parameters:' . NewLine . NewLine;
            $error .= 'query:' . $query . NewLine;
            $error .= 'firstOnly:' . $firstOnly . NewLine;
            $error .= 'object:' . $object . NewLine;            
            Debug::console_log($error,'SQL ERROR selectObj()');
            Email::send('error@collectioninventory.com', 'SQL ERROR selectObj()', $error);            
            return $ret;
        } 
        if ($firstOnly){
            if ($object)
                $ret = $result->fetch_object($object);
            else
                $ret = $result->fetch_object();
        }
        else {                        
            if ($result->num_rows > 0) {
                if ($object){
                    while ($row = $result->fetch_object($object)) {
                        $ret[] = $row;
                    }
                }
                else {
                    while ($row = $result->fetch_object()) {
                        $ret[] = $row;
                    }            
                }            
            }        
        }
        return $ret;
    } 
    
    public static function selectAssoc($query, $firstOnly = false)
    {        
        $ret = array();
        $conn = self::connect();
        $result = $conn->query($query);
        if (!$result){
            $error = $conn->error . NewLine . 'In Sql::selectAssoc()' . NewLine . NewLine;
            $error .= 'Parameters:' . NewLine . NewLine;
            $error .= 'query:' . $query . NewLine;
            $error .= 'firstOnly:' . $firstOnly . NewLine;
            $error .= 'object:' . $object . NewLine;            
            Debug::console_log($error,'SQL ERROR selectAssoc()');
            Email::send('error@collectioninventory.com', 'SQL ERROR selectAssoc()', $error);            
            return $ret;
        }
        if ($firstOnly){            
            $ret = $result->fetch_assoc();
        }
        else {            
            if ($result->num_rows > 0) {
                while ($row = $result->fetch_assoc()) {
                    $ret[] = $row;
                }            
            }        
        }
        return $ret;
    }            
    
    public static function update($obj, $table = null, $pk = null)
    {
        $class = get_class($obj);
        $properties = get_object_vars($obj);    
        if ($table == null)
            $table = (new $class)->getTableName();              
        if ($pk == null)
            $pk = (new $class)->getPrimaryKey();                     
        $sql = 'UPDATE ' . $table;
        $keys = ' SET ';        
        foreach ($properties as $key => $value) {            
            $keys .= $key . '="' . $value . '", ';                            
        }   
        $keys = rtrim($keys, ", ");        
        $id = Helper::getObjectPropertyValueByName($obj, $pk);        
        $sql = $sql . $keys . " WHERE $pk = $id";               
        $conn = self::connect();          
        $success = $conn->query($sql);
        if (!$success){
            ob_start();
            var_dump($obj);
            $objStr = ob_get_clean();
            $error = $conn->error . NewLine . 'In Sql::update()' . NewLine . NewLine;
            $error .= 'Sql Command: ' . $sql . NewLine.NewLine;
            $error .= 'Parameters: ' . NewLine.NewLine;
            $error .= 'obj: ' . $objStr . NewLine.NewLine;
            $error .= 'table: ' . $table . NewLine.NewLine;
            $error .= 'pk: ' . $pk . NewLine.NewLine;            
            Debug::console_log($error,'SQL ERROR update()');
            Email::send('error@collectioninventory.com', 'SQL ERROR update()', $error);    
        }
        return $success;
    }
    
    public static function insert($obj, $table = null, $alternateDb = false)
    {    
        $class = get_class($obj);
		Debug::console_log($class, 'SQL insert $class');
        $properties = get_object_vars($obj);
        if ($table == null){
            // TODO!!! 
            // $table = (new $class)->getTableName();
            $table = $obj->getTableName();
        }
        $sql = 'INSERT INTO ' . $table;
        $keys = '(';
        $values = ' VALUES (';
        foreach ($properties as $key => $value) {
            if ($value && strlen($value) > 0){
                $keys .= $key . ', ';
                $values .= "'".$value . "', ";
            }                        
        }
        $keys = rtrim($keys, ", ");
        $values = rtrim($values, ", ");
        $keys .= ')';
        $values .= ')';      
        $sql .= $keys . $values;
        Debug::console_log($sql,'INSERT :');
        $conn = self::connect($alternateDb);          
        if ($conn->query($sql) === TRUE) 
            return $conn->insert_id;
        else {
            ob_start();
            var_dump($obj);
            $objStr = ob_get_clean();
            $error = $conn->error . NewLine . 'In Sql::insert()' . NewLine . NewLine;
            $error .= 'Parameters:' . NewLine . NewLine;
            $error .= 'obj:' . $objStr . NewLine;
            $error .= 'table:' . $table . NewLine;            
            Debug::console_log($error,'SQL ERROR insert()');
            Email::send('error@collectioninventory.com', 'SQL ERROR insert()', $error);           
            return -1;
        }
    }                
    
    public static function deleteObj($obj, $table = null, $pk = null)
    {
        $class = get_class($obj);
        $properties = get_object_vars($obj);
        if ($table == null)
            $table = (new $class)->getTableName();
        if ($pk == null)
            $pk = (new $class)->getPrimaryKey();
        $id = Helper::getObjectPropertyValueByName($obj, $pk);  
        $sql = 'DELETE FROM ' . $table . ' WHERE ' . $pk . ' = ' . $id;
        $conn = self::connect();  
        $success = $conn->query($sql);
        if (!$success){
            ob_start();
            var_dump($obj);
            $objStr = ob_get_clean();
            $error = $conn->error . NewLine . 'In Sql::deleteObj()' . NewLine . NewLine;
            $error .= 'Parameters:' . NewLine . NewLine;
            $error .= 'obj:' . $objStr . NewLine;
            $error .= 'table:' . $table . NewLine;          
            $error .= 'pk:' . $pk . NewLine;            
            Debug::console_log($error,'SQL ERROR deleteObj()');
            Email::send('error@collectioninventory.com', 'SQL ERROR deleteObj()', $error);     
        }
        return $success;
    }
    
    public static function query($sql)
    {                
        $conn = self::connect();
        $success = $conn->query($sql);
        if (!$success){
            $error = $conn->error . NewLine . 'In Sql::query()' . NewLine . NewLine;
            $error .= 'Parameters:' . NewLine . NewLine;
            $error .= 'sql:' . $sql . NewLine;       
            Debug::console_log($error,'SQL ERROR query()');
            Email::send('error@collectioninventory.com', 'SQL ERROR query()', $error);     
        }
        return $success;        
    }
    
    public static function listTables($all = false)
    {    
        $ret = array();               
        $conn = self::connect();
        $sql = 'SHOW TABLES FROM '.self::db;
        if(!$all)
              $sql .= ' WHERE Tables_in_'.self::db.' LIKE "items_%"';
        if(isset($_SESSION['username'])) {
            if ($_SESSION['username'] != 'admin')
                $sql .= ' AND Tables_in_'.self::db.' NOT LIKE "%teszt%"';            
        }
        else 
            $sql .= ' AND Tables_in_'.self::db.' NOT LIKE "%teszt%"';            
        $result = $conn->query($sql);
        while ($row = $result->fetch_assoc()) {
            $ret[] = $row['Tables_in_'.self::db];
        }            
        return $ret;    
    }    
}

?>