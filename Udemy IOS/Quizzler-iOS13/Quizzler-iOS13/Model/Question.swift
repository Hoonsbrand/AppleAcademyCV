//
//  Question.swift
//  Quizzler-iOS13
//
//  Created by Hoonsbrand on 12/9/21.
//  Copyright © 2021 The App Brewery. All rights reserved.
//

import Foundation

struct Question{
    let text: String
    let answers: Array<String>
    let correctAnswer : String
    
    init(q: String, a: Array<String>, correctAnswer: String){
        text = q
        answers = a
        self.correctAnswer = correctAnswer
    }
}
