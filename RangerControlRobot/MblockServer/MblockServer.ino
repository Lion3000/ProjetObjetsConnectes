///
/// \file MblockServer.c
/// \brief Permet de contrôler le robot
/// \author Camélia Benhmida Zarzitski

#include <Arduino.h>
#include <Wire.h>
#include <MeAuriga.h>
#include <SoftwareSerial.h>

/// \def Enum des commandes possibles
// DÉFINITION DE L'ENUM DES COMMANDES
///
#define TRACKS_FORWARD 0
#define TRACKS_BACKWARD 1 
#define TRACK_TURN_LEFT 2
#define TRACK_TURN_RIGHT 3
#define TRACKS_STOP 4
#define ARM_UP 5
#define ARM_DOWN 6
#define ARM_STOP 7
#define GRIPPER_OPEN 8
#define GRIPPER_CLOSE 9
#define GRIPPER_STOP 10 
#define ALL_STOP 12

// Autres constantes pour les directions
#define BACKWARD 1
#define FORWARD 2
#define TURN_LEFT 3
#define TURN_RIGHT 4

// DEFINITION DES MOTEURS
MeEncoderOnBoard Encoder_1(SLOT1);  // moteur ranger gauche
MeEncoderOnBoard Encoder_2(SLOT2);  // moteur ranger droit
MeDCMotor motor_1(1); // moteur bras
MeDCMotor motor_2(2); // moteur pinces

// Définition de la vitesse des moteurs roues
int8_t Speed = 150;

// DÉFINITION DES INTERRUPTIONS
void isr_process_encoder1(void)
{
      if(digitalRead(Encoder_1.getPortB()) == 0){
            Encoder_1.pulsePosMinus();
      }else{
            Encoder_1.pulsePosPlus();
      }
}

void isr_process_encoder2(void)
{
      if(digitalRead(Encoder_2.getPortB()) == 0){
            Encoder_2.pulsePosMinus();
      }else{
            Encoder_2.pulsePosPlus();
      }
}
///
/// \fn move(int direction, int speed)
// Fonction qui permet de déplacer le robot
///
void move(int direction, int speed)
{
      int leftSpeed = 0;
      int rightSpeed = 0;
      if(direction == 1){
            leftSpeed = -speed;
            rightSpeed = speed;
      }else if(direction == 2){
            leftSpeed = speed;
            rightSpeed = -speed;
      }else if(direction == 3){
            leftSpeed = -speed;
            rightSpeed = -speed;
      }else if(direction == 4){
            leftSpeed = speed;
            rightSpeed = speed;
      }
     
       Encoder_1.setTarPWM(leftSpeed);
       Encoder_2.setTarPWM(rightSpeed);
      
}

double angle_rad = PI/180.0;
double angle_deg = 180.0/PI;

void setup(){
    
    //Set Pwm 8KHz
    TCCR1A = _BV(WGM10);
    TCCR1B = _BV(CS11) | _BV(WGM12);
    TCCR2A = _BV(WGM21) | _BV(WGM20);
    TCCR2B = _BV(CS21);
    
    attachInterrupt(Encoder_1.getIntNum(), isr_process_encoder1, RISING);
    attachInterrupt(Encoder_2.getIntNum(), isr_process_encoder2, RISING);
    Encoder_1.setPulse(9);
    Encoder_1.setRatio(39.267);
    Encoder_1.setPosPid(1.8,0,1.2);
    Encoder_1.setSpeedPid(0.18,0,0);
    Encoder_2.setPulse(9);
    Encoder_2.setRatio(39.267);
    Encoder_2.setPosPid(1.8,0,1.2);
    Encoder_2.setSpeedPid(0.18,0,0);
    Serial.begin(9600, SERIAL_8E1);
}

///
/// \fn int getIntFromSerial(char c1, char c2)
/// \brief Fonction qui permet de calculer la 
/// valeur reçue en sérial
/// \param c1 char le premier caractère reçu
/// \param c2 char le second caractère reçu
/// \return int : entier correspondant à la 
/// commande reçue
///
int getIntFromSerial(char c1, char c2){  
  int in1 = c1 - '0';
  int in2 = c2 - '0';
  Serial.println(in1); 
  Serial.println(in2);
  if(c1 == '0'){
    return c2 - '0';
   // Serial.println(c1);
  }
  else{
    int i2 = c2 - '0';
   // Serial.println(i2);  
    switch(i2){
      case 0 :
      return 10;
      break;

      case 1 :
      return 11;
      break;

      case 2 :
      return 12;
      break;

      default:
      return 12;
      break;
    }
  }
}
void loop(){
     _loop(); 
     char recievedData[2];
     if (Serial.available() > 1) {
      recievedData[0] = Serial.read();
      recievedData[1] = Serial.read();
      int cmd = getIntFromSerial(recievedData[0], recievedData[1]);
      controlRobot(cmd);
    }   
}

///
/// \fn int controlRobot(int cmd)
/// \brief contrôle le robot selon 
/// la commande reçue
/// \param cmd int : entier correspondant 
/// à la commande reçue
///
void controlRobot(int cmd){
  switch(cmd){                
        case TRACKS_FORWARD:
         // Serial.print("AVANCER ");
          move(FORWARD, Speed);
        break;
        case TRACKS_BACKWARD:
         // Serial.print("RECULER ");
          move(BACKWARD, Speed);
        break;
        case TRACK_TURN_LEFT:
         // Serial.print("TOURNER A GAUCHE ");
          move(TURN_LEFT, Speed);
        break;
        case TRACK_TURN_RIGHT:
        //  Serial.print("TOURNER A DROITE ");
          move(TURN_RIGHT, Speed);
        break;
        case TRACKS_STOP:
        //  Serial.print("TRACKS STOP ");
          move(FORWARD, 0);
        break;
        case ARM_UP:
          Serial.print("ARM UP ");
          motor_1.run(-120);
        break; 
        case ARM_DOWN:
          Serial.print("ARM DOWN ");
          motor_1.run(120);
        break; 
        case ARM_STOP:
          Serial.print("ARM STOP ");
          motor_1.run(0);
        break; 
        case GRIPPER_OPEN:
        //  Serial.print("GRIPPER OPEN ");
          motor_2.run(-110);
        break;       
         case GRIPPER_CLOSE:
        //  Serial.print("GRIPPER CLOSE ");
          motor_2.run(110);
        break; 
        case GRIPPER_STOP:
          motor_2.run(0);
        break;  
        case ALL_STOP:
        //  Serial.print("STOP ALL ");
          motor_2.run(0);
          motor_1.run(0);
          move(FORWARD, 0);
        break;          
    }
}

void _loop(){
    Encoder_1.loop();
    Encoder_2.loop();
}
void _delay(float seconds){
    long endTime = millis() + seconds * 1000;
    while(millis() < endTime)_loop();
}

