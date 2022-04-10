<?php

class Query
{
    public $success;
    public $list;
    public $filtered;
    public $ordered;
    public $offsetlimit;
    public $data;
    public $sqlQuery;
    public $bindParams;
    public $filterObject;

    public function __construct($sqlQueryOrArray = null, $bindParams = null)
    {
        $this->success = false;
        $this->list = array();
        $this->filtered = array();
        $this->offsetlimit = array();
        $this->ordered = array();
        $this->data = null;     
        $this->bindParams = $bindParams;
        $this->filterObject = null;
        if(is_array($sqlQueryOrArray)){
            $this->list = $sqlQueryOrArray;
        }
        else if (is_string($sqlQueryOrArray)){
            $this->Query($sqlQueryOrArray, $bindParams);               
        }
    }

    public function All($obj = null, $alternateDb = false) {
        //Debug::console_log($this->filterObject, "Query->All($this->filterObject)");
        $this->success = false;
        if ($this->filterObject == null) {
            $this->list = PdoSql::selectObj($this->sqlQuery, ALL, $obj, $this->bindParams, $alternateDb);
            //Debug::console_log($this->list, 'CORE / QUERY / ALL $this->list ');
            if ($this->list != false)
                $this->success = true;
            return $this->list;
        }
        else {
            if (is_array($this->filterObject->value)){
                $result = array();
                $subResult = array();
                foreach($this->filterObject->value as $subValue) {
                    $subResult = $this->_Filtering($this->filterObject->key, $subValue, ALL, $this->filterObject->property, $this->filterObject->distinct);
                    $result = array_merge($result, $subResult);
                }
                //Debug::console_log($result, 'Query / All /$result');
                return $result;
            }
            else {
                return $this->_Filtering($this->filterObject->key, $this->filterObject->value, ALL, $this->filterObject->property, $this->filterObject->distinct);
            }
        }
    }
    
    public function First($obj = null, $alternateDb = false) {
        $this->success = false;
        if ($this->filterObject == null) {
            $this->data = PdoSql::selectObj($this->sqlQuery, FIRST, $obj, $this->bindParams, $alternateDb);
            if ($this->data != false)
                $this->success = true;
            return $this->data;
        }
        else {
            return $this->_Filtering($this->filterObject->key, $this->filterObject->value, FIRST, $this->filterObject->property, $this->filterObject->distinct);
        }
    }

    public function Query($sqlQuery = null, $bindParams = null)
    {              
        $this->sqlQuery = preg_replace('!\s+!', ' ', str_replace("\r\n",' ',$sqlQuery) );
        $this->bindParams = $bindParams;
        $this->filterObject = null;
        return $this;
    }

    public function Order($orderBy, $type = STR, $dir = 'asc')    
    {
        if (count($this->list) == 0) {
            $this->success = false;
            return array();
        }
        $asc = (strcmp(strtolower($dir),'asc') == 0);
        $arr = $this->list;        
        $this->_sortArrayByKey($arr, $orderBy, $type == STR, $asc);
        $this->ordered = $arr;
        return $this->ordered;
    }
    
    public function Filter($key, $value, $property = false, $distinct = true) {
        $this->filterObject = new FilterObject($key, $value, $property, $distinct);
        return $this;
    }

    public function OffsetLimit($offset, $limit){
        $this->offsetlimit = array();
        $cntr = 0;
        foreach($this->list as $key => $elem) {
            if ($key >= $offset){
                $this->offsetlimit []= $elem;
                if (++$cntr >= $limit) break;
            }
        }
        return $this->offsetlimit;
    }

    private function _Filtering($key, $value, $mode = FIRST, $property = false, $distinct = true)
    {
        $this->success = false;
        $this->filtered = array();
        foreach($this->list as $elem) {
            if ($elem->{$key} == $value || $value == ANYTHING) {
                // FIRST
                if ($mode == FIRST){
                    // WHOLE OBJECT
                    if ($property == false){
                        $this->success = true;
                        $this->data = $elem;
                        return $elem;
                    }
                    // JUST A PROPERTY
                    else {
                        $this->success = true;
                        $this->data = $elem->{$property};
                        return $elem->{$property};
                    }
                }
                // ALL
                else {
                    // WHOLE OBJECT
                    if ($property == false){
                        if ($distinct){
                            // TODO Object comparision, MERT ez még nem distinct !!!
                            $this->success = true;
                            $this->filtered[] = $elem; 
                        }
                        else {
                            $this->success = true;
                            $this->filtered[] = $elem;    
                        }                        
                    }
                    // JUST A PROPERTY
                    else {
                        if ($distinct){
                            if (in_array($elem->{$property}, $this->filtered) == false){
                                $this->success = true;
                                $this->filtered[] = $elem->{$property};
                            }
                        }
                        else {
                            $this->success = true;
                            $this->filtered[] = $elem->{$property};
                        }
                    }
                }
            }
        }
        if ($mode == FIRST){
            $this->data = null;
            return null;
        }
        return $this->filtered;
    }

    private function _sortArrayByKey(&$array, $key, $string = false, $asc = true){
        if($string){
            usort($array,function ($a, $b) use(&$key,&$asc)
            {
                if($asc)    return strcmp(strtolower($a->{$key}), strtolower($b->{$key}));
                else        return strcmp(strtolower($b->{$key}), strtolower($a->{$key}));
            });
        }else{
            usort($array,function ($a, $b) use(&$key,&$asc)
            {
                if($a->{$key} == $b->{$key}){return 0;}
                if($asc) return ($a->{$key} < $b->{$key}) ? -1 : 1;
                else     return ($a->{$key} > $b->{$key}) ? -1 : 1;
            });
        }
    }
}

class FilterObject
{
    public $key;
    public $value;
    public $property;
    public $distinct;

    public function __construct($key = null, $value = null, $property = false, $distinct = true)
    {
        $this->key = $key;
        $this->value = $value;
        $this->property = $property;
        $this->distinct = $distinct;
    }
}