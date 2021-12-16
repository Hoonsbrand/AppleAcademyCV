//
//  ViewController.swift
//  Xylophone
//
//  Created by Angela Yu on 28/06/2019.
//  Copyright © 2019 The App Brewery. All rights reserved.
//

import UIKit
import AVFoundation

class ViewController: UIViewController {
    
    var player: AVAudioPlayer!

    override func viewDidLoad() {
        super.viewDidLoad()
    }

    @IBAction func keyPressed(_ sender: UIButton) {
        
//        if let buttonTitle = sender.title(for: .normal) {
//            print(buttonTitle)
//        } else{
//            print("nil")
//        }

//        print(sender.currentTitle)
        sender.alpha = 0.5
        playSound(soundName: sender.currentTitle ?? "C")
        
        // ! : Don't worry we've already checked
        //playSound(soundName: sender.currentTitle!)
        
        DispatchQueue.main.asyncAfter(deadline: .now() + 0.2) {
            print("start")
            print("end")
            
            sender.alpha = 1.0
        }
    }
    
    func playSound(soundName: String) {
        let url = Bundle.main.url(forResource: soundName, withExtension: "wav")
        player = try! AVAudioPlayer(contentsOf: url!)
        player.play()
                
    }
}


