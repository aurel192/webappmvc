cls

@echo off
FOR /f "tokens=*" %%i IN ('docker ps -aq') DO docker rm -f %%i

docker ps -a

docker images

pause