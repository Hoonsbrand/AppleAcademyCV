//
//  TimerModeViewController.swift
//  The Gym Hero
//
//  Created by Hoonsbrand on 12/14/21.
//

import UIKit

class TimerModeViewController: UIViewController {
        
    @IBOutlet weak var timePicker: UIPickerView!
    @IBOutlet weak var timeLabel: UILabel!
    
    var hour: Int = 0
    var minutes: Int = 0
    var seconds: Int = 0
    var timer = Timer()
    var totalTime = 0
    var secondsPassed = 0
    var minute = 0
    
    var totalPassed = 0
    var applyMinute = 0
    var applySecond = 0
    var onlySecond = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        timePicker.delegate = self
    }
    
    @IBAction func startButton(_ sender: Any) {
        timer.invalidate()
        
        totalTime = (timePicker.selectedRow(inComponent: 0) * 60) +                     timePicker.selectedRow(inComponent: 1)
        
        timeLabel.frame.size = timeLabel.intrinsicContentSize
        
        secondsPassed = 0
        
        timer = Timer.scheduledTimer(timeInterval: 1, target: self, selector: #selector(updateMinuteSecond), userInfo: nil, repeats: true)
    }
    
    @objc func updateTimer(){
        if secondsPassed <= totalTime {
            timeLabel.text = String(totalTime - secondsPassed)
            secondsPassed += 1
        } else {
            timeLabel.text = "FINISHED!"
            timer.invalidate()
        }
    }
    
    @objc func updateMinuteSecond() {
        totalPassed = totalTime - secondsPassed
        applyMinute = totalPassed / 60
        applySecond = totalPassed % 60
        onlySecond = totalTime - secondsPassed
        
        if secondsPassed <= totalTime {
            if totalPassed >= 60 && applySecond != 0 {
                if applyMinute < 10 && applySecond < 10 {
                    timeLabel.text = String("0\(applyMinute):0\(applySecond)")
                } else if applyMinute < 10 && applySecond > 10 {
                    timeLabel.text = String("0\(applyMinute):\(applySecond)")
                } else if applyMinute > 10 && applySecond < 10 {
                    timeLabel.text = String("\(applyMinute):0\(applySecond)")
                } else if applyMinute == 10 && applySecond < 10 {
                    timeLabel.text = String("\(applyMinute):0\(applySecond)")
                } else if applyMinute < 10 && applySecond == 10 {
                    timeLabel.text = String("0\(applyMinute):\(applySecond)")
                } else {
                    timeLabel.text = String("\(applyMinute):\(applySecond)")
                }
            } else if totalPassed >= 60 && applySecond == 0 {
                if applyMinute < 10 {
                    timeLabel.text = String("0\(applyMinute):00")
                } else {
                    timeLabel.text = String("\(applyMinute):00")
                }
            } else {
                if onlySecond < 10 {
                    timeLabel.text = String("00:0\(onlySecond)")
                } else {
                    timeLabel.text = String("00:\(onlySecond)")
                }
            }
            secondsPassed += 1
        } else {
            timeLabel.text = "FINISHED!"
            timer.invalidate()
        }
    }
    
    @IBAction func resetButton(_ sender: UIButton) {
        timer.invalidate()
        timeLabel.text = "00:00"
        totalTime = 0
        secondsPassed = 0
        totalPassed = 0
        applyMinute = 0
        applySecond = 0
        onlySecond = 0

    }
    
    
    func getRow(row: Int) -> String {
        let rowNumber = timePicker.selectedRow(inComponent: row)
        
        if rowNumber < 10 {
            return "0\(rowNumber)"
        }
        else {
            return String(rowNumber)
        }
    }
    
}

extension TimerModeViewController: UIPickerViewDelegate, UIPickerViewDataSource {
    
    func numberOfComponents(in pickerView: UIPickerView) -> Int {
//        return 3
        return 2
    }
    
    func pickerView(_ pickerView: UIPickerView, numberOfRowsInComponent component: Int) -> Int {
        switch component {
        case 0, 1:
            return 60
//        case 1, 2:
//            return 60
        default:
            return 0
        }
    }
    
    func pickerView(_ pickerView: UIPickerView, widthForComponent component: Int) -> CGFloat {
        return pickerView.frame.size.width/2
    }
    
    func pickerView(_ pickerView: UIPickerView, titleForRow row: Int, forComponent component: Int) -> String? {
        switch component {
        case 0:
//            return "\(row) Hour"
            return "\(row) Minute"

        case 1:
//            return "\(row) Minute"
            return "\(row) Second"

//        case 2:
//            return "\(row) Second"
        default:
            return ""
        }
    }
    
    func pickerView(_ pickerView: UIPickerView, didSelectRow row: Int, inComponent component: Int) {
        switch component {
        case 0:
//            hour = row
            minutes = row

        case 1:
//            minutes = row
            seconds = row

//        case 2:
//            seconds = row
        default:
            break;
        }
    }
}
