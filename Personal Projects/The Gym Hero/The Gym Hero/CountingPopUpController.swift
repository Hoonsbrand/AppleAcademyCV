//
//  CountingPopUpController.swift
//  The Gym Hero
//
//  Created by Hoonsbrand on 12/14/21.
//

import UIKit


class CountingPopUpController: UIViewController {
    
    @IBOutlet weak var valueLabel: UILabel!
    
    var sendInterval: Float = 1.0
    
    
    override func viewDidLoad() {
        super.viewDidLoad()
    }
    
    @IBAction func submitValue(_ sender: UIButton) {
        //sendInterval.secondsInterval = Int(valueLabel.text!)!
        let secondsInterval = Int(valueLabel.text!)!
        switch secondsInterval {
        case 1:
            sendInterval = 1
        case 2:
            sendInterval = 2
        case 3:
            sendInterval = 3
        default:
            sendInterval = 1.0
        }
    }
    
    @IBAction func cancelButton(_ sender: UIButton) {
    }
    
    @IBAction func sliderValueChanged(_ sender: UISlider) {
        let value = Int(sender.value)
        valueLabel.text = String(value)
    }
    
}
