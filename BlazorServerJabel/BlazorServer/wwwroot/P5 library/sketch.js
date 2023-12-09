

let zoom=1;



function setup() {

  /*
  var svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
  svg.setAttribute("width", "200");
  svg.setAttribute("height", "200");
  document.body.appendChild(svg);
  
  // Crear el path
  var path = document.createElementNS("http://www.w3.org/2000/svg", "path");
  path.setAttribute("d", "M50,50 m-50,0a50,50 0 1,0 100,0a50,50 0 1,0 -100,0");
  path.setAttribute("stroke", "black");
  path.setAttribute("fill", "none");
  svg.appendChild(path);
  
  // Animar el path
  var length = path.getTotalLength();
  path.style.transition = path.style.WebkitTransition = 'none';
  path.style.strokeDasharray = length + ' ' + length;
  path.style.strokeDashoffset = length;
  path.getBoundingClientRect();
  path.style.transition = path.style.WebkitTransition = 'stroke-dashoffset 3s ease-in-out';
  path.style.strokeDashoffset = '0';
  
  
  
  */
  
  
    createCanvas(800, 800);
    
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
 

/*
function mouseWheel(event){

  if(event.deltaY>0){
    zoom-=0.1;

  }

  else{
    zoom+=0.1;
  }
}

function keyPressed(){

  if(keyCode==UP_ARROW){

    zoom+=0.1;
    zoom=constrain(zoom,0.5,2);
  }
  else if(keyCode==DOWN_ARROW){

    zoom-=0.1;
    zoom=constrain(zoom,0.5,2);
  }
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

}


function DibujarSegment(parameters){
  line(parameters.param1,parameters.param2,parameters.param3 ,parameters.param4);
}





function DibujarRay(parameters){
  // param1 y param2 son los puntos de inicio
  // param 3 y param4 son otros dos punto que pertenecen

    if(parameters.param1==parameters.param3){
  
  
          line(parameters.param1,parameters.param2,parameters.param1,800);
    }
  
    else{
          let m=(parameters.param2-parameters.param4)/(parameters.param1-parameters.param3);
  
          let n=parameters.param2-m*parameters.param1;
          if(m==0){
    
  
            
            line(parameters.param1,parameters.param2,800,parameters.param2);
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
  
  }


function DibujareEllipse(parameters){
  //frameRate(1);
 
  ellipse(parameters.param1,parameters.param2,2*(parameters.param3),2*(parameters.param3));
}


function DibujarePoint(parameters){
fill(0,0,255);
  
  point(parameters.param1,parameters.param3);
  noFill();
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

