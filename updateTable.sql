UPDATE Entry 
SET usable=1
WHERE id > 0;

UPDATE Entry 
SET usable=0
WHERE LENGTH(description) < 4 and
id > 0;

UPDATE Entry 
SET usable=0
WHERE description regexp '^[^(\s^)]{1,3}[:space:]\\('
and id > 0;

UPDATE Entry 
SET usable=0
WHERE description regexp '\''
and id > 0;

SELECT * FROM spanzuratoarea.Entry WHERE description regexp '^[^(/]{1,5}[/]';

UPDATE Entry 
SET usable=0
WHERE description regexp '^[^(/]{1,5}[/]'
and id > 0;

UPDATE Entry 
SET usable=0
WHERE description regexp '^[^(/]+[\-]'
and id > 0;

UPDATE Entry 
SET usable=0
WHERE MD5(description) <> MD5(LOWER(description))
and id > 0;