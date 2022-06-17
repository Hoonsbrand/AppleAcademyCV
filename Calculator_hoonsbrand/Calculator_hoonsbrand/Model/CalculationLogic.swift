//
//  CalculationLogic.swift
//  Calculator_hoonsbrand
//
//  Created by Hoonsbrand on 5/27/22.
//

import Foundation
import UIKit

struct CalculationLogic {
    private var num: Double?
    
    mutating func setNum(_ num: Double) {
        self.num = num
    }
    
    var formula = ""
    var isRad = false
    var isOppositeSquared = false
    
    private var isParenthesesAtFirst = (isStartParenthese: false, isEndParenthese: false, isReadyToCalculate: false)
    private var leftParenthese = 0
    private var rightParenthese = 0
    
    private var tempMemory = ""
    private var memory = ""
    private var count = 0
    
    private var isYInRoot = false
    private var firstRoot: Double?
    
    private var isNewMethod = false
    
    private var eeCalc: (num: Double, isEEPressed: Bool)?
    
    private var logBase: Double?
    private var isBase = true
    
    private var isX = true
    private var xFromXY: Double?
    
    private var numWithCalcMethod: (n1: Double, calcMethod: String)?
    
    mutating func calculate(_ symbol: String, _ clearBtn: UIButton) -> Any? {
        print(formula)
        
        if isParenthesesAtFirst.isStartParenthese && isParenthesesAtFirst.isEndParenthese && !isParenthesesAtFirst.isReadyToCalculate && leftParenthese == rightParenthese {
            formula += "\(symbol)"
            isParenthesesAtFirst.isReadyToCalculate = true
            return nil
        }
        
        if let n = num {
            switch symbol {
                
            //MARK: - case Basics
            case "+/-":
                return n * -1
            case "%":
                return n * 0.01
                
            case "=":
                isNewMethod = true
                
                if !isBase {
                    return runLogCalculate(n)
                }
                
                if !isX {
                    return squaredCalculation(isOppositeSquared, n)
                }
                
                if eeCalc != nil {
                    return eeCalculate(n)
                }
                
                if isParenthesesAtFirst.isStartParenthese && isParenthesesAtFirst.isEndParenthese {
                    return parenthesesCalculation(n)
                }
                
                if isYInRoot {
                    isYInRoot = false
                    return rootCalculate(n)
                }
                
                if formula.contains("(") && formula.contains(")") {
                    return parenthesesCalculation()
                } else if formula.contains("(") && !formula.contains(")") {
                    formula += "\(String(n)))"
                    return parenthesesCalculation()
                }
                
                return runCalculation(n)
                
            //MARK: - case Factorial
            case "x!":
                var temp = 1
                if floor(n) != n {
                    return "오류"
                } else if n == 0 {
                    return "0"
                } else {
                    for i in 2...Int(n) {
                        temp *= i
                    }
                    return Double(temp)
                }
                
            //MARK: - case Parentheses
            case "(":
                leftParenthese += 1
                
                if formula == "" {
                    isParenthesesAtFirst.isStartParenthese = true
                }
                formula += "("
                tempMemory += "("
            case ")":
                rightParenthese += 1
                
                if isParenthesesAtFirst.isStartParenthese {
                    isParenthesesAtFirst.isEndParenthese = true
                }
                
                if leftParenthese == rightParenthese {
                    formula += ")"
                    return nil
                }
                
                if !formula.contains("(") {
                    return nil
                } else {
                    formula += "\(String(n)))"
                }
                
                if !tempMemory.contains("(") {
                    return nil
                } else {
                    tempMemory += "\(String(n)))"
                }
                
            //MARK: - case Memory
            case "m+":
                tempMemory += "\(String(n))"
                memoryAdd()
            case "m-":
                tempMemory += "\(String(n))"
                memorySubtract()
            case "mr":
                isNewMethod = true
                
                if formula.contains("(") && !formula.contains(")") {
                    tempMemory += ")"
                    return memoryCalculation()
                } else {
                    return nil
                }
                
            case "mc":
                isNewMethod = false
                formula = ""
                tempMemory = ""
                memory = ""
                count = 0
                                
            //MARK: - Case Squared
            case "x²":
                return pow(n,n)
            case "x³":
                return pow(n,3)
            case "xʸ", "yˣ":
                if isX {
                    xFromXY = n
                    isX = false
                } else {
                    isX = true
                    return squaredCalculation(isOppositeSquared,n)
                }
                
            //MARK: - case Squared & Root
            case "eˣ":
                return pow(exp(1), n)
            case "e":
                return exp(1.0)
            case "10ˣ":
                return pow(10, n)
            case "2ˣ":
                return pow(2, n)
            case "¹/𝗑":
                if !(1/n).isFinite {
                    return "오류"
                }
                return 1/n
            case "²√x":
                return pow(n, 1/2)
            case "³√x":
                return pow(n,1/3)
            case "ʸ√x":
                if isYInRoot {
                    isYInRoot = false
                    return rootCalculate(n)
                } else {
                    firstRoot = n
                    isYInRoot = true
                }
                
            //MARK: - case Log
            case "ln":
                return log(n)/log(exp(1))
            case "log₁₀":
                return log10(n)
            case "log𝚢":
                if isBase {
                    logBase = n
                    isBase = false
                } else {
                    return runLogCalculate(n)
                }
            case "log₂":
                return log2(n)
                
                
            case "EE":
                eeCalc = (num: n, isEEPressed: true)
                return nil
                
            case "π":
                return Double.pi
                
            //MARK: - case Angle Functions
            case "sin", "cos", "tan", "sinh", "cosh", "tanh":
                if let result = trigonometricFunction(symbol, n) {
                    if result.isNaN {
                        return "오류"
                    } else {
                        return result
                    }
                }
            case "sin⁻¹", "cos⁻¹", "tan⁻¹", "sinh⁻¹", "cosh⁻¹", "tanh⁻¹":
                if let result = inverseTrigonometricFunction(symbol, n) {
                    if result.isNaN {
                        return "오류"
                    } else {
                        return result
                    }
                }
           
            case "Rand":
                return Double.random(in: (0.0).nextUp..<1.0)
                
            //MARK: - default
            default:
                if !isX {
                    let newResult = squaredCalculation(isOppositeSquared,n)
                    tempMemory += "\(String(newResult!))\(symbol)"
                    formula += "\(String(newResult!))\(symbol)"
                    print(formula)
                    numWithCalcMethod = (n1: newResult!, calcMethod: symbol)
                    return newResult
                } else {
                   
                    tempMemory += "\(String(n))\(symbol)"
                    formula += "\(String(n))\(symbol)"
                    print(formula)
                    numWithCalcMethod = (n1: n, calcMethod: symbol)
                }
            }
        }
        return nil
    }
    
    func runLogCalculate(_ n2: Double) -> Double? {
        if let n1 = logBase {
            return logC(val: n1, forBase: n2)
        }
        return nil
    }
    
    func logC(val: Double, forBase base: Double) -> Double {
        return log(val)/log(base)
    }
    
    func inverseTrigonometricFunction(_ funcs: String, _ number: Double) -> Double? {
        print(isRad)
        switch funcs {
        case "sin⁻¹":
            return isRad ? asin(number) : InverseRad2Deg(asin(number))
        case "cos⁻¹":
            return isRad ?  acos(number) : InverseRad2Deg(acos(number))
        case "tan⁻¹":
            return isRad ? atan(number) : InverseRad2Deg(atan(number))
        case "sinh⁻¹":
            return isRad ? asinh(number) : InverseRad2Deg(asinh(number))
        case "cosh⁻¹":
            return isRad ? acosh(number) : InverseRad2Deg(acosh(number))
        case "tanh⁻¹":
            return isRad ? atanh(number) : InverseRad2Deg(atanh(number))
        default:
            return nil
        }
    }
    
    func trigonometricFunction(_ funcs: String, _ number: Double) -> Double? {
        print(isRad)
        switch funcs {
        
        case "sin":
            return isRad ? sin(number) : sin(rad2Deg(number))
        case "cos":
            return isRad ?  cos(number) : cos(rad2Deg(number))
        case "tan":
            return isRad ? tan(number) : tan(rad2Deg(number))
        case "sinh":
            return isRad ? sinh(number) : sinh(rad2Deg(number))
        case "cosh":
            return isRad ? cosh(number) : cosh(rad2Deg(number))
        case "tanh":
            return isRad ? tanh(number) : tanh(rad2Deg(number))
        default:
            return nil
        }
    }
    
    func rad2Deg(_ number: Double) -> Double {
        return number * (.pi / 180)
    }
    
    func InverseRad2Deg(_ number: Double) -> Double {
        return number * (180.0 / .pi)
    }
    
    
    //MARK: - EE Method
    mutating func eeCalculate(_ n2: Double) -> Double? {
        if let n1 = eeCalc?.num {
            return n1 * (pow(10, n2))
        }
        
        eeCalc?.isEEPressed = false
        return nil
    }
    
    //MARK: - Root Method
    
    func rootCalculate(_ n2: Double) -> Double? {
       
        if let x = firstRoot {
            return pow(x,1/n2)
        }
        return nil
    }
    
    
    //MARK: - Custom Squared Method
    func squaredCalculation(_ isOpposite: Bool,_ n2: Double) -> Double? {
        if let n1 = xFromXY {
            return isOpposite ? pow(n2, n1) : pow(n1, n2)
        }
        return nil
    }
    
    //MARK: - Memory Method
    
    mutating func memorySubtract() {
        if count == 0 {
            memory += "(\(tempMemory))"
            tempMemory = ""
            print(memory)
        } else {
            memory += "-(\(tempMemory))"
            tempMemory = ""
            print(memory)
        }
        count += 1
    }
    
    mutating func memoryAdd() {
        if count == 0 {
            memory += "(\(tempMemory))"
            tempMemory = ""
            print(memory)
        } else {
            memory += "+(\(tempMemory))"
            tempMemory = ""
            print(memory)
        }
        count += 1
    }
    
    mutating func memoryCalculation() -> Double? {
        print(memory)
        if memory.contains("x") {
            memory = memory.replacingOccurrences(of: "x", with: "*")
        } else if memory.contains("÷") {
            memory = memory.replacingOccurrences(of: "÷", with: "/")
        }
        
        let expression = NSExpression(format:memory)
        let value = expression.expressionValue(with: nil, context: nil) as? Double
    
        memory = ""
        formula = ""
        count = 0
        isNewMethod = true
        return value
    }
    
    //MARK: - Basic Method
    
    mutating func parenthesesCalculation(_ firstParen: Double? = 0.0) -> Double? {
        
        if isParenthesesAtFirst.isStartParenthese && isParenthesesAtFirst.isEndParenthese && isParenthesesAtFirst.isReadyToCalculate {
            formula += String(firstParen!)
            isParenthesesAtFirst = (isStartParenthese: false, isEndParenthese: false, isReadyToCalculate: false)
        }
        
        if formula.contains("x") {
            formula = formula.replacingOccurrences(of: "x", with: "*")
        } else if formula.contains("÷") {
            formula = formula.replacingOccurrences(of: "÷", with: "/")
        }
        
        let expression = NSExpression(format:formula)
        let value = expression.expressionValue(with: nil, context: nil) as? Double
        print(formula)
        formula = ""
        tempMemory = ""
        return value
    }
    
    mutating func runCalculation(_ n2: Double) -> Double? {
        isNewMethod = true
        formula = ""
        tempMemory = ""
        
        if let n1 = numWithCalcMethod?.n1 {
            if let calcMethod = numWithCalcMethod?.calcMethod {
  
                switch calcMethod {
                    
                case "÷":
                    return n1 / n2
                case "x":
                    return n1 * n2
                case "-":
                    return n1 - n2
                case "+":
                    return n1 + n2
                    
                default:
                    fatalError("Error occured at runCalculation")
                }
            }
        }
        return nil
    }
}

// ( (2+3) x 2 ) x 2 = 20
