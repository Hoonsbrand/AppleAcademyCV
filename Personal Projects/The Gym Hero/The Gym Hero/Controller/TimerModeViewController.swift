//
//  TimerModeViewController.swift
//  The Gym Hero
//
//  Created by Hoonsbrand on 12/14/21.
//

import UIKit
import AVFoundation

class TimerModeViewController: UIViewController {
    var player: AVAudioPlayer!

    @IBOutlet weak var startLabel: UIButton!
    @IBOutlet weak var stopLabel: UIButton!
    
    @IBOutlet weak var timePicker: UIPickerView!
    @IBOutlet weak var timeLabel: UILabel!
    @IBOutlet weak var startPauseButton: UIButton!
    
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
    
    var minString = "Minute"
    var secString = "Seconds"
    
    override func viewDidLoad() {
        super.viewDidLoad()
        timePicker.delegate = self
        localLang()
        //setLanguage()
    }
    
    @IBAction func startButton(_ sender: UIButton) {
        
        timer.invalidate()
        totalTime = (timePicker.selectedRow(inComponent: 0) * 60) + timePicker.selectedRow(inComponent: 1)
        timeLabel.frame.size = timeLabel.intrinsicContentSize
        secondsPassed = 0
        timer = Timer.scheduledTimer(timeInterval: 1, target: self, selector: #selector(updateMinuteSecond), userInfo: nil, repeats: true)
    }

    @objc func updateMinuteSecond() {
        totalPassed = totalTime - secondsPassed
        applyMinute = totalPassed / 60
        applySecond = totalPassed % 60
        onlySecond = totalTime - secondsPassed
        
        if secondsPassed <= totalTime {
            timeLabelLogic()
            secondsPassed += 1
        } else {
            finishSound()
            timeLabel.text = "FINISHED!".localized
            //setLanguage()
            timerEnd()
        }
    }
    
    func timeLabelLogic() {
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
        return 2
    }
    
    func pickerView(_ pickerView: UIPickerView, numberOfRowsInComponent component: Int) -> Int {
        switch component {
        case 0, 1:
            return 60
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
            return "\(row) "+minString.localized

        case 1:
            return "\(row) "+secString.localized

        default:
            return ""
        }
    }
    
    func pickerView(_ pickerView: UIPickerView, didSelectRow row: Int, inComponent component: Int) {
        switch component {
        case 0:
            minutes = row

        case 1:
            seconds = row
        default:
            break;
        }
    }
    
    func finishSound(){
        let url = Bundle.main.url(forResource: "Finish_alarm", withExtension: "wav")
        player = try! AVAudioPlayer(contentsOf: url!)
        player.play()
    }
    
    func timerEnd() {
        DispatchQueue.main.asyncAfter(deadline: .now() + 4) {
            self.timer.invalidate()
            self.timeLabel.text = "00:00"
            self.totalTime = 0
            self.secondsPassed = 0
            self.totalPassed = 0
            self.applyMinute = 0
            self.applySecond = 0
            self.onlySecond = 0
            self.player.stop()
        }
    }
    
    func localLang() {
        startLabel.setTitle("Start".localized, for: .normal)
        stopLabel.setTitle("Stop".localized, for: .normal)
        self.navigationItem.title = "Timer Mode".localized
        self.navigationItem.backButtonTitle = "Back".localized
    }
    
    func setLanguage() {
        
        // 설정된 언어 코드 가져오기
        let language = UserDefaults.standard.array(forKey: "AppleLanguages")?.first as! String
        let index = language.index(language.startIndex, offsetBy: 2)
        let languageCode = String(language[..<index]) //"ko" , "en" 등
        
        //설정된 언어 파일 가져오기
        let path = Bundle.main.path(forResource: languageCode, ofType: "lproj")
        let bundle = Bundle(path: path!)
        
        if timeLabel.text == "FINISHED" { bundle?.localizedString(forKey: "FINISHED!", value: nil, table: nil) }
        //timeLabel.text = bundle?.localizedString(forKey: "00:00", value: nil, table: nil)
        startLabel.setTitle(bundle?.localizedString(forKey: "Start", value: nil, table: nil), for: .normal)
        stopLabel.setTitle(bundle?.localizedString(forKey: "Stop", value: nil, table: nil), for: .normal)
        minString = (bundle?.localizedString(forKey: "Minute", value: nil, table: nil))!
        secString = (bundle?.localizedString(forKey: "Seconds", value: nil, table: nil))!
        navigationItem.title = bundle?.localizedString(forKey: "Timer Mode", value: nil, table: nil)
        navigationItem.backButtonTitle = bundle?.localizedString(forKey: "Back", value: nil, table: nil)
        
    }
}
