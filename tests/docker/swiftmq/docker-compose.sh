#!/bin/bash
  
export myIP=`ifconfig $(netstat -rn | grep -E "^default|^0.0.0.0" | head -1 | awk '{print $NF}') | grep 'inet ' | awk '{print $2}' | grep -Eo '([0-9]*\.){3}[0-9]*'`
export PWD=`pwd`

case "$1" in
  start)
        docker-compose pull swiftmq
        docker-compose up -d
    ;;
  stop)
        docker-compose down
    ;;
  status)
        docker ps
    ;;
  *)
    echo "Usage: $N {start|stop|status}"
    exit 1
    ;;
esac
exit 0