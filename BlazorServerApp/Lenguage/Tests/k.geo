r = {1, 2, {3}};


j=line (point(145,223),point(229,15));
k=circle (point(261,144),84);

draw k;
a,b,_=intersect(j,k);

draw segment(a,b);



regularTriangle(p,m) =
    let
        point p2;
        l1 = line(p,p2);
        c1 = circle(p,m);
        i1,i2,_ = intersect(l1,c1);
        c2 = circle(i1,m);
        c3 = circle(i2,m);
        i3,i4,_ = intersect(c2,c1);
        i5,i6,_ = intersect(c3,c1);
    in {i1,i5,i6};  

a,b,c,_=regularTriangle(point(123,100),70);

draw {segment(a,b),segment(b,c),segment(a,c)};
            

           ####3#


regularHexagon(p,m) =
    let
        point p2;
        l1 = line(p,p2);
        draw l1;
        c1 = circle(p,m);
        draw c1 ;
        i1,i2,_ = intersect(l1,c1);
        c2 = circle(i1,m);
        draw c2;
        c3 = circle(i2,m);
        draw c3;
        i3,i4,_ = intersect(c2,c1);
        i5,i6,_ = intersect(c3,c1);
    in {i1,i3,i5,i2,i6,i4};

print "Regular passed";


mediatrix(p1, p2) = 
    let
        l1 = line(p1, p2);
        m = measure (p1, p2);
        c1 = circle (p1, m);
        c2 = circle (p2, m);
        i1,i2,_ = intersect(c1, c2);
    in line(i1,i2); 

print "mediatrix passed";

