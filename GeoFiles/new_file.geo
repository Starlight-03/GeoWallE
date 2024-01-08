p1 = point(0, 0);
p2 = point(5, 5);
c = circle(p2, measure(p1, p2));

draw { p1, p2 };
color cyan;
draw c;