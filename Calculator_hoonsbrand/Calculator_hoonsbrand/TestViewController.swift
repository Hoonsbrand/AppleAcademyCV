//
//  TestViewController.swift
//  Calculator_hoonsbrand
//
//  Created by Hoonsbrand on 6/1/22.
//

import UIKit

class TestViewController: UIViewController {
    
    @IBOutlet var collection: [UIStackView]!
    
    private var isLandscape = Bool()
    
    override func viewDidAppear(_ animated: Bool) {
        if let appDelegate = UIApplication.shared.delegate as? AppDelegate {
            rotate(appDelegate.isLandscape)
        }
    }
    
    override func viewWillTransition(to size: CGSize, with coordinator: UIViewControllerTransitionCoordinator) {
        rotate()
    }
    
    private func rotate(_ isLandscapeRotation: Bool? = UIDevice.current.orientation.isLandscape) {
        
        if isLandscapeRotation! {
            for c in collection {
                c.isHidden = false
            }
        } else {
            for c in collection {
                c.isHidden = true
            }
        }
    }
}
