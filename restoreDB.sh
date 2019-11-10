#!/bin/bash
if [ $# -eq 0 ]
 then
   echo Restoring backup.
   echo No password given as argument.
   echo Please enter database password:
   read password
   else 
      password=$1
 fi

#echo Copying backup file from current directory to container.
docker cp backup/ygoproa.bak db:/var/opt/mssql/backup

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