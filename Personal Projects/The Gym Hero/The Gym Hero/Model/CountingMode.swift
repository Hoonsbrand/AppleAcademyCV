//
//  CountingMode.swift
//  The Gym Hero
//
//  Created by Hoonsbrand on 1/3/22.
//

import Foundation

struct CountingMode {
    var timer = Timer()
    var totalCounts = 0
    var countsPassed = 0
    var secondsInterval: Float?
    var countsBox = 0
    
    init(totalCounts: Int, countsPassed: Int, secondsInterval: Float, countsBox: Int) {
        self.totalCounts = totalCounts
        self.countsPassed = countsPassed
        self.secondsInterval = secondsInterval
        self.countsBox = countsBox
    }
}
