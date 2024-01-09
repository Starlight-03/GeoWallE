p1 = point(0, 0);
p2 = point(2, 4);
m = measure(p1, p2);
c = circle(p2, m);
s = segment(p1, p2);

draw { p1, p2 };
color cyan;
draw c;
color red;
deaw s;