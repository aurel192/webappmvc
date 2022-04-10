clear
echo "STOPPING DOCKER CONTAINERS"
docker stop -f $(docker ps -a -q)
docker rm -f $(docker ps -a -q)
echo "STARTING DOCKER CONTAINERS ..."
docker load -i /home/aurel/image.tar
docker load -i /home/aurel/consoleappimage.tar
docker-compose up
echo "DOCKER COMPOSE READY !!!"
read -p "Press any key!"
