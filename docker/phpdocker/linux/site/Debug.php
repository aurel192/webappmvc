<?php

class Debug
{
    public static function console_log( $data, $msg = '', $email = null, $color = null, $bgcolor = null) 
    {        
        if(Development){
            $isArray = false;
            if ($color || $bgcolor) {
                $msg = '%c ' . $msg;
            }                
            $colorTag =  '';
            if ($color)
                $colorTag .=  "color: $color;";
            if ($bgcolor)
                $colorTag .=  "background: $bgcolor;";       
            $class = '';
            $classColor = '';
            if (is_object($data)){
                $class = ' %c ' . get_class($data);
                $msg .= $class;
                if (!(strlen($colorTag)>0))
                    $colorTag = 'color:green';
                else
                    $classColor = 'color:green';
            }
            else if (is_string($data)){
                $msg .= ' %c string';
                if (!(strlen($colorTag)>0))
                    $colorTag = 'color:green';
                else
                    $classColor = 'color:green';
            }
            else if (is_array($data)){
                $isArray = true;
                $msg .= ' %c array';
                if (!(strlen($colorTag)>0))
                    $colorTag = 'color:green';
                else
                    $classColor = 'color:green';
            }
            else if (is_numeric($data)){
                $msg .= ' %c numeric';
                if (!(strlen($colorTag)>0))
                    $colorTag = 'color:green';
                else
                    $classColor = 'color:green';
            }
            $array = array($msg, $colorTag, $classColor, json_encode($data));
            self::GenerateConsoleLog($array, 4, $isArray);
        }
        if ($email != null)
            Email::send($email, $msg, $data);  
        return json_encode( $data );
    }
    
    private static function GenerateConsoleLog($array, $jsonPos, $isArray)
    {
        $jsonPos--;
        echo '<script>console.log(';
            $i = 0;
            $lenght = count($array);
            foreach ($array as $value) {
                if ($i != $jsonPos)
                    echo "'";
                if ($isArray && $i== $jsonPos)
                    echo '';//'"ARRAY"';
                else
                    echo $value;
                if ($i != $jsonPos)
                    echo "'";
                if ($i != $lenght - 1)
                    echo ', ';
                $i++;
            }
        echo ');';
        if ($isArray){
            echo 'console.table(';
            echo $array[$jsonPos];
            echo ');';
        }
        echo '</script>';
    }
    
    public static function console_log2( $data, $msg = '', $email = null)
    {        
        $data = var_export($data, true);
        if(Development){
            echo '<script>';        
            echo 'console.log("'. $msg . '",' . json_encode( $data ) .')';
            echo '</script>';            
        }
        if ($email != null)
            Email::send($email, $msg, $data);  
        return json_encode( $data );
    }       
    
    public static function var_dump_pre($mixed = null, $msg = '')
    {     
        if(!Development)
            return;    
        echo '<br>';
        echo $msg;
        echo '<br>';
        echo '<pre>';
        $res = var_dump($mixed);
        echo '</pre>';
        return $res;
    }
    
    public static function print_r_pre($mixed = null, $msg = '')
    {
        if(!Development)
            return;    
        echo '<br>';
        echo '  <div align="center"><font color="red"><b>'.$msg.'</b></font></div>';
        echo '<br>';    
        echo '<pre  align="left">';
        $res = print_r($mixed);
        echo '</pre>';
        return $res;
    }    

    public static function CreateLog(){
        $user_ip = getenv('REMOTE_ADDR');
        $IPskipList = array(
            // "193.6.168.66" => true
        );  
        $SessionSkipList = array("location");
        $PostSkipList = array("LoginPassword","loginpassword");
        foreach ($IPskipList as $key => $value) {
            if ($key == $user_ip){
                return;
            }
        }
        $log = '';
        $log .= '==========  ' . date ('Y-m-d H:i:s') . '  ==========' . "\n";
        foreach ($_POST as $key => $value) {
            if (!is_object($value) && !is_array($value) && !in_array($key, $PostSkipList))
                $log .= 'POST: ' . $key . ' ' . $value . "\n";
        }
        foreach ($_SESSION as $key => $value) {
            if (!is_object($value) && !is_array($value) && !in_array($key, $SessionSkipList))
                $log .= 'SESSION: ' . $key . ' ' . $value . "\n";
        }
        foreach ($_GET as $key => $value) {
            if (!is_object($value) && !is_array($value))
                $log .= 'GET: ' . $key . ' ' . $value . "\n";
        }
        $log .= self::getLocation();
        $log .=  "\n\n";
        return $log;
    }    
        
    private static function getLocation() {
        if (isset($_SESSION['location'])) {
            return $_SESSION['location'];
        }
        $user_ip = getenv('REMOTE_ADDR');
        $geo = unserialize(file_get_contents("http://www.geoplugin.net/php.gp?ip=$user_ip"));
        $country = $geo["geoplugin_countryName"];
        $city = $geo["geoplugin_city"];
        $result = $country . ' ' . $city . ' ' . $user_ip;
        $_SESSION['location'] = $result;
        return $result;
    }    
 
}


?>