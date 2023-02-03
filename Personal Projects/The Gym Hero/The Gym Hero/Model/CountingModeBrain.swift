//
//  CountingModeBrain.swift
//  The Gym Hero
//
//  Created by Hoonsbrand on 1/3/22.
//

import Foundation
import UIKit
import AVFoundation

struct CountingModeBrain {
    
    var countsPassed = 0
    var totalCounts = 0
    var countingModeViewController = CountingModeViewController()
    
    var player: AVAudioPlayer!

    mutating func countSound(){
        let url = Bundle.main.url(forResource: "C", withExtension: "wav")
        player = try! AVAudioPlayer(contentsOf: url!)
        player.play()
    }
    
    mutating func endSound(){
        let url = Bundle.main.url(forResource: "alarm_sound", withExtension: "mp3")
        player = try! AVAudioPlayer(contentsOf: url!)
        player.play()
    }
}
