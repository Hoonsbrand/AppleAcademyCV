//
//  ViewController.swift
//  Calculator_hoonsbrand
//
//  Created by Hoonsbrand on 5/27/22.
//

import UIKit

class CalculatorViewController: UIViewController {
    
    @IBOutlet weak var displayLabel: UILabel!
    @IBOutlet weak var clearBtn: UIButton!
    @IBOutlet var collection: [UIStackView]!

    @IBOutlet weak var eBtn: UIButton!
    @IBOutlet weak var squaredBtn: UIButton!
    @IBOutlet weak var lnBtn: UIButton!
    @IBOutlet weak var logBtn: UIButton!
    @IBOutlet weak var sinBtn: UIButton!
    @IBOutlet weak var cosBtn: UIButton!
    @IBOutlet weak var tanBtn: UIButton!
    @IBOutlet weak var sinhBtn: UIButton!
    @IBOutlet weak var coshBtn: UIButton!
    @IBOutlet weak var tanhBtn: UIButton!
    
    @IBOutlet weak var radBtn: UIButton!
    
    private var isLandscape = Bool()
    private var isFinishedTypingNumber = true
    private var isNewInput = true
    private var calculationLogic = CalculationLogic()
    
    private var tempDouble = Double()
    
    private var isSecondBtnPressed = false
    
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
    
    private func setDisplayedNum(_ inputNum: Any?) {
        if let num = inputNum {
            displayLabel.text = num as? String
        }
        else {
            displayLabel.text = "오류"
        }
    }
    
    private func numberFormatter(_ newNum: Double) -> String{
        let formatter = NumberFormatter()
        formatter.maximumFractionDigits = 6
        formatter.minimumFractionDigits = 0
        formatter.numberStyle = .decimal
        
        return formatter.string(for: newNum)!
    }

    var displayValue: Any {
        get {
            if let number = Double(displayLabel.text!) {
                return number
            } else {
                return 0
            }
        }
        set {
            if type(of: newValue) == String.self {
                displayLabel.text = newValue as? String
            } else {
                tempDouble = newValue as! Double
                if floor(tempDouble) == tempDouble {
                    displayLabel.text = tempDouble.clean
                } else {
                    if tempDouble < 1 {
                        // 부동소숫점 검색
                        displayLabel.text = numberFormatter(newValue as! Double)
                    } else {
                        displayLabel.text = String(tempDouble)
                    }
                }
            }
        }
    }
    
    @IBAction func calcButtonPressed(_ sender: UIButton) {
        isFinishedTypingNumber = true
        isNewInput = true
        
        if sender.currentTitle == "C" || sender.currentTitle == "AC" {
            clearBtn.setTitle("AC", for: .normal)
            displayValue = 0.0
            calculationLogic.formula = ""
            return
        }
        
        if sender.currentTitle == "2ⁿᵈ" {
            secondButtonPressed()
            return
        }
        
        if sender.currentTitle == "Rad" || sender.currentTitle == "Deg" {
            calculationLogic.isRad = !calculationLogic.isRad

            if !calculationLogic.isRad {
                radBtn.setTitle("Rad", for: .normal)
            } else {
                radBtn.setTitle("Deg", for: .normal)
            }
            return
        }
        
        calculationLogic.setNum(displayValue as! Double)
        
        if let calcMethod = sender.currentTitle {
            
            if let result = calculationLogic.calculate(calcMethod, clearBtn) {
                
                if type(of: result) == String.self {
                    displayValue = result as! String
                } else {
                    displayValue = result as! Double
                }
            }
        }
    }
    
    @IBAction func numButtonPressed(_ sender: UIButton) {
        clearBtn.setTitle("C", for: .normal)
        
        if let numValue = sender.currentTitle {
            
            if isFinishedTypingNumber {
                if numValue == "." && isNewInput {
                    displayLabel.text = "0."
                    isFinishedTypingNumber = false
                    isNewInput = false
                } else {
                    displayLabel.text = numValue
                    isFinishedTypingNumber = false
                    isNewInput = false
                }
            } else {
                if numValue == "." {
                    guard !displayLabel.text!.contains(".") else {
                        return
                    }
                }
                displayLabel.text = displayLabel.text! + numValue
            }
        }
    }
    
    func secondButtonPressed() {
        isSecondBtnPressed = !isSecondBtnPressed
        calculationLogic.isOppositeSquared = !calculationLogic.isOppositeSquared
        
        if isSecondBtnPressed {
            eBtn.setTitle("yˣ", for: .normal)
            squaredBtn.setTitle("2ˣ", for: .normal)
            lnBtn.setTitle("log𝚢", for: .normal)
            logBtn.setTitle("log₂", for: .normal)
            sinBtn.setTitle("sin⁻¹", for: .normal)
            cosBtn.setTitle("cos⁻¹", for: .normal)
            tanBtn.setTitle("tan⁻¹", for: .normal)
            sinhBtn.setTitle("sinh⁻¹", for: .normal)
            coshBtn.setTitle("cosh⁻¹", for: .normal)
            tanhBtn.setTitle("tanh⁻¹", for: .normal)
        } else {
            eBtn.setTitle("eˣ", for: .normal)
            squaredBtn.setTitle("10ˣ", for: .normal)
            lnBtn.setTitle("ln", for: .normal)
            logBtn.setTitle("log₁₀", for: .normal)
            sinBtn.setTitle("sin", for: .normal)
            cosBtn.setTitle("cos", for: .normal)
            tanBtn.setTitle("tan", for: .normal)
            sinhBtn.setTitle("sinh", for: .normal)
            coshBtn.setTitle("cosh", for: .normal)
            tanhBtn.setTitle("tanh", for: .normal)
        }
    }
}

extension Double {
    var clean: String {
       return self.truncatingRemainder(dividingBy: 1) == 0 ? String(format: "%g", self) : String(self)
    }
}

// guard .을 포함하지 않으면? 추가, 포함하면? -> reutrn
// .이 있으면 다음으로 숫자를 눌러도 else return 때문에 숫자가 추가되지 않음.
// numValue 가 "."일 때만 체크하기

