?php

$day = date(&#39;j&#39;); //what day is it today
$month = date(&#39;n&#39;); //what month are we in?
$year = date(&#39;Y&#39;); //what year are we in?
//get the first first day of the month
$first_day_of_month = date(&#39;w&#39;,mktime(1,1,1,$month,1,$year));
$days_in_month = date(&#39;t&#39;); //how many days in this month

print "SU\tMO\tTU\tWE\tTH\tFR\tSA\n"; //print the weekday headers
//count ahead to the weekday of the first day of the month
for ($spacer = 0; $spacer < $first_day_of_month; $spacer++) {
  print "  \t";
}

for ($x = 1; $x <= $days_in_month; $x++) { //begin our main loop
  //if we have gone past the end of the week, go to the next line
  if ($spacer >= 7) { print "\n"; $spacer = 0; }
  //if the length of the current day is one, put a "0" in front of it
  if (strlen($x) == 1) { $x = "0$x"; }
  if ($x == $day) { //is this day the current day
    print "$x^\t"; //if so put an indicator
  } else {
    print "$x\t"; //otherwise just print it
  }
  $spacer++; //increment our spacer
}

?>