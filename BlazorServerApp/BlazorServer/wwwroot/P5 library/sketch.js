

let zoom=1;
let canvas;


function setup() {

  
    canvas=createCanvas(800, 800);
    
    //background(200);
    noFill();
    draw();
    //frameRate(1);
    //Dibujarline();
  }
  
  function draw(/*parameters*/) {
  
    //translate(400,400);
    
    //line(parameters.param1,parameters.param2,parameters.param3,parameters.param4);
    //ellipse(200,200,50,50);
    ///ellipse(random(width),random(height),50,50);
  }

  function destroyCanvas(){
    
  
      canvas.remove();
   
  }
/*
  function PrintText(msg){
    var divEspecified= document.getElementById('plane');

    var parrafo= document.createElement('p');

    parrafo.textContent=msg;

    divEspecified.appendChild(parrafo);


  }
  */
function DibujarLine(parameters){


  if(parameters.param1==parameters.param3){


        line(parameters.param1,-400,parameters.param1,800);
  }

  else{
        let m=(parameters.param2-parameters.param4)/(parameters.param1-parameters.param3);

        let n=parameters.param2-m*parameters.param1;
        if(m==0){
  

          
          line(-400,parameters.param2,800,parameters.param2);
        }
  
        else{

          let x1= (-400-n)/m;
          let x2= (800-n)/m;
          line(x1,-400,x2,800);
        }
  
  }
  
  if(parameters.tag!=""){
    fill(0);
    textSize(15);
    text(parameters.tag,parameters.param1-5,parameters.param2-5);
    noFill();
  }

}


function DibujarSegment(parameters){
  line(parameters.param1,parameters.param2,parameters.param3 ,parameters.param4);
  
  
  if(parameters.tag!=""){
    fill(0);
    textSize(15);
    text(parameters.tag,parameters.param1-5,parameters.param2-5);
    noFill();
  }
  
}



function DibujarRay(parameters){
  // param1 y param2 son los puntos de inicio
  // param 3 y param4 son otros dos punto que pertenecen

    if(parameters.param1==parameters.param3){
  
          if(parameters.param2>parameters.param4){
            line(parameters.param1,parameters.param2,parameters.param1,-800);
          }
          else{
            line(parameters.param1,parameters.param2,parameters.param1,800);
          }
          
    }
  
    else{
          let m=(parameters.param2-parameters.param4)/(parameters.param1-parameters.param3);
  
          let n=parameters.param2-m*parameters.param1;
          if(m==0){
    
  
            if(parameters.param1>parameters.param3){
              line(parameters.param1,parameters.param2,-800,parameters.param2);
            }
            else{
              line(parameters.param1,parameters.param2,800,parameters.param2);
            }
            
          }
    
          else{
  
            if(parameters.param1>parameters.param3){

              let y=m*(-400)+n;
              line(parameters.param1,parameters.param2,-400,y);
            }
            else{

              let y=m*800+n;
              line(parameters.param1,parameters.param2,800,y);
            }
          }
    
    }
    
    if(parameters.tag!=""){
      fill(0);
      textSize(15);
      text(parameters.tag,parameters.param1-5,parameters.param2-5);
      noFill();
    }
  
  }

function DibujareEllipse(parameters){
  //frameRate(1);
 
  ellipse(parameters.param1,parameters.param2,2*(parameters.param3),2*(parameters.param3));


  if(parameters.tag!=""){
    fill(0);
    textSize(15);
    text(parameters.tag,parameters.param1+parameters.param3,parameters.param3/2+parameters.param2);
    noFill();
  }
  
}


function DibujarePoint(parameters){
  fill(0,0,255);
  
  point(parameters.param1,parameters.param3);
  
  if(parameters.tag!=""){
    fill(0);
    textSize(15);
    text(parameters.tag,parameters.param1,parameters.param3);
    
  }
  
  noFill();
}
function DibujarArc(parameters){
  console.log(parameters.param2);
  console.log(parameters.param6);
  console.log(parameters.param2-parameters.param6);
  console.log(parameters.param1-parameters.param5);
  angleFirstRay= Math.atan( (parameters.param2-parameters.param4)/(parameters.param1-parameters.param3));
  angleSecondRay= Math.atan( (parameters.param2-parameters.param6)/(parameters.param1-parameters.param5));

  console.log(angleSecondRay);
  if(angleFirstRay==0 ){
    angleFirstRay= (parameters.param1>parameters.param3)?PI:angleFirstRay;
  }
  else{
    angleFirstRay=AngleWork(angleFirstRay,parameters.param2,parameters.param4);
  }
  if(angleSecondRay==0){
    angleSecondRay= (parameters.param1>parameters.param5)?PI:angleFirstRay;
  }
  else{
    angleSecondRay=AngleWork(angleSecondRay,parameters.param2,parameters.param6);
  }
  
  console.log(angleFirstRay);
  console.log(angleSecondRay);
  
  arc(parameters.param1,parameters.param2,parameters.param7,parameters.param7,angleFirstRay,angleSecondRay);
}
function AngleWork(angle,paramY1,paramY2){
  
  if(paramY2>=paramY1 && angle<0){
    angle= angle+PI;
  }
  
  else if(paramY2<paramY1 ){
    angle= (angle<0)?angle+2*PI: PI+angle;
  }
  return angle;
}

function Color(color){


  switch(color){

    case "red":
      stroke(255,0,0);
      break;
    case "blue":
      stroke(0,0,255);
      break;
    case "yellow":
      stroke(255,255,0);
      break;
    case "green":
      stroke(0,128,0);
      break;
    case "magenta":
      stroke(255,0,255);
      break;
    case "white":
        stroke(255,255,255);
        break;
    case "gray":
        stroke(128,128,128);
        break;
    case "cyan":
        stroke(0,255,255);
        break;
    default:
        stroke(0,0,0);
        break;
  }
}
/*

stroke(52,152,219) azul
stroke(0) negro
stroke

*/

