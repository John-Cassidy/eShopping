# Create Cert with OpenSSL
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout localhost.key -out localhost.crt -config localhost.conf -passin pass:Beherenow_Beginnersm3nd

# Create pfx file
openssl pkcs12 -export -out localhost.pfx -inkey localhost.key -in localhost.crt

