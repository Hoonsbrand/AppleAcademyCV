#  Personal Project - The Gym Hero
 
This Personal Project is made for reviewing what i have learned from Udemy Angela's IOS Course.

[Functions & UI Explanation]

(LaunchScreen)

![simulator_screenshot_D71BF05B-9DA3-4B6B-B7C1-47DC9DEEC948](https://user-images.githubusercontent.com/63503972/146531222-d43cf34c-393b-4b33-a38e-ca132e6999e4.png)

Neon sign - was designed by me through "canva.com".
The gym hero icon - downloaded from google.
delay - I gave it 2 seconds of delay and used "Thread.sleep(forTimeInterval: 2.0)" at AppDelegate.swift


(ViewController)

![simulator_screenshot_F59B55DA-722B-4C4C-BD80-C05CD42828A4](<img src = https://user-images.githubusercontent.com/63503972/146531336-452adc9f-45cd-469d-afb1-e71615a7125a.png width="30" height="30">)

"Wanna be cool? Do Swift" - used RGB color from the swift icon on dumbbell, I wanted to give them color match.

Navigation bar - You can go back through navigation bar.

You can select the Mode out of two options :

(Timer Mode)

![simulator_screenshot_3BF67DC0-B640-4B64-9353-FDCCC3A2000D](https://user-images.githubusercontent.com/63503972/146531739-8ae99c17-1356-41dc-9737-e934407375f8.png)
 
I used timePicker and customized it by minute and seconds only mode.
refered to - 
https://stackoverflow.com/questions/47844460/how-have-hourminutesseconds-in-date-picker-swift

You can also use reset button whenever you want to reset the timer.

issues - when it started to count down, even it was bigger than 60 seconds (minute),       it showed up with seconds like counted down from 120.
         when minute and seconds were smaller than 10, it displayed without zero before the numbers. 
         
solve - I wanted to solve it by myself. I used if - else. 


(Counting Mode)

![simulator_screenshot_B090D710-2B9A-4F60-9CB1-F037BB4C8868](https://user-images.githubusercontent.com/63503972/146537162-13be5932-987e-49b6-86dd-40171354f503.png)

The 12, 15, 20 number boxes are from my trainer at the gym.
That's what usually my trainer make me do it.

When you click the one of the three boxes, you can choose the Interval.
The number of Interval is countdown the number of counts every seconds you have chose.
Whenever a set finished, my trainer tells me to drink water, so I made my code displayed it with sound when the counts become a zero.

UIAlertController - I wanted to use Alert Function, I brought a code from stackoverflow. same link with Timer Mode - timePicker.




--------------------------------------------------------------------------------------
todo - change the if - else from TimerModeViewController to more efficiently.
       solve Exception caught in AudioQueueInternalNotifyRunning - error -66671. 
 


