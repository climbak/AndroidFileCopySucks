# AndroidFileCopySucks

When you transfer files to an Android phone, the creation time of that file gets set tot the time of copy rather than keeping the original value. This wreaks havoc in places where files are organized by date, such as photo galleries.

This is a quick tool to reset the creation time of photos/videos copied from one android to another on sd cards. The file creation time is reset by using the image metadata or attempting to parse the filename for a date. Quick. Dirty. Unkempt.

C# visual studio project. Just change the directory you want to run on, put the sd card in the computer and run.

It didn't kill my data, but I make no promises as to the safety of yours. I did no real testing. Did very little research on the best way to do it. Just threw together what worked and stuck it up here for anyone else to play with.
