//
//  ViewController.swift
//  The Gym Hero
//
//  Created by Hoonsbrand on 12/14/21.
//

import UIKit

class ViewController: UIViewController {

    override func viewDidLoad() {
        super.viewDidLoad()
        title = "Select Mode"
        configureItems()
        
    }
    
    private func configureItems() {
        navigationItem.rightBarButtonItems = [
            UIBarButtonItem(
                barButtonSystemItem: .add,
                target: self,
                action: nil
            ),
            UIBarButtonItem(
                image: UIImage(systemName: "person.circle"),
                style: .done,
                target: self,
                action: #selector(ViewController.profileTapped)
            )
        ]
        
        navigationItem.leftBarButtonItem = UIBarButtonItem(image: UIImage(systemName: "gear"), style: .done, target: self, action: nil)
    }
    
    @objc func profileTapped(sender:UIBarButtonItem) {
        performSegue(withIdentifier: "ProfileEditPage", sender: nil)
    }
    
    @IBAction func TimerMode(_ sender: UIButton) {
        let vc = storyboard?.instantiateViewController(withIdentifier: "TimerMode") as! TimerModeViewController
        vc.title = "Timer Mode"
        navigationController?.pushViewController(vc, animated: true)
    }
    
    @IBAction func CountingMode(_ sender: UIButton) {
        let vc = storyboard?.instantiateViewController(withIdentifier: "CountingMode") as! CountingModeViewController
        vc.title = "Counting Mode"
        //vc.view.backgroundColor = .systemRed
        navigationController?.pushViewController(vc, animated: true)
    }
    
}

