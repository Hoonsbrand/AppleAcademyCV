//
//  CountingModeViewController.swift
//  The Gym Hero
//
//  Created by Hoonsbrand on 12/14/21.
//

import UIKit
import AVFoundation


class CountingModeViewController: UIViewController{
    var player: AVAudioPlayer!

    @IBOutlet weak var countsNumber: UILabel!
    var timer = Timer()
    var totalCounts = 0
    var countsPassed = 0
    var secondsInterval: Float?
    var countsBox = 0
    
    override func viewDidLoad() {
        super.viewDidLoad()
        self.countsNumber.numberOfLines = 0
    }
    
    @IBAction func countsButton(_ sender: UIButton) {
        timer.invalidate()
        totalCounts = Int(sender.titleLabel!.text!)!
        
//        let alert = UIAlertController(title: "Input seconds Interval", message: "Fear is just an illusion", preferredStyle: .alert)
//        alert.addTextField { (myTextField) in
//            myTextField.placeholder = "Please Input only Integer! (1~)"
//        }
//        let ok = UIAlertAction(title: "OK", style: .default) {
//            (ok) in
//            self.secondsInterval = Float(alert.textFields![0].text!)
//            self.countsNumber.text = "Press Start!"
//        }
//        let cancel = UIAlertAction(title: "cancel", style: .cancel) {
//            (cancel) in
//        }
//        alert.addAction(cancel)
//        alert.addAction(ok)
//        self.present(alert, animated: true, completion: nil)
        
        let alert = UIAlertController(title: "Choose seconds Interval", message: "Fear is just an illusion", preferredStyle: .actionSheet)
        
        let oneSec = UIAlertAction(title: "1", style: .default) { (action) in
            self.secondsInterval = Float(alert.actions[0].title!)
        }
        
        let twoSec = UIAlertAction(title: "2", style: .default) { (action) in
            self.secondsInterval = Float(alert.actions[1].title!)
        }
        let threeSec = UIAlertAction(title: "3", style: .default) { (action) in
            self.secondsInterval = Float(alert.actions[2].title!)
        }
        let cancel = UIAlertAction(title: "cancel", style: .cancel) {
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
        countsNumber.text = "Set Counts"
        countsNumber.backgroundColor = UIColor.systemBlue
        player.stop()
    }
    
    @objc func updateCounts(){
        if countsPassed <= totalCounts {
            countsNumber.text = String(totalCounts - countsPassed)
            countsPassed += 1
            countSound()
        } else {
            endSound()
            countsNumber.text = "Drink Some Water!"
            countsNumber.backgroundColor = UIColor.systemRed
            countsEnd()
        }
    }
    
    func countTimer() {
        timer = Timer.scheduledTimer(timeInterval: 1, target: self, selector: #selector(updateCounts), userInfo: nil, repeats: false)
        
        timer = Timer.scheduledTimer(timeInterval: TimeInterval(secondsInterval!), target: self, selector: #selector(updateCounts), userInfo: nil, repeats: true)
    }
    
    func countSound(){
        let url = Bundle.main.url(forResource: "C", withExtension: "wav")
        player = try! AVAudioPlayer(contentsOf: url!)
        player.play()
    }
    
    func endSound(){
        let url = Bundle.main.url(forResource: "alarm_sound", withExtension: "mp3")
        player = try! AVAudioPlayer(contentsOf: url!)
        player.play()
    }
    
    func countsEnd() {
        DispatchQueue.main.asyncAfter(deadline: .now() + 2) {
            self.timer.invalidate()
            self.secondsInterval = 1.0
            self.countsPassed = 0
            self.countsNumber.text = "Set Counts"
            self.countsNumber.backgroundColor = UIColor.systemBlue
            self.player.stop()
        }
    }
    
}


