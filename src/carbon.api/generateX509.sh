openssl genrsa -out privatekey.pem 2048
openssl req -new -x509 -key privatekey.pem -out publickey.cer -days 1825 -subj "/C=AU/ST=Tasmania/L=Hobart/O=Zeryter XYZ/OU=Progessive Development/CN=zeryter.xyz"
openssl pkcs12 -export -out public_privatekey.pfx -inkey privatekey.pem -in publickey.cer -password pass:the_game
