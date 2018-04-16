## Groupe Camélia Zarzitski Benhmida & Alex Zarzitski
## Les modules fonctionnent :
## - Bluetooth serveur arduino : testé avec le téléphone portable
## - Lecture/ Ecriture du port série en python 
## - Affichage sur la map des coordonnées GPS + trace

import serial as s
import webbrowser, os
from math import *

def getPosition(x1, x2) :

	## Découpage des chaines pour extraires 
	## Les coordonnées des 2 points
	data1 = str(x1).split('\\r')
	data1 = data1[0].split('b\'')
	data1 = data1[1].split('#')
	
	data2 = str(x2).split('\\r')
	data2 = data1[0].split('b\'')
	data2 = data1[1].split('#')
	print(data1)
	print(data2)

	latitude1 = float(data1[0])
	longitude1 = float(data1[1])
	latitude2 = float(data2[0])
	longitude2 = float(data2[1])
	url = "https://www.google.com/maps/?q="+str(latitude1)+","+str(longitude1)
	# Affichage du premier point sur la map
	webbrowser.open(url)
	
	latLong1 = 	str(latitude1)+ "," + str(longitude1);
	latLong2 = 	str(latitude2)+","+ str(longitude2);
	
	## Affichage de la map avec les 2 points et de la trace 
	url = "https://www.google.com/maps/dir/"+latLong1 + "/"+ latLong2;
	print(url);
	webbrowser.open(url)
	
	return;
	
def main(ser) :
	try :
		## Demande des données GPS
		ser.write( str.encode("GPS\n\r"));
		## Réception des données GPS
		x1=ser.readline()
		x2=ser.readline()
		print(x1)
		print(x2)
		## Extraction des coordonnées et affichage de la trace
		getPosition(x1, x2)
	except Exception as e:
		main(ser);
	return;	
	

## Connexion au port série	
ser=s.Serial()
ser.baudrate = 9600
ser.port= "COM10"
ser.timeout=10
ser.open()

main(ser);

ser.close()





	
