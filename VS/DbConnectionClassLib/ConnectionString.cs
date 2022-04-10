#define REMOTEDOCKERMYSQL
//#define LOCALDOCKERMYSQL

namespace DbConnectionClassLib
{
    public static class ConnectionString
    {
        public static string Get()
        {
            #if REMOTEDOCKERMYSQL
                return "server=80.211.196.146;database=DockerMySqlDb2;uid=root;pwd=Qwe123!!!Qwe123!!!";
            #elif LOCALDOCKERMYSQL
                return "server=mysqlserver;database=DockerMySqlDb2;uid=root;pwd=Qwe123!!!Qwe123!!!";     
            #else
                return "server=localhost;database=LocalMySqlDb2;uid=root";
            #endif
        }
    }
}
