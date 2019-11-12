#!/bin/bash
if [ $# -eq 0 ]
 then
   echo Restoring backup.
   echo No password given as argument.
   echo Please enter database password:
   read -s password
   else 
      password=$1
 fi
 #make sure that the proper permissions are given to the backup files
chmod -R 777 backup/*
echo Copying ygoproa.bak file from backup directory to container.
#make sure that backup folder exists in the container
docker exec -it db mkdir /var/opt/mssql/backup
#copty the file
docker cp backup/ygoproa.bak db:/var/opt/mssql/backup/

docker exec -it db /opt/mssql-tools/bin/sqlcmd -S localhost \
   -U SA -P $password \
   -Q 'RESTORE FILELISTONLY FROM DISK = "/var/opt/mssql/backup/ygoproa.bak"' \
   | tr -s ' ' | cut -d ' ' -f 1-2

echo Restoring database
docker exec -it db /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U SA -P $password \
   -Q 'RESTORE DATABASE YgoProAnalytics FROM DISK = "/var/opt/mssql/backup/ygoproa.bak" WITH MOVE "YgoProAnalytics" TO "/var/opt/mssql/data/YgoProAnalytics.mdf", MOVE "YgoProAnalytics_log" TO "/var/opt/mssql/data/YgoProAnalytics_log.ldf", REPLACE'

echo Verify the restored database below:  
docker exec -it db /opt/mssql-tools/bin/sqlcmd \
   -S localhost -U SA -P $password \
   -Q 'SELECT Name FROM sys.Databases' 

#remove file from docker container after restore.
docker exec -it db rm -rf /var/opt/mssql/backup/ygoproa.bak