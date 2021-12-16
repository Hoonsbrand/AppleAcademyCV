//
//  ViewController.swift
//  Quizzler-iOS13
//
//  Created by Angela Yu on 12/07/2019.
//  Copyright © 2019 The App Brewery. All rights reserved.
//

import UIKit

class ViewController: UIViewController {
    
    @IBOutlet weak var scoreLabel: UILabel!
    @IBOutlet weak var questionLabel: UILabel!
    @IBOutlet weak var progressBar: UIProgressView!
    @IBOutlet weak var firstButton: UIButton!
    @IBOutlet weak var secondButton: UIButton!
    @IBOutlet weak var thirdButton: UIButton!
    
    var buttonArray: [UIButton] = [UIButton]()  // 이니셜라이저를 사용하여 빈 배열 생성
    var quizBrain = QuizBrain()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        buttonArray = [firstButton, secondButton, thirdButton]
        updateUI()
    }

    // 실질적인 button의 기능 구현
    @IBAction func answerButtonPressed(_ sender: UIButton) {
        
        let userAnswer = sender.currentTitle! // True, False
        let userGotItRight = quizBrain.checkAnswer(userAnswer)
        
        if userGotItRight == true {
            sender.backgroundColor = UIColor.green
        } else{
            sender.backgroundColor = UIColor.red
        }
        
        quizBrain.nextQuestion()
        
        Timer.scheduledTimer(timeInterval: 0.2, target: self, selector: #selector(updateUI), userInfo: nil, repeats: false)
    }
    
    @objc func updateUI() {
        updateAnswerButton()
        questionLabel.text = quizBrain.getQuestionText()
        progressBar.progress = quizBrain.getProgress()
        scoreLabel.text = "Score : \(quizBrain.getScore())"
        clearButton()
    }
    
    func clearButton(){
        firstButton.backgroundColor = UIColor.clear
        secondButton.backgroundColor = UIColor.clear
        thirdButton.backgroundColor = UIColor.clear
    }
    
    func updateAnswerButton(){
        let answerOptions = quizBrain.getAnswers()
        
        for i in 0..<buttonArray.count{
            buttonArray[i].setTitle(answerOptions[i], for: .normal)
        }
    }
    
//    func updateAnswerButton(){
//        for i in 0..<buttonArray.count{
//            buttonArray[i].setTitle(quizBrain.quiz[quizBrain.questionNumber].answer[i], for: .normal)
//        }
//    }
    
}

