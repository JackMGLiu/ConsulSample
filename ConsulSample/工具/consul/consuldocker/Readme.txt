
build consul����
docker build -t gliderlabs/consul-server:$('1.0.2') .

����consul����
docker run -p 8500:8500 --name hisconsul -d gliderlabs/consul-server:1.0.2


����
http://localhost:8500/