//
//  CountingModeViewController.swift
//  The Gym Hero
//
//  Created by Hoonsbrand on 12/14/21.
//

import UIKit
import AVFoundation

class CountingModeViewController: UIViewController {
    //var player: AVAudioPlayer!
    let setCountsColor = UIColor.white
    
    var countingBrain = CountingModeBrain()

    @IBOutlet weak var startLabel: UIButton!
    @IBOutlet weak var stopLabel: UIButton!
    @IBOutlet weak var countsNumber: UILabel!
    var timer = Timer()
    var totalCounts = 0
    var countsPassed = 0
    var secondsInterval: Float?
    var countsBox = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.countsNumber.numberOfLines = 0
        self.navigationItem.title = "Counting Mode".localized
        self.countsNumber.text = "Set Counts".localized
        localLang()
    }
    
    @IBAction func countsButton(_ sender: UIButton) {
        timer.invalidate()
        totalCounts = Int(sender.titleLabel!.text!)!
 
        let alert = UIAlertController(title: "Choose seconds Interval".localized, message: "Fear is just an illusion".localized, preferredStyle: .actionSheet)
        
        let oneSec = UIAlertAction(title: "2", style: .default) { (action) in
            self.secondsInterval = Float(alert.actions[0].title!)
        }
        let twoSec = UIAlertAction(title: "3", style: .default) { (action) in
            self.secondsInterval = Float(alert.actions[1].title!)
        }
        let threeSec = UIAlertAction(title: "4", style: .default) { (action) in
            self.secondsInterval = Float(alert.actions[2].title!)
        }
        let cancel = UIAlertAction(title: "cancel".localized, style: .cancel) {
            (cancel) in
        }
        alert.addAction(oneSec)
        alert.addAction(twoSec)
        alert.addAction(threeSec)
        alert.addAction(cancel)
        
        self.present(alert, animated: true, completion: nil)
    }
    
    @IBAction func startButton(_ sender: UIButton) {
        timer.invalidate()
        countsPassed = 0
        countsNumber.text = String(totalCounts)
        
        countTimer()
    }
    
    @IBAction func stopButton(_ sender: UIButton) {
        timer.invalidate()
        secondsInterval = 1.0
        countsPassed = 0
        countsNumber.text = "Set Counts".localized
        countsNumber.backgroundColor = setCountsColor
        countingBrain.player.stop()
    }
    
    @objc func updateCounts() {
        if countsPassed < totalCounts {
            countsNumber.text = String(totalCounts - countsPassed)
            countsPassed += 1
            countingBrain.countSound()
        } else {
            countingBrain.endSound()
            countsNumber.text = "Drink Some Water!".localized
            countsNumber.backgroundColor = UIColor.systemRed
            countsEnd()
        }
    }
    
    func countTimer() {
        timer = Timer.scheduledTimer(timeInterval: TimeInterval(secondsInterval!), target: self, selector: #selector(updateCounts), userInfo: nil, repeats: false)
        
        timer = Timer.scheduledTimer(timeInterval: TimeInterval(secondsInterval!), target: self, selector: #selector(updateCounts), userInfo: nil, repeats: true)
    }
    
    func countsEnd() {
        DispatchQueue.main.asyncAfter(deadline: .now() + 2) {
            self.timer.invalidate()
            self.secondsInterval = 1.0
            self.countsPassed = 0
            self.countsNumber.text = "Set Counts".localized
            self.countsNumber.backgroundColor = UIColor.white
            self.countingBrain.player.stop()
        }
    }
    
    func localLang() {
        startLabel.setTitle("Start".localized, for: .normal)
        stopLabel.setTitle("Stop".localized, for: .normal)
    }
    
//    func setLanguage() {
//
//        // 설정된 언어 코드 가져오기
//        let language = UserDefaults.standard.array(forKey: "AppleLanguages")?.first as! String
//        let index = language.index(language.startIndex, offsetBy: 2)
//        let languageCode = String(language[..<index]) //"ko" , "en" 등
//
//        //설정된 언어 파일 가져오기
//        let path = Bundle.main.path(forResource: languageCode, ofType: "lproj")
//        let bundle = Bundle(path: path!)
//
//        startLabel.setTitle(bundle?.localizedString(forKey: "Start", value: nil, table: nil), for: .normal)
//        stopLabel.setTitle(bundle?.localizedString(forKey: "Stop", value: nil, table: nil), for: .normal)
//        countsNumber.text = bundle?.localizedString(forKey: "Drink Some Water!", value: nil, table: nil)
//        countsNumber.text = bundle?.localizedString(forKey: "Set Counts", value: nil, table: nil)
//    }
    
}


